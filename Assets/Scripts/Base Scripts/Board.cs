using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public enum GameState {
    wait, move, win, lose, pause
}

public enum TileType {
    breakable, blank, normal 
}

[System.Serializable]
public class Tile { 
    public int x; 
    public int y; 
    public TileType tile; 


}

public class Board : MonoBehaviour {

    [Header("World")]
    [SerializeField] internal World world; 
    [SerializeField] private int lvl; 

    [Header("Board properites")]
    public GameState currentState = GameState.pause; 
    [SerializeField] private int width = 7; 
    [SerializeField] private int height = 10;  
    [SerializeField] private int offset = 20; 
    [SerializeField] private int eyeRatio = 0; // must be between 0 and 100 
    public int balance = 500; // controls the pace at which the game moves

    private BackgroundTile[,] allTiles; 
    [SerializeField] private GameObject[,] allDots; 
    internal FindMatches findMatches; 
    internal ScoreManager scoreManager; 
    internal SoundManager soundManager; 
    internal FloatingTextManager floatingTextManager; 
    private int streakValue = 1; 

    [Header("Board components")]
    [SerializeField] private Dot currentDot; 
    [SerializeField] private Image moveIndicatorImage;  
    private List<Color> moveColors; 
    [SerializeField] private Tile[] boardLayout; 
    [SerializeField] private BackgroundTile[,] breakableTiles; 
    [SerializeField] private GameObject tilePrefab; 
    [SerializeField] private GameObject breakableTilePrefab; 
    [SerializeField] private GameObject destroyEffect; 
    [SerializeField] private float particleLifetime = 0.5f; 
    private float refillDelay = 0.5f; 

    [Header("Dot types")]
    [SerializeField] private GameObject[] dots; 
    [SerializeField] private GameObject[] eyes; 

