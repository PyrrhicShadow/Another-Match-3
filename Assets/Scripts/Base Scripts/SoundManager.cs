using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    [SerializeField] private AudioSource destroyNoise; 
    [SerializeField] private AudioSource winNoise; 
    [SerializeField] private AudioSource loseNoise; 
    [SerializeField] private AudioSource jumpNoise; 
    [SerializeField] private AudioSource backgroundMusic; 
    private bool backgroundMusicOn = true; 
    private bool soundEffectsOn = true; 

    public void PlayDestroyNoise() {
        if (soundEffectsOn) {
            destroyNoise.Play(); 
        }
    }
    
    public void PlayWinNoise() {
        if (soundEffectsOn) {
            winNoise.Play(); 
        }
    }

    public void PlayLoseNoise() {
        if (soundEffectsOn) {
            loseNoise.Play(); 
        }
    }

    public void PlayJumpNoise() {
        jumpNoise.Play(); 
    }

    public void setBackgroundMusic(bool on) {
        if (on) {
            backgroundMusic.Pause(); 
        }
        else {
            backgroundMusic.UnPause(); 
        }
    }

    public bool isBackgroundMusicOn() {
        return backgroundMusicOn; 
    }

    public void setSoundEffects(bool on) {
        soundEffectsOn = on; 
    }

    public bool isSoundEffectsOn() {
        return soundEffectsOn; 
    }

}
