using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

public class FindMatches : MonoBehaviour {

    private Board board; 
    [SerializeField] private List<GameObject> currentMatches; 

    // Start is called before the first frame update
    void Start() {
        board = GameObject.FindWithTag("board").GetComponent<Board>(); 
        currentMatches = new List<GameObject>(); 
    }

    public void FindAllMatches() { 
        StartCoroutine(FindAllMatchesCo());  
    }

    private IEnumerator FindAllMatchesCo() {
        yield return new WaitForSeconds(0.2f); 
        for (int i = 0; i < board.getWidth(); i++) {
            for (int j = 0; j < board.getHeight(); j++) {
                GameObject currentDot = board.getDot(i, j); 
                if (currentDot != null) {
                    if (i > 0 && i < (board.getWidth() - 1)) {
                        GameObject leftDot = board.getDot(i - 1, j); 
                        GameObject rightDot = board.getDot(i + 1, j); 
                        if (leftDot != null && rightDot != null && leftDot != currentDot && rightDot != currentDot) {
                            if (leftDot.tag == currentDot.tag && rightDot.tag == currentDot.tag) {
                                // col/row bombs 
                                this.addRowMatches(currentDot.GetComponent<Dot>(), leftDot.GetComponent<Dot>(), rightDot.GetComponent<Dot>()); 
                                this.addColMatches(currentDot.GetComponent<Dot>(), leftDot.GetComponent<Dot>(), rightDot.GetComponent<Dot>()); 
                                // regular left/right matches
                                this.addMatches(currentDot, leftDot, rightDot); 
                                // adjacent bombs  
                                this.addAdjMatches(currentDot.GetComponent<Dot>(), leftDot.GetComponent<Dot>(), rightDot.GetComponent<Dot>()); 
                            }
                        }
                    }
                    if (j > 0 && j < board.getHeight() - 1) {
                        GameObject downDot = board.getDot(i, j - 1); 
                        GameObject upDot = board.getDot(i, j + 1); 
                        if (upDot != null && downDot != null && downDot != currentDot && upDot != currentDot) {
                            if (downDot.tag == currentDot.tag && upDot.tag == currentDot.tag) {
                                // col/row bombs
                                this.addRowMatches(currentDot.GetComponent<Dot>(), downDot.GetComponent<Dot>(), upDot.GetComponent<Dot>()); 
                                this.addColMatches(currentDot.GetComponent<Dot>(), downDot.GetComponent<Dot>(), upDot.GetComponent<Dot>()); 
                                // regular up/down matches
                                this.addMatches(currentDot, downDot, upDot); 
                                // adjacent bombs
                                this.addAdjMatches(currentDot.GetComponent<Dot>(), downDot.GetComponent<Dot>(), upDot.GetComponent<Dot>());
                            }
                        }
                    }
                }
            }
        }
    }

    // Adds row matches to currentMatches
    private void addRowMatches(Dot dot1, Dot dot2, Dot dot3) {
        if (dot1.isRowBomb()) {
            currentMatches.Union(getRowDots(dot1.getY())); 
        }
        if (dot2.isRowBomb()) {
            currentMatches.Union(getRowDots(dot2.getY())); 
        }
        if (dot3.isRowBomb()) {
            currentMatches.Union(getRowDots(dot3.getY())); 
        }
    }

    // Adds column matches to currentMatches
    private void addColMatches(Dot dot1, Dot dot2, Dot dot3) {
        if (dot1.isColBomb()) {
            currentMatches.Union(getColDots(dot1.getX())); 
        }
        if (dot2.isColBomb()) {
            currentMatches.Union(getColDots(dot2.getX())); 
        }
        if (dot3.isColBomb()) {
            currentMatches.Union(getColDots(dot3.getX())); 
        }
    }

    // Adds match-3 to currentMatches
    private void addMatches(GameObject dot1, GameObject dot2, GameObject dot3) {
        if (!currentMatches.Contains(dot1)) {
            currentMatches.Add(dot1); 
        }
        if (!currentMatches.Contains(dot2)) {
            currentMatches.Add(dot2); 
        }
        if (!currentMatches.Contains(dot3)) {
            currentMatches.Add(dot3);
        }
        // update matched status 
        dot1.GetComponent<Dot>().setMatched(true);  
        dot2.GetComponent<Dot>().setMatched(true);  
        dot3.GetComponent<Dot>().setMatched(true); 
    }

