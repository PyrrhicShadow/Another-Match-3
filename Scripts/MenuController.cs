using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class MenuController : MonoBehaviour {

    private Board board; 
    [SerializeField] string sceneToLoad; 

    [Header("Animators")]
    [SerializeField] protected Animator startAnim;
    [SerializeField] protected Animator endAnim; 
    [SerializeField] protected Animator winAnim;  

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
    public void toSplash() {
        SceneManager.LoadScene(sceneToLoad); 
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
        // if this is the last level, play winAnim
        if (board.getLvl() == (board.world.levels[board.world.levels.Length - 1]) && winAnim != null) {
            board.soundManager.backgroundMusicOn(false); 
            winAnim.SetBool("end", true); 
            board.soundManager.PlayJumpNoise(); 
        }
        // else, play normal end anim
        else if (endAnim != null) { 
            endAnim.SetBool("end", true); 
            board.soundManager.PlayWinNoise(); 
        }
    }

    public void loseGame() {
        if (endAnim != null) {
            endAnim.SetBool("end", true); 
            board.soundManager.PlayLoseNoise(); 
        }
    }

    public void retry() {
        if (endAnim != null) {
            endAnim.SetBool("end", false); 
        }

        StartCoroutine(GameStartCo()); 
    }
}
