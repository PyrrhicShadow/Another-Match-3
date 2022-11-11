using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    [SerializeField] private AudioSource destroyNoise; 
    [SerializeField] private AudioSource winNoise; 
    [SerializeField] private AudioSource loseNoise; 
    [SerializeField] private AudioSource jumpNoise; 
    [SerializeField] private AudioSource backgroundMusic; 
    // private bool backgroundMusicOn = true; 


    public void PlayDestroyNoise() {
        destroyNoise.Play(); 
    }
    
    public void PlayWinNoise() {
        winNoise.Play(); 
    }

    public void PlayLoseNoise() {
        loseNoise.Play(); 
    }

    public void PlayJumpNoise() {
        jumpNoise.Play(); 
    }

    public void backgroundMusicOn(bool on) {
        if (on) {
            backgroundMusic.Pause(); 
        }
        else {
            backgroundMusic.UnPause(); 
        }
    }

}