    // Awake. It's probably before start, right? 
    private void Awake() {
        lvl = PlayerPrefs.GetInt("CurrentLevel", 0); 
        if (world != null) {
            if (world.levels[lvl] != null) {
                Level myLvl = world.levels[lvl]; 
                width = myLvl.width; 
                height = myLvl.height; 
                boardLayout = myLvl.boardLayout; 
                dots = myLvl.dots; 
                eyes = myLvl.eyes; 
                balance = myLvl.balance; 
                bool summoned = false; // change after implimenting loading from save data
                if (summoned) {
                    eyeRatio = myLvl.eyeRatio; 
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start() {

        // initialize arrays
        allTiles = new BackgroundTile[width, height]; 
        allDots = new GameObject[width, height]; 
        breakableTiles = new BackgroundTile[width, height]; 

        // peer objects
        findMatches = gameObject.GetComponent<FindMatches>(); 
        scoreManager = gameObject.GetComponent<ScoreManager>(); 
        soundManager = gameObject.GetComponentInChildren<SoundManager>(); 
        floatingTextManager = gameObject.GetComponentInChildren<FloatingTextManager>(); 

        // move colors 
        moveColors = new List<Color>(); 
        moveColors.Add(new Color(0f, 0.872f, 0.1591406f, 1f)); // move, green
        moveColors.Add(new Color(1f, 0.6806262f, 0f, 1f)); // wait, orange
        moveColors.Add(new Color(0.1933962f, 0.86737f, 1f, 1f)); // none, blue

        SetUp(); 

        currentState = GameState.pause; 
    }

    /// <summary>Creates board, tiles, and dots on board </summary>
    private void SetUp() {

        // set up board
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                Vector2 tempPos = new Vector2(i, j); 
                // create a backgroundTile
                GameObject backgroundTile = Instantiate(tilePrefab, tempPos, Quaternion.identity); 
                backgroundTile.transform.parent = this.transform; 
                backgroundTile.name = "tile (" + i + ", " + j + ")"; 

                // create a dot, pick a random type, check for matches, and add it to the array 
                tempPos = new Vector2(i, j + offset); 
                int dotToUse = Random.Range(0, dots.Length);
                int maxIterations = 0; 
                while(MatchesAt(i, j, dots[dotToUse]) && maxIterations < 100) {
                    dotToUse = Random.Range(0, dots.Length);
                    maxIterations++; 
                }
                maxIterations = 0; 

                GameObject dotObj = Instantiate(dots[dotToUse], tempPos, Quaternion.identity);
                Dot dot = dotObj.GetComponent<Dot>(); 
                dot.setX(i); 
                dot.setY(j); 
                dot.updatePrevXY(); 
                dotObj.transform.parent = this.transform; 
                //dot.name = "dot (" + i + ", " + j + ")";
                allDots[i, j] = dotObj; 
            }
        }
        GenerateBlankTiles(); 
        GenerateBreakableTiles(); 
    }

    void Update() {
        if (currentState == GameState.move) {
            moveIndicatorImage.color = moveColors[0]; 
        }
        else if(currentState == GameState.wait) {
            moveIndicatorImage.color = moveColors[1]; 
        }
        else {
            moveIndicatorImage.color = moveColors[2]; 
        }

        if (Input.GetKeyDown("s")) {
            ShuffleBoard();
        }
    }

    /// <summary>Generates a breakable tile at all positions marked breakable</summary>
    private void GenerateBreakableTiles() {
        // look at all tiles in layout
        for (int i = 0; i < boardLayout.Length; i++) {
            // if tile is breakable
            if (boardLayout[i].tile == TileType.breakable) {
                // create breakable tile at that position 
                Vector2 tempPos = new Vector2(boardLayout[i].x, boardLayout[i].y); 
                GameObject tile = Instantiate(breakableTilePrefab, tempPos, Quaternion.identity);
                breakableTiles[boardLayout[i].x, boardLayout[i].y] = tile.GetComponent<BackgroundTile>(); 
                tile.transform.parent = this.transform;  
            }
        }
    }

    private void GenerateBlankTiles() {
        // generate blank tiles
    }

    /// <summary>Returns true if adding this dot in this position would generate a match</summary>
    private bool MatchesAt(int x, int y, GameObject dot) {
        if (x > 1 && y > 1) {
            if (allDots[x - 1, y] != null && allDots[x - 2, y] != null){
                if (allDots[x - 1, y].tag == dot.tag && allDots[x - 2, y].tag == dot.tag) {
                return true; 
                }
            }
            if (allDots[x, y - 1] != null && allDots[x, y - 2] != null) {
                if (allDots[x, y - 1].tag == dot.tag && allDots[x, y - 2].tag == dot.tag) {
                    return true; 
                } 
            }
        }
        else if (x <= 1 || y <= 1) {
            if (y > 1) {
                if (allDots[x, y - 1] != null && allDots[x, y - 2] != null) {
                    if (allDots[x, y - 1].tag == dot.tag && allDots[x, y - 2].tag == dot.tag) {
                        return true; 
                    }
                }
            }
            else if (x > 1) {
                if (allDots[x - 1, y] != null && allDots[x - 2, y] != null) {
                    if (allDots[x - 1, y].tag == dot.tag && allDots[x - 2, y].tag == dot.tag) {
                        return true; 
                    }
                }
            }
        }
        return false; 
    }

    /// <summary>Checks findMatches for bomb-making, adds score, breaks breakable tiles, then destroys matched dot</summary>
    private void DestroyMatchesAt(int x, int y) {
        Dot dot = allDots[x, y].GetComponent<Dot>(); 
        if (dot.isMatched()) {

            // checks if a tile needs to be broken, then breaks it
            if (breakableTiles[x, y] != null) {
                breakableTiles[x, y].TakeDmg(1); 
                if (breakableTiles[x, y].getHp() <= 0) {
                   breakableTiles[x, y] = null; 
                }
            }

            if (scoreManager != null) {
                // add the broken dot to the score 
                scoreManager.IncreaseScore(dot.getPoints() * streakValue); 
                scoreManager.compareGoal(allDots[x, y].tag); 
                scoreManager.UpdateGoals(); 
            }

            // does sound manager exist? 
            if (soundManager != null) {
                soundManager.PlayDestroyNoise(); 
            }
            // create a dot destroy particle then destroy the dot
            GameObject particle = Instantiate(destroyEffect, allDots[x, y].transform.position, Quaternion.identity);
            Destroy(particle, particleLifetime); 
            if (allDots[x, y] != null) {
                Destroy(allDots[x, y]); 
            }
            allDots[x, y] = null; 
        }
    }

    /// <summary>Loops through the entire board and destroys matches</summary>
    public void DestroyMatches() {
        if (currentDot != null) {
            findMatches.makeBomb(); 
        }
        
        findMatches.getCurrentMatches().Clear();
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                if (allDots[i, j] != null) {
                    DestroyMatchesAt(i, j); 
                }
            }
        }
        StartCoroutine(DecreaseRowCo()); 
    }