    // Adds adjacent matches to currentMatches 
    private void addAdjMatches(Dot dot1, Dot dot2, Dot dot3) {
        if (dot1.isAdjBomb()) {
            currentMatches.Union(getAdjDots(dot1.getX(), dot1.getY())); 
        }
        if (dot2.isAdjBomb()) {
            currentMatches.Union(getAdjDots(dot2.getX(), dot2.getY())); 
        }
        if (dot3.isAdjBomb()) {
            currentMatches.Union(getAdjDots(dot3.getX(), dot3.getY())); 
        }
    }

    // returns all dots in given column
    private List<GameObject> getColDots(int col) {
        List<GameObject> dots = new List<GameObject>(); 
        for (int i = 0; i < board.getHeight(); i++) {
            if (board.getDot(col, i) != null) {
                Dot dot = board.getDot(col, i).GetComponent<Dot>(); 
                if (dot.isRowBomb()) {
                    dots.Union(getRowDots(i)); 
                }
                dots.Add(board.getDot(col, i));  
                dot.setMatched(true);  
            }
        }
        return dots; 
    }

    /// <summary>returns all dots in given row</summary>
    private List<GameObject> getRowDots(int row) {
        List<GameObject> dots = new List<GameObject>(); 
        for (int i = 0; i < board.getWidth(); i++) {
            if (board.getDot(i, row) != null) {
                Dot dot = board.getDot(i, row).GetComponent<Dot>();
                if (dot.isColBomb()) {
                    dots.Union(getColDots(i)); 
                }
                dots.Add(board.getDot(i, row)); 
                dot.setMatched(true);  
            }
        }
        return dots; 
    }

    /// <summary>returns all dots adjacent to given dot </summary>
    private List<GameObject> getAdjDots(int x, int y) {
        List<GameObject> dots = new List<GameObject>(); 
        for (int i = x - 1; i <= x + 1; i++) {
            // check if dot exists
            for (int j = y - 1; j <= y + 1; j++) {
                if (i >= 0 && i < board.getWidth() && j >= 0 && j < board.getHeight()) {
                    if (board.getDot(i, j) != null) {
                        dots.Add(board.getDot(i, j)); 
                        board.getDot(i, j).GetComponent<Dot>().setMatched(true); 
                    }
                }
            }
        }
        return dots; 
    }

    public void MatchColors(string color) {
        for (int i = 0; i < board.getWidth(); i++) {
            for (int j = 0; j < board.getHeight(); j++) {
                // Check if that piece exists 
                if (board.getDot(i, j) != null) {
                    // check tag on dot 
                    if (board.getDot(i, j).CompareTag(color)) {
                        board.getDot(i, j).GetComponent<Dot>().setMatched(true); 
                    } 
                }
            }
        }
    }


    /// <summary>returns true if there are 5 in a row or column, else false (like a T-shape or L-shape match)</summary>
    private bool colRowMatch() {
        int col = 0; 
        int row = 0; 
        Dot first = currentMatches[0].GetComponent<Dot>(); 
        if (first != null) {
            foreach (GameObject currentDot in currentMatches) {
                Dot dot = currentDot.GetComponent<Dot>(); 
                if (dot.getY() == first.getY()){
                    row++; 
                }
                if(dot.getX() == first.getX()) {
                    col++; 
                }
            }
        }
        Debug.Log("col: " + col + " | row: " + row); 
        return (col == 5 || row == 5); // returns true if there are 5 in a row or column 
    }

