using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 
using System.IO; 
using System.Runtime.Serialization.Formatters.Binary; 

[Serializable]
public class SaveData {
    [SerializeField] private bool[] actives; 
    [SerializeField] private int[] highScores; 
    [SerializeField] private int[] stars; 
    [SerializeField] private bool summon; 

    public void setActive(int i, bool status) {
        actives[i] = status; 
    }

    public bool isActive(int i) {
        return actives[i]; 
    }

    public void setStar(int i, int star) {
        stars[i] = star; 
    }

    public int getStar(int i) {
        return stars[i];
    }

    public void setHighScore(int i, int score) {
        highScores[i] = score; 
    }

    public int getHighScore(int i) {
        return highScores[i];
    }

    public void Summoned() {
        summon = true; 
    }

    public bool hasSummoned() {
        return summon; 
    }
}

public class GameData : MonoBehaviour {

    public static GameData gameData; 
    public SaveData saveData; 
    // Start is called before the first frame update
    void Awake()
    {
        if (gameData == null) {
            DontDestroyOnLoad(this.gameObject); 
            gameData = this; 
        }
        else {
            Destroy(this.gameObject); 
        }
        
        Load(); 
    }

    private void Start() {

    }

    public void Save() {
        // create a binary formatter that can read binary files 
        BinaryFormatter formatter = new BinaryFormatter(); 

        // open file stream 
        FileStream file = File.Open(Application.persistentDataPath + "/player.dat", FileMode.Create); 

        // create a copy of save data
        SaveData data = new SaveData();
        data = saveData;  


        // write the save data to the file
        formatter.Serialize(file, data); 

        // close data stream *important*
        file.Close(); 

        Debug.Log("Saved"); 
    }

    public void Load() {
        // Check if the save game file exists 
        if (File.Exists(Application.persistentDataPath + "/player.dat")) {
            // create a binary formatter 
            BinaryFormatter formatter = new BinaryFormatter(); 
            FileStream file = File.Open(Application.persistentDataPath + "/player.dat", FileMode.Open); 

            saveData = formatter.Deserialize(file) as SaveData; 

            file.Close(); 
            Debug.Log("Save loaded from file"); 
        }
    }

    private void OnDisable() {
        Save(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
