using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour {
    
    [SerializeField]
    private Animator startAnim;
    [SerializeField]
    private Animator endAnim; 
    private Board board;  

    void Start() {
        board = FindObjectOfType<Board>(); 
    }

    /// <summary>button to start the current game</summar>
    public void buttonOK() {
        if (startAnim != null) {
            startAnim.SetBool("show", true); 
            board.currentState = GameState.move; 
        }
    }

    /// <summary>called to generate new game menus</summar>
    public void newGame() {
        if (endAnim != null) {
            endAnim.SetBool("end", false); 
            startAnim.SetBool("show", false); 
        }
    }

    /// <summary>called when game ends</summary>
    public void endGame() {
        if (endAnim != null) {
            endAnim.SetBool("end", true); 
            board.currentState = GameState.wait; 
        }
    }
}