    /// <summary>When bomb is needed, make a bomb here.</summary>
    public void makeBomb() {
        int matches = getCurrentMatches().Count; 
        if (matches == 4 || matches == 7) {
            // make a col/row bomb
            int typeOfBomb = Random.Range(0, 100); 
            if (typeOfBomb < 50) {
                // Make a row bomb
                if (board.getCurrentDot() != null) {
                    if (board.getCurrentDot().isMatched()) {
                        if (!board.getCurrentDot().isRowBomb() && !board.getCurrentDot().isColBomb()){
                            board.getCurrentDot().setMatched(false); 
                            board.getCurrentDot().makeRowBomb(); 
                            Debug.Log("Row bomb generatd");
                        }
                    }
                    // then, is other dot matched?
                    else if (board.getCurrentDot().getOtherDot() != null) {
                        Dot otherDot = board.getCurrentDot().getOtherDot().GetComponent<Dot>(); 
                        if (otherDot.isMatched()) {
                            if (!otherDot.isRowBomb() && !otherDot.isColBomb()){
                                otherDot.setMatched(false); 
                                otherDot.makeRowBomb(); 
                                Debug.Log("Row bomb generatd");
                            }
                        }
                    }
                }
            }
            else {
                // Make a col bomb 
                if (board.getCurrentDot() != null) {
                    if (board.getCurrentDot().isMatched()) {
                        if (!board.getCurrentDot().isRowBomb() && !board.getCurrentDot().isColBomb()){
                            board.getCurrentDot().setMatched(false); 
                            board.getCurrentDot().makeColBomb(); 
                            Debug.Log("Column bomb generatd");
                        }
                    }
                    // then, is other dot matched?
                    else if (board.getCurrentDot().getOtherDot() != null) {
                        Dot otherDot = board.getCurrentDot().getOtherDot().GetComponent<Dot>(); 
                        if (otherDot.isMatched()) {
                            if (!otherDot.isRowBomb() && !otherDot.isColBomb()){
                                otherDot.setMatched(false); 
                                otherDot.makeColBomb(); 
                                Debug.Log("Column bomb generatd");
                            }
                        }
                    }
                }
            }
        }
        if (matches == 5 || matches == 7) {
            // make a adj/color bomb 
            if (this.colRowMatch()) {
                // make a color bomb
                // is current dot matched? 
                if (board.getCurrentDot() != null) {
                    if (board.getCurrentDot().isMatched()) {
                        if (!board.getCurrentDot().isColorBomb()){
                            board.getCurrentDot().setMatched(false); 
                            board.getCurrentDot().makeColorBomb(); 
                            Debug.Log("Color bomb generatd");
                        }
                    }
                    // then, is other dot matched?
                    else if (board.getCurrentDot().getOtherDot() != null) {
                        Dot otherDot = board.getCurrentDot().getOtherDot().GetComponent<Dot>(); 
                        if (otherDot.isMatched()) {
                            if (!otherDot.isColorBomb()){
                                otherDot.setMatched(false); 
                                otherDot.makeColorBomb(); 
                                Debug.Log("Color bomb generatd");
                            }
                        }
                    }
                }
            }
            else {
                // make an adjacent bomb
                // is current dot matched? 
                if (board.getCurrentDot() != null) {
                    if (board.getCurrentDot().isMatched()) {
                        if (!board.getCurrentDot().isAdjBomb()){
                            board.getCurrentDot().setMatched(false); 
                            board.getCurrentDot().makeAdjBomb(); 
                            Debug.Log("Adjacent bomb generatd");
                        }
                    }
                    // then, is other dot matched?
                    else if (board.getCurrentDot().getOtherDot() != null) {
                        Dot otherDot = board.getCurrentDot().getOtherDot().GetComponent<Dot>(); 
                        if (otherDot.isMatched()) {
                            if (!otherDot.isAdjBomb()){
                                otherDot.setMatched(false); 
                                otherDot.makeAdjBomb(); 
                                Debug.Log("Adjacent bomb generatd");
                            }
                        }
                    }
                } 
            }
        }

    }
    
