using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour {
    
    [SerializeField] protected Animator startAnim;
    [SerializeField] protected Animator endAnim; 
    [SerializeField] protected Animator winAnim; 
    private Board board;  

    protected void Start() {
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
        if (endAnim != null && startAnim != null) {
            endAnim.SetBool("end", false); 
            startAnim.SetBool("show", false); 
        }
    }

    /// <summary>called when game ends</summary>
    /** 
     * Game manager handles level selection 
     * If board.lvl == gameManager.lvls.Length - 1
     * then this is the last level, use end win screen 
     * else this is a normal level, use win screen
     */ 
    public void winGame() {
        StartCoroutine(GameEndCo()); 
    }

    private IEnumerator GameEndCo() {
        yield return new WaitForSeconds(1f); 
        if (board.getLvl() == (board.world.levels.Length - 1) && winAnim != null) {
            winAnim.SetBool("end", true); 
        }
        else if (endAnim != null) { 
            endAnim.SetBool("end", true); 
        }
    }

    public void loseGame() {
        if (endAnim != null) {
            endAnim.SetBool("end", true); 
        }
    }

    public void retry() {
        if (endAnim != null) {
            endAnim.SetBool("end", false); 
        }

        StartCoroutine(GameStartCo()); 
    }
}
