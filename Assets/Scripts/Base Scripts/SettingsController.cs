using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings; 

public class SettingsController : MonoBehaviour {

    private Board board; 
    private SoundManager soundManager; 
    [SerializeField] private GameObject pauseMenu; 
    [SerializeField] private bool paused = false; 

    private void Start() {
        board = this.gameObject.GetComponent<Board>(); 
        soundManager = this.gameObject.GetComponent<SoundManager>(); 
        pauseMenu.SetActive(false); 

        StartCoroutine(setLanguageCo()); 
    }
    public void LanguageDropDown(int value) {
        PlayerPrefs.SetInt("Locale", value);
        ChangeLanguage(); 
    }

    public IEnumerator setLanguageCo() {
        // Wait for the localization system to initialize, loading Locales, preloading, etc.
        yield return LocalizationSettings.InitializationOperation;

        // This variable selects the language. For example, if in the table your first language is English then 0 = English. 
        int i = PlayerPrefs.GetInt("Locale", 0); 

        // This part changes the language
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[i];
    }

    private void ChangeLanguage() {
        // This variable selects the language. For example, if in the table your first language is English then 0 = English. 
        int i = PlayerPrefs.GetInt("Locale", 0); 

        // This part changes the language
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[i];
    }

    
    public void PauseGame() {
        paused = !paused; 
    }

    private void Update() {
        if (paused && !pauseMenu.activeInHierarchy) {
            pauseMenu.SetActive(true); 
            Debug.Log("Make pause menu active"); 
            if (board != null) {
                board.currentState = GameState.pause; 
            }
        }
        if (!paused && pauseMenu.activeInHierarchy) {
            pauseMenu.SetActive(false); 
            if (board != null) {
                board.currentState = GameState.move; 
            }
        }
    }

    public void Sounds() {
        if (soundManager != null) {
            if (soundManager.isBackgroundMusicOn()) {
                soundManager.setBackgroundMusic(false); 
            }
            else {
                soundManager.setBackgroundMusic(true); 
            }
        }
        else {

        }
        // save volume PlayerPrefs.SetFloat("MusicVol", 0f); PlayerPrefs.SetFloat("EffectsVol", 0f); 
    }
}
