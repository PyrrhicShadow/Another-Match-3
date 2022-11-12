using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class BackgroundManager : MonoBehaviour
{
    /*****    Deprecated class     *****/
    private Board board; 
    private ScoreManager scoreManager; 
    private List<Color> bgColors; 
    private Image background; 
    private int lastScore; 

    // Start is called before the first frame update
    void Start()
    {
        // grab peer objects 
        board = FindObjectOfType<Board>(); 
        scoreManager = FindObjectOfType<ScoreManager>(); 

        // big list of colors 
        bgColors = new List<Color>();  
        bgColors.Add(new Color(0.235493f, 0.2646766f, 0.3396226f, 1f)); // blue-black 
        bgColors.Add(new Color(0.2470588f, 0.2431373f, 0.3137255f, 1f)); // off-blue
        bgColors.Add(new Color(0.2588235f, 0.2196079f, 0.282353f, 1f)); // off-purple
        bgColors.Add(new Color(0.2705882f, 0.2f, 0.2588235f, 1f)); // purple
        bgColors.Add(new Color(0.3137255f, 0.1294118f, 0.1764706f, 1f)); // pale-red
        bgColors.Add(new Color(0.369f, 0f, 0.0115967f, 1f)); // blood-red

        // grab image component
        background = GetComponent<Image>(); 
        background.color = bgColors[0]; 
    }

    // Update is called once per frame
    void Update()
    {
        if (scoreManager.getScore() != lastScore) {
            ChangeBackgroundColor();
            scoreManager.getScore(); 
            lastScore = scoreManager.getScore(); 
        }
    }

    /// <summary>Checks for score and changes background color </summary>
    private void ChangeBackgroundColor() {
        int currentScore = scoreManager.getScore(); 
        if (currentScore > board.balance * 5) {
            background.color = bgColors[5]; 
            Debug.Log("Background increased to 5"); 
        }
        else if (currentScore > board.balance * 4) {
            background.color = bgColors[4]; 
            Debug.Log("Background increased to 4"); 
        }
        else if (currentScore > board.balance * 3) {
            background.color = bgColors[3]; 
            Debug.Log("Background increased to 3"); 
        }
        else if (currentScore > board.balance * 2) {
            background.color = bgColors[2]; 
            Debug.Log("Background increased to 2"); 
        }
        else if (currentScore > board.balance) {
            background.color = bgColors[1]; 
            Debug.Log("Background increased to 1"); 
        }
        else {
            background.color = bgColors[0]; 
            Debug.Log("Background is set at 0"); 
        }
    }
}
