using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour {
    
    [SerializeField] private Animator startAnim;
    [SerializeField] private Animator winAnim; 
    [SerializeField] private Animator endAnim; 
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
    /** 
     * Game manager handles level selection 
     * If board.lvl == gameManager.lvls.Length 
     * then use end win screen 
     * else use win screen
     */ 
    public void winGame() {
        if (winAnim != null && endAnim != null) {
            int rand = Random.Range(0, 100); 
            winAnim.SetBool("end", false); 
            if (rand < 5) {
                winAnim.SetBool("end", true); 
            }
            else {
                endAnim.SetBool("lose", true); 
            }
        }
    }

    public void loseGame() {
        if (endAnim != null) {
            endAnim.SetBool("lose", true); 
        }
    }

    public void retry() {
        if (endAnim != null) {
            endAnim.SetBool("lose", false); 
        }

        StartCoroutine(GameStartCo()); 
    }
}
