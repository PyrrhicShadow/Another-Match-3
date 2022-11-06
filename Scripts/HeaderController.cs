using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class HeaderController : MonoBehaviour { 

    /*     Deprecated class     */

    private Board board; 
    private GameObject moveIndicator; 
    private Image moveIndicatorImage;  
    private List<Color> moveColors; 

    // Start is called before the first frame update
    void Start()
    {
        // Find peer objects
        board = FindObjectOfType<Board>(); 

        // move colors 

        moveColors.Add(new Color(0f, 0.872f, 0.1591406f, 1f)); // green
        moveColors.Add(new Color(1f, 0.6806262f, 0f, 1f)); // orange
        moveColors.Add(new Color(0.6901961f, 0.8392158f, 0.882353f, 1f)); // blue

        // find the move indicator, a child object
        moveIndicator = transform.GetChild(0).gameObject; 
        moveIndicatorImage = moveIndicator.GetComponent<Image>(); 

    }

    // Update is called once per frame
    void Update()
    {
        if (board.currentState == GameState.move) {
            moveIndicatorImage.color = moveColors[0]; 
        }
        else if(board.currentState == GameState.wait) {
            moveIndicatorImage.color = moveColors[1]; 
        }
        else {
            moveIndicatorImage.color = moveColors[3]; 
        }
    }

}
