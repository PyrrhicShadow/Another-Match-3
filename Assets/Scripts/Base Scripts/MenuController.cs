using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement; 

public class MenuController : MonoBehaviour {

    private Board board; 
    private GameData gameData; 
    [SerializeField] string sceneToLoad; 
    private int lvl; 

    [Header("Animators")]
    [SerializeField] private GameObject menu; 
    [SerializeField] protected Animator startAnim;
    [SerializeField] protected Animator endAnim; 
    [SerializeField] protected Animator winAnim; 

    [Header("Pause Manager")]
    [SerializeField] private GameObject pauseMenu; 
    [SerializeField] private bool paused = false; 

    protected void Start() {
        board = GameObject.FindWithTag("board").GetComponent<Board>(); 
        gameData = FindObjectOfType<GameData>(); 
        pauseMenu.SetActive(false); 
        lvl = PlayerPrefs.GetInt("CurrentLevel", 0); 
        if (menu != null) {
            menu.SetActive(true); 
        }
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
    public void toSplashWin() {
        if (gameData != null) {
            SaveData thisLevel = gameData.saveData; 
            thisLevel.setActive(lvl + 1, true); 
            if (board.scoreManager.getScore() > thisLevel.getHighScore(lvl)) {
                thisLevel.setHighScore(lvl, board.scoreManager.getScore()); 
            }

            gameData.Save(); 
        }
        SceneManager.LoadScene(sceneToLoad); 
    }

    public void toSplashLose() {
        if (gameData != null) {
            SaveData thisLevel = gameData.saveData; 
            if (board.scoreManager.getScore() > thisLevel.getHighScore(lvl)) {
                thisLevel.setHighScore(lvl, board.scoreManager.getScore()); 
            }
        }
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
            board.soundManager.setBackgroundMusic(false); 
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

        if (gameData != null) {
            SaveData thisLevel = gameData.saveData; 
            if (board.scoreManager.getScore() > thisLevel.getHighScore(lvl)) {
                thisLevel.setHighScore(lvl, board.scoreManager.getScore()); 
            }
        }
        
        board.scoreManager.clearScore(); 
        board.scoreManager.clearLevelGoals(); 

        StartCoroutine(GameStartCo()); 
    }

    /*****     PauseManager     *****/

    public void PauseGame() {
        paused = !paused; 
    }

    private void Update() {
        if (paused && !pauseMenu.activeInHierarchy) {
            pauseMenu.SetActive(true); 
            board.currentState = GameState.pause; 
        }
        if (!paused && pauseMenu.activeInHierarchy) {
            pauseMenu.SetActive(false); 
            board.currentState = GameState.move; 
        }
    }

    public void Sounds() {
        if (board.soundManager.isBackgroundMusicOn()) {
            board.soundManager.setBackgroundMusic(false); 
        }
        else {
            board.soundManager.setBackgroundMusic(true); 
        }
    }
}
