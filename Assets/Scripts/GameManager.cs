using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour {
    [HideInInspector] public bool player1Turn = true;
    [HideInInspector] public bool gameOver = false;
    
    public static GameManager instance = null;
    public Hexagon[] hex = new Hexagon[GridManager.gridWidth * GridManager.gridHeight];

    private GridManager gridManager;
    private GameObject gameOverMenu;
    private GameObject swapButton;

    private int totCells;
    private int[] dsu = new int[GridManager.gridWidth * GridManager.gridHeight + 4];
    private int[] color = new int[GridManager.gridWidth * GridManager.gridHeight];
    private bool[] visited = new bool[GridManager.gridWidth * GridManager.gridHeight];

    private int moves;

    private readonly int[] dx = {0, 1, -1, 1, -1, 0};
    private readonly int[] dy = {-1, -1, 0, 0, 1, 1};

    private int DsuFind(int a) {
        while (dsu[a] != a) {
            dsu[a] = dsu[dsu[a]];
            a = dsu[a];
        }
        return a;
    }

    private void DsuUnion(int a, int b) {
        dsu[DsuFind(a)] = DsuFind(b);
    }

    private void Start() {
        Application.targetFrameRate = 60;

        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);

        gameOverMenu = GameObject.Find("GameOverMenu");
        gameOverMenu.SetActive(false);
        swapButton = GameObject.Find("SwapButton");
        swapButton.SetActive(false);

        moves = 0;

        totCells = GridManager.gridWidth * GridManager.gridHeight;
        for (int i = 0; i < totCells + 4; i++)
            dsu[i] = i;
        
        gridManager = GetComponent<GridManager>();
        gridManager.CreateGrid();
    }

    public static int Normalize(int x, int y) {
        // Turn a pair into a single integer
        return x * GridManager.gridHeight + y;
    }

    private IEnumerator DfsAndFlip(int x, int y) {
        // Flip and mark as visited
        int currNorm = Normalize(x, y);
        visited[currNorm] = true;
        StartCoroutine(hex[currNorm].Flip());
        yield return new WaitForSeconds(0.15f);

        for (int i = 0; i < 6; i++) {
            int nx = x + dx[i];
            int ny = y + dy[i];
            if (nx == -1 || ny == -1 || nx == GridManager.gridWidth || ny == GridManager.gridHeight)
                continue;
            int nextNorm = Normalize(nx, ny);
            if (color[nextNorm] == color[currNorm] && !visited[nextNorm])
                StartCoroutine(DfsAndFlip(nx, ny));
        }
    }

    private void UnionWithNeighbours(int x, int y) {
        int currNorm = Normalize(x, y);
        for (int i = 0; i < 6; i++) {
            int nx = x + dx[i];
            int ny = y + dy[i];
            // Union with sides if needed
            if (nx == -1 && color[currNorm] == 1) // Left side
                DsuUnion(currNorm, GridManager.gridWidth * GridManager.gridHeight);
            else if (nx == GridManager.gridWidth && color[currNorm] == 1) // Right side
                DsuUnion(currNorm, GridManager.gridWidth * GridManager.gridHeight + 1);
            else if (ny == -1 && color[currNorm] == 2) // Top side
                DsuUnion(currNorm, GridManager.gridWidth * GridManager.gridHeight + 2);
            else if (ny == GridManager.gridHeight && color[currNorm] == 2) // Bottom side
                DsuUnion(currNorm, GridManager.gridWidth * GridManager.gridHeight + 3);
            // Union with other cells
            if (nx == -1 || ny == -1 || nx == GridManager.gridWidth || ny == GridManager.gridHeight)
                continue;
            int nextNorm = Normalize(nx, ny);
            if (color[nextNorm] == color[currNorm])
                DsuUnion(currNorm, nextNorm);
        }
    }

    public void HandleSwapRule() {
        Debug.Assert(moves == 1);
        for (int x = 0; x < GridManager.gridWidth; x++) {
            for (int y = 0; y < GridManager.gridHeight; y++) {
                int currNorm = Normalize(x, y);
                if (color[currNorm] != 0) {
                    if (x != y) // Ensure a single flip
                        StartCoroutine(hex[currNorm].Flip(true));
                    // "Reset" the cell
                    color[currNorm] = 0;
                    dsu[currNorm] = currNorm;
                    // Flip opposite cell
                    HandleFlip(y, x);
                    return;
                }
            }
        }
    }

    public void HandleFlip(int x, int y) {
        // Normalize the coordinates and flip
        int currNorm = Normalize(x, y);
        StartCoroutine(hex[currNorm].Flip());
        color[currNorm] = (player1Turn ? 1 : 2);
        UnionWithNeighbours(x, y);

        // Only allow swap rule after first turn
        swapButton.SetActive(++moves == 1);

        if (DsuFind(totCells) == DsuFind(totCells + 1)) {
            // Player 1 has won
            gameOver = true;
            StartCoroutine(DfsAndFlip(x, y));
            gameOverMenu.SetActive(true);
            TextMeshProUGUI tmp = gameOverMenu.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            tmp.text = "Player 1 wins!";
        } else if (DsuFind(totCells + 2) == DsuFind(totCells + 3)) {
            // Player 2 has won
            gameOver = true;
            StartCoroutine(DfsAndFlip(x, y));
            gameOverMenu.SetActive(true);
            TextMeshProUGUI tmp = gameOverMenu.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            tmp.text = "Player 2 wins!";
        }
    }
}
