using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScalar : MonoBehaviour {
    private Board board; 
    private float cameraOffset = -10; 
    [SerializeField] private float aspectRaio = 0.5625f;
    private float padding = 3f; 
    [SerializeField] private float yOffset = 1f; 

    // Start is called before the first frame update
    void Start() {
        board = FindObjectOfType<Board>(); 
        if (board != null) {
            RepositionCamera(board.getWidth() - 1, board.getHeight() - 1); 
        }
    }

    public void RepositionCamera(int x, int y) {
        Vector3 tempPos = new Vector3(x / 2, (y / 2) + yOffset, cameraOffset);
        transform.position = tempPos; 
        if (board.getWidth() >= board.getHeight()) {
            Camera.main.orthographicSize = (board.getWidth() / 2 + padding) / aspectRaio; 
        }
        else {
            Camera.main.orthographicSize = board.getHeight() / 2 + padding; 
        }
    }

    public float getYOffset() {
        return yOffset; 
    }

    public void setYOffset(float yOffset) {
        this.yOffset = yOffset; 
    }
}