    /// <summary>Makes dots fall when the dot under them is null</summary>
    private IEnumerator DecreaseRowCo() {
        yield return new WaitForSeconds(refillDelay * 0.5f); 
        int nullCount = 0; 
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                if (allDots[i, j] == null) {
                    nullCount++; 
                }
                else if (nullCount > 0) {
                    allDots[i, j].GetComponent<Dot>().setY(allDots[i, j].GetComponent<Dot>().getY() - nullCount); 
                    allDots[i, j] = null; 
                }
            }
            nullCount = 0; 
        }
        yield return new WaitForSeconds(refillDelay); 

        StartCoroutine(FillBoardCo()); 
    }

    /// <summary>Refills dots from the top of the board</summary>
    private void RefillBoard() {
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                if (allDots[i, j] == null) {
                    Vector2 tempPos = new Vector2(i, j + offset); 
                    int dice = Random.Range(0, 100); 
                    int dotToUse = Random.Range(0, dots.Length); 
                    GameObject chosen = null; 
                    if (dice >= eyeRatio) {
                        chosen = dots[dotToUse]; 
                    }
                    else {
                        chosen = eyes[dotToUse]; 
                    }
                    GameObject dot = Instantiate(chosen, tempPos, Quaternion.identity); 
                    dot.transform.parent = this.transform; 
                    allDots[i, j] = dot; 
                    dot.GetComponent<Dot>().setX(i); 
                    dot.GetComponent<Dot>().setY(j); 
                    dot.GetComponent<Dot>().updatePrevXY(); 
                }
            }
        }
    }

    /// <summary>Loops through board and determine if there are still matches on board</summary>
    private bool MatchesOnBoard() {
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                if (allDots[i, j] != null) {
                    if (allDots[i, j].GetComponent<Dot>().isMatched()) {
                        return true; 
                    }
                }
            }
        }
        return false; 
    }

    /// <summary>If there are still matches on the board, destroy them</summary>
    private IEnumerator FillBoardCo() {
        RefillBoard(); 
        yield return new WaitForSeconds(refillDelay * 0.5f); 

        while(MatchesOnBoard()) {
            streakValue ++; 
            DestroyMatches(); 
            yield return new WaitForSeconds(refillDelay); 
        }
        findMatches.getCurrentMatches().Clear(); 
        currentDot = null; 

        if (isDeadLocked()) {
            Debug.Log("Deadlocked!"); 
            ShuffleBoard(); 
        }

        yield return new WaitForSeconds(refillDelay); 
        if (currentState == GameState.wait) {
            currentState = GameState.move; 
        }
        streakValue = 1; 
    }

    public void SwitchPieces(int x, int y, Vector2 dir) {
        // take first piece and save it in holder 
        GameObject holder = allDots[x + (int)dir.x, y + (int)dir.y] as GameObject; 
        allDots[x + (int)dir.x, y + (int)dir.y] = allDots[x, y];
        allDots[x, y] = holder; 
    }

    private bool isDeadLocked() {
        for (int i = 0; i < width - 2; i++) {
            for (int j = 0; j < height - 2; j++) {
                if (allDots[i, j] != null) {
                    if (findMatches.SwitchAndCheck(i, j, Vector2.right)) {
                        // not deadlocked
                        return false; 
                    } 
                    if (findMatches.SwitchAndCheck(i, j, Vector2.up)) {
                        // not deadlocked
                        return false; 
                    }
                }
            }
        }
        // is deadlocked
        return true; 
    }

    /// <summary>Records every piece on the board, randomly suffles them, and then, if deadlocked, calls ShuffleBoard() again.</summary>
    public void ShuffleBoard() {
        // Create a list of dots on the board 
        List<GameObject> newBoard = new List<GameObject>(); 

        // Add every piece to this list 
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                if (allDots[i, j] != null) {
                    newBoard.Add(allDots[i, j]); 
                }
            }
        }

        //for ever spot on the board . . . 
        for (int k = 0; k < width; k++) {
            for (int l = 0; l < height; l++) {
                // pick random number (dot)
                int dotToUse = Random.Range(0, newBoard.Count); 
                // Make sure using this dot here doesn't create a match 
                int maxIterations = 0; 
                while(MatchesAt(k, l, newBoard[dotToUse]) && maxIterations < 100) {
                    dotToUse = Random.Range(0, newBoard.Count);
                    maxIterations++; 
                }
                maxIterations = 0; 
                // assign new x and y for dot
                newBoard[dotToUse].GetComponent<Dot>().setX(k); 
                newBoard[dotToUse].GetComponent<Dot>().setY(l); 
                // put new dot on its place on the board
                allDots[k, l] = newBoard[dotToUse]; 
                // remove dot from pool of unassigned dots
                newBoard.Remove(newBoard[dotToUse]); 
            }
        }

        // check for deadlock 
        if (isDeadLocked()) {
            ShuffleBoard(); 
        }
    }

    private IEnumerator WaitForSecondsCo() {
        yield return new WaitForSeconds(0.5f); 
    }

