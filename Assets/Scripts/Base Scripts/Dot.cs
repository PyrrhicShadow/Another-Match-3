using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour {

    [Header("Board Variables")]
    [SerializeField] protected int x;
    [SerializeField] private int y; 
    private int targetX; 
    private int targetY; 
    [SerializeField] private int prevX; 
    [SerializeField] private int prevY; 
    [SerializeField] private bool matched = false; 
    protected int points = 20; 
    [SerializeField] protected string type; 

    protected Board board; 
    protected FindMatches findMatches; 
    protected HintManager hintManager; 
    protected ScoreManager scoreManager; 
    protected GameObject otherDot; 
    protected Vector2 firstTouchPos; 
    protected Vector2 finalTouchPos;
    protected Vector2 tempPos; 
    protected SpriteRenderer mySprite; 

    [Header("Swipe Values")]
    [SerializeField] protected float swipeAngle = 0.0f; 
    [SerializeField] protected float swipeResist = 0.5f; 
    protected float moveSpeed = 0.4f; 

    [Header("Powerups")]
    [SerializeField] private bool colBomb; 
    [SerializeField] private bool rowBomb; 
    [SerializeField] private bool colorBomb; 
    [SerializeField] private bool adjBomb; 

    [SerializeField] protected GameObject rowArrow; 
    [SerializeField] protected GameObject colArrow; 
    [SerializeField] protected Sprite rainbowBomb; 
    [SerializeField] protected GameObject adjMarker; 

    // Start is called before the first frame update
    protected void Start() {
        board = GameObject.FindWithTag("board").GetComponent<Board>();
        findMatches = board.findMatches; 
        mySprite = GetComponent<SpriteRenderer>(); 
        hintManager = GameObject.FindWithTag("board").GetComponent<HintManager>(); 
        scoreManager = board.scoreManager; 

        colBomb = false; 
        rowBomb = false; 
        colorBomb = false; 
        adjBomb = false; 

    }

    // This is for testing and debug only; and a bit of a cheat code for players who find it.
    protected void OnMouseOver() {
        if (board.currentState == GameState.move){
            if (!this.colBomb && !this.rowBomb && !this.colorBomb && !this.adjBomb) {
                if (Input.GetKeyDown("up") || Input.GetKeyDown("down")) {
                    this.makeColBomb(); 
                    Debug.Log("Player created a column bomb"); 
                }
                else if (Input.GetKeyDown("right") || Input.GetKeyDown("left")) {
                    this.makeRowBomb(); 
                    Debug.Log("Player created a row bomb");
                }
                else if (Input.GetKeyDown("c")) {
                    this.makeColorBomb(); 
                    Debug.Log("Player created a color bomb");
                }
                else if (Input.GetKeyDown("a")) {
                    this.makeAdjBomb(); 
                    Debug.Log("Player created an adjacent bomb");
                }
            }
            else {
                if (Input.GetMouseButtonDown(1)) {
                    // unmake bombs 
                    this.unmakeBomb(); 
                }
            }
        }
    }

    /// <summary>Update is called once per frame</summary>
    protected void Update() {
        targetX = x; 
        targetY = y; 
        if (Mathf.Abs(targetX - transform.position.x) > .1) {
            // Move Towards the target
            tempPos = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPos, moveSpeed); 
            if (board.getDot(x, y) != this.gameObject) {
                board.setDot(x, y, this.gameObject); 
            }
            findMatches.FindAllMatches(); 
        }
        else {
            // Directly set the position
            tempPos = new Vector2(targetX, transform.position.y);
            transform.position = tempPos; 
        }
        if (Mathf.Abs(targetY - transform.position.y) > .1) {
            // Move Towards the target
            tempPos = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPos, moveSpeed);  
            if (board.getDot(x, y) != this.gameObject) {
                board.setDot(x, y, this.gameObject); 
            }
            findMatches.FindAllMatches(); 
        }
        else {
            // Directly set the position
            tempPos = new Vector2(transform.position.x, targetY);
            transform.position = tempPos; 
        }
    }

    /// <summary>checks to see if moving this dot creates a match </summary>
    public IEnumerator CheckMoveCo() {
        yield return new WaitForSeconds(0.5f); 

        if (colorBomb) {
            // This dot is a color bomb, and the other dot is the color to destroy
            findMatches.MatchColors(otherDot.tag); 
            this.setMatched(true); 
        }
        else if (otherDot.GetComponent<Dot>().isColorBomb()) {
            // The other dot is a color bomb, and this dot is the color to destroy
            findMatches.MatchColors(this.gameObject.tag); 
            otherDot.GetComponent<Dot>().setMatched(true); 
        }
        
        if (otherDot != null) {
            if (!matched && !otherDot.GetComponent<Dot>().isMatched()) {
                otherDot.GetComponent<Dot>().setX(x);
                otherDot.GetComponent<Dot>().setY(y); 
                x = prevX; 
                y = prevY; 
                yield return new WaitForSeconds(0.5f); 
                board.setCurrentDot(null); 
                board.currentState = GameState.move; 
            } 
            else { 
                if (scoreManager != null) {
                    if (scoreManager.getGameType() == GameType.moves) {
                        scoreManager.DecreaseCounter(); 
                    } 
                    else if (scoreManager.getGameType() == GameType.time) {

                    }
                }
                board.DestroyMatches(); 
            }
        }
    }

    /// <summary>sets the mouse-down position</summary>
    protected void OnMouseDown() {
        // destroy the hint 
        if (hintManager != null) {
            hintManager.DestroyHint(); 
        }

        if (board.currentState == GameState.move) {
            firstTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
        }
    }

    /// <summary>sets the mouse-up position</summary>
    protected void OnMouseUp() {
        if (board.currentState == GameState.move) {
            finalTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
            CalculateAngle(); 
        }
    }

    /// <summary>calculates the angle of the mouse-down to mouse-up swipe in radians</summary>
    private void CalculateAngle() {
        if (Mathf.Abs(finalTouchPos.y - firstTouchPos.y) > swipeResist || Mathf.Abs(finalTouchPos.x - firstTouchPos.x) > swipeResist) {
            swipeAngle = Mathf.Atan2(finalTouchPos.y - firstTouchPos.y, finalTouchPos.x - firstTouchPos.x); 
            //Debug.Log((swipeAngle * Mathf.PI) + "π rad"); 
            board.currentState = GameState.wait; 
            board.setCurrentDot(this); 
            MoveDots(); 
        }
        else {
            board.currentState = GameState.move; 
        }
    }

    /// <summary>move right, up, left, or down based on the angle of the swipe</summary>
    private void MoveDots() {
        if (swipeAngle > -(Mathf.PI/4) && swipeAngle <= (Mathf.PI/4) && x < (board.getWidth() - 1)) {
            // right swipe
            //Debug.Log("Rigth swipe"); 
            this.SwapDots(x + 1, y); 
        }
        else if (swipeAngle > (Mathf.PI/4) && swipeAngle <= (3*Mathf.PI/4) && y < (board.getHeight() - 1)) {
            // up swipe 
            //Debug.Log("Up swipe"); 
            this.SwapDots(x, y + 1); 
        }
        else if((swipeAngle > (3*Mathf.PI/4) || swipeAngle <= -(3*Mathf.PI/4)) && x > 0) {
            // left swipe 
            //Debug.Log("Left swipe"); 
            this.SwapDots(x - 1, y); 
        }
        else if (swipeAngle < -(Mathf.PI/4) && swipeAngle >= -(3*Mathf.PI/4) && y > 0) {
            // down swipe 
            //Debug.Log("Down swipe"); 
            this.SwapDots(x, y - 1); 
        }
        else {
            // invalid move
            Debug.Log("Invalid move"); 
            board.currentState = GameState.move; 
        }
    }

    /// <summary>swaps this dot with a left, right, up, or down neighbor dot</summary>
    private void SwapDots(int x, int y) {
        otherDot = board.getDot(x, y); 
        if (otherDot != null) {
            otherDot.GetComponent<Dot>().setX(this.x); 
            otherDot.GetComponent<Dot>().setY(this.y); 
        }
        else {
            board.currentState = GameState.move; 
        }
        updatePrevXY(); 
        this.x = x;
        this.y = y;  
        StartCoroutine(CheckMoveCo());
    }

    /// <summary>turns the current dot into a column bomb</summary>
    public void makeColBomb() {
        colBomb = true; 
        GameObject arrow = Instantiate(colArrow, transform.position, Quaternion.identity); 
        arrow.transform.parent = this.transform; 
    }

    /// <summary>turns the current dot into a row bomb</summary>
    public void makeRowBomb() {
        rowBomb = true; 
        GameObject arrow = Instantiate(rowArrow, transform.position, Quaternion.identity); 
        arrow.transform.parent = this.transform; 
    }

    /// <summary>turns the current dot into a rainbow bomb</summary>
    public void makeColorBomb() {
        colorBomb = true; 
        this.gameObject.tag = "rainbow"; 
        mySprite.sprite = rainbowBomb; 
        mySprite.color = new Color(1f, 1f, 1f, 1f); 
    }

    // <summary>turns the current dot into an adjacent bomb</summary>
    public void makeAdjBomb() {
        adjBomb = true; 
        GameObject marker = Instantiate(adjMarker, transform.position, Quaternion.identity); 
        marker.transform.parent = this.transform;
    }

    /// <summary>turns the current dot back into a (random) normal dot</summary>
    public void unmakeBomb() {
        if (colBomb) {
            unmakeColBomb(); 
            Debug.Log("Player returned a column bomb into a " + this.tag + " dot");
        }
        else if (rowBomb) {
            unmakeRowBomb(); 
            Debug.Log("Player returned a row bomb into a " + this.tag + " dot");
        }
        else if (adjBomb) {
            unmakeAdjBomb(); 
            Debug.Log("Player returned an adjacent bomb into a " + this.tag + " dot");
        }
        else if (colorBomb) {
            unmakeColorBomb(); 
            Debug.Log("Player returned a color bomb into a " + this.tag + " dot");
        }
        else {
            Debug.Log("No bomb detected.");
        }
    }

    protected void unmakeColBomb() {
        colBomb = false; 
        GameObject arrow = this.transform.GetChild(0).gameObject;
        Destroy(arrow);  
    }

    protected void unmakeRowBomb() {
        rowBomb = false; 
        GameObject arrow = this.transform.GetChild(0).gameObject;
        Destroy(arrow);  
    }

    protected void unmakeAdjBomb() {
        adjBomb = false; 
        GameObject marker = this.transform.GetChild(0).gameObject;
        Destroy(marker); 
    }

    protected void unmakeColorBomb() {
        colorBomb = false; 
        GameObject[] dots = board.getDots(); 
        GameObject dotToUse = Instantiate(dots[Random.Range(0, dots.Length)], transform.position, Quaternion.identity);
        this.gameObject.tag = dotToUse.tag; 
        mySprite.sprite = dotToUse.GetComponent<SpriteRenderer>().sprite; 
        Destroy(dotToUse);
    }

    /************** Public Getters and Setters **************/

    /// <summary>gets current x-value</summary>
    public int getX() {
        return x; 
    }

    /// <summary>gets current y-value</summary>
    public int getY() {
        return y; 
    }

    /// <summary>gets target x-value</summary>
    public int getTargetX() {
        return targetX; 
    }

    /// <summary>gets target y-value</summary>
    public int getTargetY() {
        return targetY; 
    } 

    /// <summary>gets the other dot</summary>
    public GameObject getOtherDot() {
        return otherDot; 
    }

    /// <summary>gets this Dot's Sprite</summary>
    public Sprite getSprite() {
        return mySprite.sprite; 
    }

    public string getType() {
        return type; 
    }

    /// <summary>returns true if is matched, otherwise false</summary>
    public bool isMatched() {
        return matched; 
    }

    /// <summary>returns true if is ColBomb, otherwise false </summary>
    public bool isColBomb() {
        return colBomb; 
    }

    /// <summary>returns true if this Dot is a rowBomb, otherwise false</summary>
    public bool isRowBomb() {
        return rowBomb; 
    }

    /// <summary>returns true if is colorBomb, otherwise false </summary>
    public bool isColorBomb() {
        return colorBomb; 
    }

    /// <summary>returns true if is adjBomb, otherwise false </summary>
    public bool isAdjBomb() {
        return adjBomb; 
    }

    /// <summary>returns this dot's point value</summary>
    public int getPoints() {
        return points; 
    }

    /// <summary>set x-value to new x-value</summary>
    public void setX(int newX) {
        this.x = newX; 
    }

    /// <summary>set y-value to new y-value</summary>
    public void setY(int newY) {
        this.y = newY; 
    }

    /// <summary>set previous x- and y-value to current x- and y-value </summary>
    public void updatePrevXY() {
        prevX = x; 
        prevY = y; 
    }

    /// <summary>set matched to new bool </summary>
    public void setMatched(bool matched) {
        this.matched = matched; 
    }

    /// <summary>set the point value for this dot</summary> 
    public void setPoints(int amt) {
        points = amt; 
    }

    public void setRowBomb(bool val) {
        rowBomb = val; 
    }
    
    public void setColBomb(bool val) {
        colBomb = val; 
    }

    public void setAdjBomb(bool val) {
        adjBomb = val;
    }
    public void setColorBomb(bool val) {
        colorBomb = val; 
    }
}
