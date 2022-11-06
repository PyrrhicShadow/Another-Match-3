using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

[System.Serializable]
public class BlankGoal {
    [SerializeField]
    private int numNeeded; 
    private int numberCollected; 
    [SerializeField]
    private Sprite goalSprite; 
    [SerializeField]
    private string matchTag; 

    public void addCollected(int amt) {
        numberCollected += amt; 
    }

    public int getNumberCollected() {
        return numberCollected; 
    }

    public string getTag() {
        return matchTag; 
    }

    public bool isNumNeeded() {
        return numNeeded == numberCollected; 
    }

    public Sprite getSprite() {
        return goalSprite; 
    }
}

public class ScoreManager : MonoBehaviour {
    [SerializeField]
    private int score; 
    private int lastScore; 
    [SerializeField]
    private Text scoreText; 
    private Board board; 
    [SerializeField]
    private Image scoreBar; 
    [SerializeField]
    private Image background; 
    private List<Color> bgColors; 

    [SerializeField]
    private BlankGoal[] levelGoals; 
    [SerializeField]
    private GameObject goalPrefab; 
    [SerializeField]
    private GameObject goalIntroParent; 
    [SerializeField]
    private GameObject goalGameParent; 

    // Start is called before the first frame update
    void Start() {
        board = FindObjectOfType<Board>(); 
        score = 0; 

        // big list of colors 
        bgColors = new List<Color>();  
        bgColors.Add(new Color(0.235493f, 0.2646766f, 0.3396226f, 1f)); // blue-black 
        bgColors.Add(new Color(0.2470588f, 0.2431373f, 0.3137255f, 1f)); // off-blue
        bgColors.Add(new Color(0.2588235f, 0.2196079f, 0.282353f, 1f)); // off-purple
        bgColors.Add(new Color(0.2705882f, 0.2f, 0.2588235f, 1f)); // purple
        bgColors.Add(new Color(0.3137255f, 0.1294118f, 0.1764706f, 1f)); // pale-red
        bgColors.Add(new Color(0.369f, 0f, 0.0115967f, 1f)); // blood-red

        background.color = bgColors[0]; 

    }

    // Update is called once per frame
    void Update() {
        if (score != lastScore) {
            scoreText.text = "Score: " + score; 
            ChangeBackgroundColor(); 
            lastScore = score; 
        }
    }

    /// <summary>Increases score by given amount <paramref name="amt"/></summary>
    public void IncreaseScore(int amt) {
        if (board != null && scoreBar != null) {
            score += amt; 
            scoreBar.fillAmount = (float)score / (float)(Board.balance * 5); 
        }
    }

    private void SetupIntroGoals() {
        for (int i = 0; i < levelGoals.Length; i++) {
            // create a new goal panel at the goalIntroParent position 
            GameObject goal = Instantiate(goalPrefab, goalIntroParent.transform); 
            goal.transform.SetParent(goalIntroParent.transform); 
        }
    }

    /// <summary>Checks for score and changes background color </summary>
    private void ChangeBackgroundColor() {
        if (score > Board.balance * 5) {
            background.color = bgColors[5]; 
            Debug.Log("Background increased to 5"); 
        }
        else if (score > Board.balance * 4) {
            background.color = bgColors[4]; 
            Debug.Log("Background increased to 4"); 
        }
        else if (score > Board.balance * 3) {
            background.color = bgColors[3]; 
            Debug.Log("Background increased to 3"); 
        }
        else if (score > Board.balance * 2) {
            background.color = bgColors[2]; 
            Debug.Log("Background increased to 2"); 
        }
        else if (score > Board.balance) {
            background.color = bgColors[1]; 
            Debug.Log("Background increased to 1"); 
        }
        else {
            background.color = bgColors[0]; 
            Debug.Log("Background is set at 0"); 
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
}
