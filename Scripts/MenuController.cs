using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour {
    
    [SerializeField] private Animator startAnim;
    [SerializeField] private Animator winAnim; 
    [SerializeField] private Animator loseAnim; 
    private Board board;  

    void Start() {
        board = FindObjectOfType<Board>(); 
    }

    /// <summary>button to start the current game</summar>
    public void buttonOK() {
        if (startAnim != null) {
            startAnim.SetBool("show", true); 
        }

        StartCoroutine(GameStartCo()); 
    }

    private IEnumerator GameStartCo() {
        yield return new WaitForSeconds(1f); 
        board.currentState = GameState.move; 
    }

    /// <summary>called to generate new game menus</summar>
    public void newGame() {
        if (winAnim != null) {
            winAnim.SetBool("end", false); 
            startAnim.SetBool("show", false); 
        }
    }

    /// <summary>called when game ends</summary>
    public void winGame() {
        if (winAnim != null) {
            winAnim.SetBool("end", false); 
            winAnim.SetBool("end", true); 
        }
    }

    public void loseGame() {
        if (loseAnim != null) {
            loseAnim.SetBool("lose", true); 
        }
    }

    public void retry() {
        if (loseAnim != null) {
            loseAnim.SetBool("lose", false); 
        }

        StartCoroutine(GameStartCo()); 
    }
}
