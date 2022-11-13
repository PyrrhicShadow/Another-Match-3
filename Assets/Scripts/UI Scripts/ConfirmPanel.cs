using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement; 

public class ConfirmPanel : MonoBehaviour {

    [Header("Level Info")]
    [SerializeField] string sceneToLoad; 
    private GameData gameData; 
    [SerializeField] int lvl; 
    private int stars; 
    private int highScore; 

    [Header("UI stuff")]
    [SerializeField] Image[] starImages; 
    [SerializeField] Text highScoreText; 

    // Start is called before the first frame update
    private void Start() {
        gameData = FindObjectOfType<GameData>(); 
        LoadData(); 
        Cancel(); 
    }

    void OnEnable() {
        gameData = FindObjectOfType<GameData>(); 
        LoadData(); 
        UpdateHighScore(); 
        ActivateStars(); 
    }

    private void ActivateStars() {
        for (int i = 0; i < starImages.Length; i++) {
            starImages[i].enabled = false; 
        }
        for (int i = 0; i < stars; i++) {
            starImages[i].enabled = true; 
        }
    }

    private void UpdateHighScore() {
        highScoreText.text = highScore.ToString();
    }

    private void LoadData() {
        if (gameData != null) {
            if (lvl <= gameData.saveData.Count()){
                Debug.Log("Level: " + lvl); 
                SaveData thisLevel = gameData.saveData; 
                stars = thisLevel.getStar(lvl - 1); 
                highScore = thisLevel.getHighScore(lvl - 1); 
            }
            else {
                Debug.Log("Level not found");
                stars = 0; 
                highScore = 0; 
            }
        }
        else {
            Debug.Log("GameData not found"); 
        }
    }

    // Update is called once per frame
    void Update() {

    }

    public void Cancel() {
        this.gameObject.SetActive(false); 
    }

    // public void OnEnable() {
    //     LoadData(); 
    //     UpdateHighScore(); 
    //     ActivateStars(); 
    // }

    public void Play() {
        PlayerPrefs.SetInt("CurrentLevel", lvl - 1); 
        SceneManager.LoadScene(sceneToLoad); 
    }

    public void setLvl(int lvl) {
        this.lvl = lvl; 
    }
}