    public void CheckBombs(int matches) { 
        if (board.getCurrentDot() != null) {
            // Is the dot they moved matched? 
            if (board.getCurrentDot().isMatched()) {
                // make it unmatched so it can become a bomb 
                board.getCurrentDot().setMatched(false); 
                // decide what kind of bomb to make 
                this.makeBomb(board.getCurrentDot(), matches); 
            }
            else if (board.getCurrentDot().getOtherDot() != null){
                Dot otherDot = board.getCurrentDot().getOtherDot().GetComponent<Dot>(); 
                // is the other dot matched? 
                if (otherDot.isMatched()) {
                    // make it unmatched so it can become a bomb 
                    otherDot.setMatched(false); 
                    // decide what kind of bomb to make 
                    this.makeBomb(otherDot, matches); 
                }
            }
        }
    }

    /* 
    // first, check if the two dots being moved are rainbows 
            if (board.getCurrentDot().tag == "rainbow" && otherDot.gameObject.tag == "rainbow") {
                for (int i = 0; i < board.getWidth(); i++) {
                    for (int j = 0; j < board.getHeight(); j++) {
                        board.getDot(i, j).GetComponent<Dot>().setMatched(true); 
                    }
                }
                Debug.Log("Double rainbow! Board cleared."); 
            }
            */

    /// <summary>Make a bomb at dot <paramref name="dot"/> of type appropriate for <paramref name="matches"/> matches</summary>
    private void makeBomb(Dot dot, int matches) {
        if (matches == 4) {
            int typeOfBomb = Random.Range(0, 100); 
            if (typeOfBomb < 50) {
                // Make a row bomb
                dot.makeRowBomb(); 
            }
            else {
                // Make a col bomb
                dot.makeColBomb(); 
            }
        }
        else if (matches == 5) {
            // Make a color bomb
            dot.makeColorBomb(); 
        }
    }

    private bool ChekcForMatches() {
        for (int i = 0; i < board.getWidth() - 2; i++) {
            for (int j = 0; j < board.getHeight() - 2; j++) {
                if (board.getDot(i, j) != null) {
                    // grab this dot's tag 
                    string dotTag = board.getDot(i, j).tag; 
                    // check right and two to the right
                    if (board.getDot(i + 1, j) != null && board.getDot(i + 2, j) != null) {
                        if (board.getDot(i + 1, j).CompareTag(dotTag) && board.getDot(i + 2, j).CompareTag(dotTag)) {
                            return true; 
                        }
                    }
                    // check up and two to the up
                    if (board.getDot(i, j + 1) != null && board.getDot(i, j + 2) != null) {
                        if (board.getDot(i, j + 1).CompareTag(dotTag) && board.getDot(i, j + 2).CompareTag(dotTag)) {
                            return true; 
                        }
                    } 
                }
            }
        }
        return false; 
    }

    public bool SwitchAndCheck(int x, int y, Vector2 dir) {
        board.SwitchPieces(x, y, dir); 
        if (ChekcForMatches()) {
            board.SwitchPieces(x, y, dir); 
            return true; 
        }
        board.SwitchPieces(x, y, dir); 
        return false; 
    }

    /// <summary>finds all possible matches on board and returns one random one</summary>
    private List<GameObject> GetMatches() {
        // find all possible matches 
        List<GameObject> allMatches = new List<GameObject>();
        for (int i = 0; i < board.getWidth() - 2; i++) {
            for (int j = 0; j < board.getHeight() - 2; j++) {
                if (board.getDot(i, j) != null) {
                    if (SwitchAndCheck(i, j, Vector2.right)) {
                        // not deadlocked
                        allMatches.Add(board.getDot(i, j)); 
                    } 
                    if (SwitchAndCheck(i, j, Vector2.up)) {
                        // not deadlocked
                        allMatches.Add(board.getDot(i, j)); 
                    }
                }
            }
        }
        return allMatches; 
    }

    /// <summary>Picks a random match from a list of matches</summary>
    public GameObject RandomMatch() {
        List<GameObject> possibleMoves = this.GetMatches(); 
        if (possibleMoves.Count > 0) {
            int dotToUse = Random.Range(0, possibleMoves.Count); 
            return possibleMoves[dotToUse]; 
        }
        return null; 
    }

    /// <summary>returns currentMatches</summary>
    public List<GameObject> getCurrentMatches() {
        return currentMatches; 
    }
}
