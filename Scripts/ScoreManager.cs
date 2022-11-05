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
    [SerializeField]
    private Text scoreText; 
    private Board board; 
    [SerializeField]
    private Image scoreBar; 

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
    }

    // Update is called once per frame
    void Update() {
        scoreText.text = "Score: " + score; 
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

    /// <summary>get score </summary>
    public int getScore() {
        return score; 
    }

    /// <summary>clear score </summary>
    public void clearScore() {
        score = 0; 
    }
}
