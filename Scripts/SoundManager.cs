using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public AudioSource[] destroyNoise; 

    public void PlayRandomDestroyNoise() {
        // Choose a random number 
        int noiseToPlay = Random.Range(0, destroyNoise.Length); 
        // play that clip
        destroyNoise[noiseToPlay].Play(); 
    }

}
