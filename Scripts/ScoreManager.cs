using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    private int score; 
    private Text scoreText; 
    private Board board; 
    private Image scoreBar; 

    // Start is called before the first frame update
    void Start()
    {
        scoreText = this.transform.Find("Score text").GetComponent<Text>(); 
        board = FindObjectOfType<Board>(); 
        scoreBar = this.transform.Find("Score bar").GetComponent<Image>(); 
        score = 0; 
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + score; 
    }

    /// <summary>Increases score by given amount <paramref name="amt"/></summary>
    public void IncreaseScore(int amt) {
        if (board != null && scoreBar != null) {
            score += amt; 
            scoreBar.fillAmount = (float)score / (float)(Board.balance * 5); 
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
