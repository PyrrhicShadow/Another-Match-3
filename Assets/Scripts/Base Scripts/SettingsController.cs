using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.Localization.Settings; 
using UnityEngine.Localization; 
using UnityEngine.Localization.Tables; 

public class SettingsController : MonoBehaviour {

    private Board board; 
    private SoundManager soundManager; 
    [SerializeField] private GameObject pauseMenu; 
    [SerializeField] private Dropdown dropdown;
    [SerializeField] private bool paused = false; 

    [Header("Localization")] 
    [SerializeField] private LocalizedStringTable _localizedStringTable;
    private StringTable _currentStringTable;

    private IEnumerator Start() {
        board = this.gameObject.GetComponent<Board>(); 
        soundManager = this.gameObject.GetComponent<SoundManager>(); 
        pauseMenu.SetActive(false); 

        // Wait for the localization system to initialize, loading Locales, preloading, etc.
        yield return LocalizationSettings.InitializationOperation;

        var tableLoading = _localizedStringTable.GetTable(); 

        yield return tableLoading; 

       _currentStringTable = tableLoading;

        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>(); 

        int selected = PlayerPrefs.GetInt("Locale", 0); 
        for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; i++) {
            var locale = LocalizationSettings.AvailableLocales.Locales[i]; 
            if (LocalizationSettings.AvailableLocales.Locales[selected] == locale) {
                selected = i;
            }
            string localizedLocal = _currentStringTable[locale.name].LocalizedValue; 
            options.Add(new Dropdown.OptionData(localizedLocal)); 
        }
        dropdown.options = options; 
        dropdown.value = selected; 
        dropdown.onValueChanged.AddListener(LocaleSelected); 
        ChangeLanguage(); 
    }

    static void LocaleSelected(int index) {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index]; 
        PlayerPrefs.SetInt("Locale", index); 
    }

    public void LanguageDropDown(int value) {
        PlayerPrefs.SetInt("Locale", value);
        ChangeLanguage(); 
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
