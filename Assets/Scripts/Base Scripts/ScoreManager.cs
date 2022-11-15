using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

[System.Serializable]
public class BlankGoal {
    [SerializeField] private int numNeeded; 
    private int numCollected;  
    [SerializeField] private Sprite goalSprite; 
    [SerializeField] private Color goalSpriteColor = Color.white; 
    [SerializeField] private string matchTag; 

    public void addCollected(int amt) {
        numCollected += amt; 
    }

    public void clearCollected() {
        numCollected = 0; 
    }

    public int getNumCollected() {
        return numCollected; 
    }

    public int getNumNeeded() {
        return numNeeded; 
    }

    public string getTag() {
        return matchTag; 
    }

    public bool isComplete() {
        return numCollected >= numNeeded; 
    }

    public Sprite getSprite() {
        return goalSprite; 
    }

    public Color getSpriteColor() {
        return goalSpriteColor;
    }
}

public enum GameType {
    moves, time
}

[System.Serializable]
public class EndGameReqs {
    [SerializeField] private GameType gameType; 
    [SerializeField] private int counter; 

    public int getCounter() {
        return counter; 
    }

    public void setCouter(int amt) {
        counter = amt; 
    }

    public GameType getGameType() {
        return gameType; 
    }
}
public class ScoreManager : MonoBehaviour {
    
    private Board board; 
    private MenuController menuController;

    [Header("Score Manager")]
    [SerializeField] int score; 
    private int lastScore; 
    [SerializeField] int scoreGoal; 
    [SerializeField] Text scoreText;  
    [SerializeField] Image scoreBar; 

    [Header ("Background Manager")]
    [SerializeField]
    private Image background; 
    private List<Color> bgColors; 
    private int bgTier; 

    [Header("Goal Manager")]
    [SerializeField] int level; 
    [SerializeField] Text levelText; 
    [SerializeField] BlankGoal[] levelGoals; 
    private List<GoalPanel> currentGoals; 
    [SerializeField] GameObject goalPrefab; 
    [SerializeField] GameObject goalStartParent; 
    [SerializeField] GameObject goalGameParent; 

    [Header("Endgame Manager")]
    [SerializeField] EndGameReqs reqs; 
    [SerializeField] GameObject reqMoveText;
    [SerializeField] GameObject reqTimeText;  
    [SerializeField] GameObject winPanel; 
    [SerializeField] Text winScore; 
    [SerializeField] Text winEndScore; 
    [SerializeField] GameObject[] winStars; 
    [SerializeField] GameObject losePanel; 
    [SerializeField] Text loseScore; 
    [SerializeField] Image reqBar; 
    [SerializeField] Text counterText; 
    private int counter; 
    private float timer; 

    // Start is called before the first frame update
    void Start() {
        board = GameObject.FindWithTag("board").GetComponent<Board>(); 
        menuController = board.gameObject.GetComponent<MenuController>(); 
        level = PlayerPrefs.GetInt("CurrentLevel", 0) + 1;  
        currentGoals = new List<GoalPanel>(); 
        score = 0; 

        // big list of colors 
        bgColors = new List<Color>();  
        bgColors.Add(new Color(0.235493f, 0.2646766f, 0.3396226f, 1f)); // blue-black 
        bgColors.Add(new Color(0.2470588f, 0.2431373f, 0.3137255f, 1f)); // off-blue
        bgColors.Add(new Color(0.2588235f, 0.2196079f, 0.282353f, 1f)); // off-purple
        bgColors.Add(new Color(0.2705882f, 0.2f, 0.2588235f, 1f)); // purple
        bgColors.Add(new Color(0.3137255f, 0.1294118f, 0.1764706f, 1f)); // pale-red
        bgColors.Add(new Color(0.369f, 0f, 0.0115967f, 1f)); // blood-red

        // background.color = bgColors[0]; 
        // bgTier = 0; 

        Level myLvl = board.getLvl(); 
        if (myLvl != null) {
            // score
            scoreGoal = myLvl.scoreGoal; 
            background.color = myLvl.backgroundColor; 
            // goals 
            levelGoals = myLvl.levelGoals; 
            this.clearLevelGoals();
            // endgame 
            reqs = myLvl.reqs; 
        }

        SetUpGoals(); 
        SetUpReqs(); 
    }

    // Update is called once per frame
    void Update() {
        if (reqs.getGameType() == GameType.time && counter > 0) {
            timer -= Time.deltaTime; 
            if (timer <= 0) {
                DecreaseCounter(); 
                timer = 1; 
            }
        }
    }

    /// <summary>Increases score by given amount <paramref name="amt"/></summary>
    public void IncreaseScore(int amt) {
        if (board != null && scoreBar != null) {
            score += amt; 
            scoreBar.fillAmount = (float)score / (float)(board.balance * scoreGoal); 
            UpdateScore(); 
            // ChangeBackgroundColor(); 
        }
    }

