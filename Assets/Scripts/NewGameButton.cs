using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; 

public class NewGameButton : MonoBehaviour, IPointerClickHandler {

    /*     Deprecated class     */

    private Board board; 
    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData pointerEventData) {
        // if user clicks using the left-mouse button
        if (pointerEventData.button == PointerEventData.InputButton.Left) {
            // new game
        }
        else if (pointerEventData.button == PointerEventData.InputButton.Right) {
            board.ShuffleBoard();
        }
    }
}
