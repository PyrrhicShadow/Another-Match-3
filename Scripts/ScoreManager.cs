using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    private int score; 
    private Text scoreText; 

    // Start is called before the first frame update
    void Start()
    {
        scoreText = this.GetComponent<Text>(); 
        score = 0; 
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + score; 
    }

    /// <summary>Increases score by given amount <paramref name="amt"/></summary>
    public void IncreaseScore(int amt) {
        score += amt; 
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