/********************* Public Getters and Setters *********************/ 

    /// <summary> gets a dot given its <paramref name="x"/> and <paramref name="y"/> </summary>
    /// <param name="x">x-vlaue</param>
    /// <param name="y">y-value</param>
    public GameObject getDot(int x, int y) {
        return allDots[x, y]; 
    }

    /// <summary>returns width</summary>
    public int getWidth() {
        return width; 
    }

    // <summary>returns height</summary>
    public int getHeight() {
        return height; 
    }
    
    /// <summary>returns the current Dot</summary>
    public Dot getCurrentDot() {
        return currentDot; 
    }
    /// <summary>returns all dots </summary>
    public GameObject[] getDots() {
        return dots; 
    }
    /// <summary>returns all eyes (special dots) </summary>
    public GameObject[] getEyes() {
        return eyes; 
    }

    /// <summary>sets the given Dot's <paramref name="x"/>, <paramref name="y"/></summary>
    /// <param name="x">the new x</param>
    /// <param name="y">the new y</param>
    /// <param name="dot">the given Dot</param>
    public void setDot(int x, int y, GameObject dot) {
        this.allDots[x, y] = dot; 
    }

    /// <summary>sets the CurrentDot to given Dot</summary>
    public void setCurrentDot(Dot dot) {
        currentDot = dot; 
    }

    /// <summary>sets the eyeRatio between 0 to 100 incl; returns false if value out of range.</summary>
    public bool setEyeRatio(int eye) {
        if (eye >= 0 && eye <= 100) {
            this.eyeRatio = eye; 
            return true; 
        }
        else {
            return false; 
        }
    }

    public Level getLvl() {
        if (lvl >= 0 && lvl < world.levels.Length) {
            return world.levels[lvl]; 
        }
        else {
            return null; 
        }
    }
}