    private void SetUpGoals() {
        levelText.text = "Level: " + level; 

        for (int i = 0; i < levelGoals.Length; i++) {
            // create a new goal panel at the goalIntroParent position 
            GameObject startGoal = Instantiate(goalPrefab, goalStartParent.transform); 
            startGoal.transform.SetParent(goalStartParent.transform); 
            
            // create a new goal panel at the goalGameParent position
            GameObject gameGoal = Instantiate(goalPrefab, goalGameParent.transform); 
            gameGoal.transform.SetParent(goalGameParent.transform); 
            
            // set the image and text of the goal 
            GoalPanel startPanel = startGoal.GetComponent<GoalPanel>(); 
            startPanel.setSprite(levelGoals[i].getSprite(), levelGoals[i].getSpriteColor()); 
            startPanel.setText(levelGoals[i].getNumNeeded().ToString()); 

            GoalPanel gamePanel = gameGoal.GetComponent<GoalPanel>(); 
            gamePanel.setSprite(levelGoals[i].getSprite(), levelGoals[i].getSpriteColor()); 
            gamePanel.setText("0/" + levelGoals[i].getNumNeeded()); 

            // add this goal to list of current goals
            currentGoals.Add(gamePanel); 
        }
    }

    public void UpdateGoals() {
        int goalsCompleted = 0;
        for (int i = 0; i < levelGoals.Length; i++) {
            currentGoals[i].setText(levelGoals[i].getNumCollected().ToString() + "/" + levelGoals[i].getNumNeeded()); 
            if (levelGoals[i].isComplete()) {
                goalsCompleted++; 
                currentGoals[i].setText(levelGoals[i].getNumNeeded().ToString() + "/" + levelGoals[i].getNumNeeded()); 
            }
        }

        if (goalsCompleted >= levelGoals.Length) {
            if (menuController != null && score >= scoreGoal * board.balance) {
                Debug.Log("You win!"); 
                WinGame(); 
            }
        }
    }

    public void compareGoal(string goal) {
        for (int i = 0; i < levelGoals.Length; i++) {
            if (goal == levelGoals[i].getTag()) {
                levelGoals[i].addCollected(1); 
            }
        }
    }

    private void SetUpReqs() {
        counter = reqs.getCounter(); 
        if (reqs.getGameType() == GameType.moves) {
            reqMoveText.SetActive(true); 
            reqTimeText.SetActive(false); 
        }
        else if (reqs.getGameType() == GameType.time) {
            reqMoveText.SetActive(false); 
            reqTimeText.SetActive(true); 
            timer = 1; 
        }
        else {

        }
        counterText.text = counter.ToString(); 
        winPanel.SetActive(false); 
        losePanel.SetActive(false); 
    }

    public void DecreaseCounter() {

        if (board.currentState != GameState.pause){
            counter--;
            counterText.text = counter.ToString();
        }

        if (reqBar != null) {
            reqBar.fillAmount = (float)counter / (float)reqs.getCounter(); 
        }

        if (counter <= 0) {
            Debug.Log("You lose :("); 
            LoseGame(); 
        }
    }

    public void WinGame() {
        board.currentState = GameState.win; 
        losePanel.SetActive(false);
        winPanel.SetActive(true); 
        winScore.text = score.ToString();
        winEndScore.text = score.ToString(); 
        menuController.winGame(); 
    }

    public void LoseGame() {
        board.currentState = GameState.lose; 
        losePanel.SetActive(true);
        winPanel.SetActive(false); 
        counter = 0; 
        counterText.text = counter.ToString(); 
        loseScore.text = score.ToString(); 
        menuController.loseGame(); 
    }

    public GameType getGameType() {
        return reqs.getGameType(); 
    }

    private void UpdateScore() {
        scoreText.text = "Score: " + score; 
    }

    /// <summary>Checks for score and changes background color </summary>
    private void ChangeBackgroundColor() {
        if (score > board.balance * scoreGoal && bgTier < 5) {
            ChangeBackgroundColor(5); 
        }
        else if (score > board.balance * scoreGoal / .8 && bgTier < 4) {
            ChangeBackgroundColor(4); 
        }
        else if (score > board.balance * scoreGoal / .6 && bgTier < 3) {
            ChangeBackgroundColor(3); 
        }
        else if (score > board.balance * scoreGoal / .4 && bgTier < 2) {
            ChangeBackgroundColor(2); 
        }
        else if (score > board.balance * scoreGoal / .2 && bgTier < 1) {
            ChangeBackgroundColor(1); 
        }
    }

    /// <summary>Changes the background color to match the current tier</summary>
    private void ChangeBackgroundColor(int tier) {
        if (tier > 0 && tier < (bgColors.Count) && bgTier != tier) {
            background.color = bgColors[tier]; 
            bgTier = tier; 
            Debug.Log("Background tier increased to " + tier); 
        }
    }

    /// <summary>get score </summary>
    public int getScore() {
        return score; 
    }

    /// <summary>clear score </summary>
    public void clearScore() {
        score = 0; 
    }

    public void clearLevelGoals() {
        foreach (BlankGoal goal in levelGoals) {
            goal.clearCollected();
        }
    }
}
