using UnityEngine;
using UnityEngine.UI; 

public class FloatingText
{
    [SerializeField] private bool active;
    [SerializeField] private GameObject go;
    [SerializeField] private Text txt;
    [SerializeField] private Vector3 motion;
    [SerializeField] private float duration;
    [SerializeField] private float lastShown; 

    public void Show() {
        active = true;
        lastShown = Time.time;
        go.SetActive(active); 
    }

    public void Hide() {
        active = false;
        go.SetActive(active); 
    }

    public void UpdateFloatingText() {
        if (!active) {
            return; 
        }

        if (Time.time - lastShown > duration) {
            Hide(); 
        }

        go.transform.position += motion * Time.deltaTime; 
    }

    // get txt 
    public Text getTxt() {
        return txt; 
    }

    // get GameObject 
    public GameObject getGo() { 
        return go; 
    }

    // get motion
    public Vector3 getMotion() {
        return motion; 
    }

    // get duration
    public float getDuration() {
        return duration; 
    }

    public bool isActive() {
        return active; 
    }

    // set txt 
    public void setTxt(Text txt) {
        this.txt = txt; 
    }

    // set GameObject
    public void setGo(GameObject go) {
        this.go = go; 
    }

    // set motion 
    public void setMotion(Vector3 dir) {
        motion = dir; 
    }

    // set duration
    public void setDuration(float amt) {
        duration = amt; 
    }
}
