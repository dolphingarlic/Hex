using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour {
    [HideInInspector] public bool player1Turn = true;
    [HideInInspector] public bool gameOver = false;
    
    public static GameManager instance = null;

    private GridManager gridManager;
    private GameObject gameOverMenu;

    private int totCells;
    private int[] dsu = new int[GridManager.gridWidth * GridManager.gridHeight + 4];
    private int[] colour = new int[GridManager.gridWidth * GridManager.gridHeight];
    private Hexagon[] hex = new Hexagon[GridManager.gridWidth * GridManager.gridHeight];
    private bool[] visited = new bool[GridManager.gridWidth * GridManager.gridHeight];

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

        totCells = GridManager.gridWidth * GridManager.gridHeight;
        for (int i = 0; i < totCells + 4; i++)
            dsu[i] = i;
        
        gridManager = GetComponent<GridManager>();
        gridManager.CreateGrid();
    }

    private int Normalize(int x, int y) {
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
            if (colour[nextNorm] == colour[currNorm] && !visited[nextNorm])
                StartCoroutine(DfsAndFlip(nx, ny));
        }
    }

    public void HandleFlip(Hexagon h) {
        // First, flip the hexagon
        StartCoroutine(h.Flip());

        // Get the coordinates and normalize them
        int x = h.x;
        int y = h.y;
        int currNorm = Normalize(x, y);
        colour[currNorm] = (player1Turn ? 1 : 2);
        hex[currNorm] = h;

        for (int i = 0; i < 6; i++) {
            int nx = x + dx[i];
            int ny = y + dy[i];
            // Union with sides if needed
            if (nx == -1 && colour[currNorm] == 1) // Left side
                DsuUnion(currNorm, GridManager.gridWidth * GridManager.gridHeight);
            else if (nx == GridManager.gridWidth && colour[currNorm] == 1) // Right side
                DsuUnion(currNorm, GridManager.gridWidth * GridManager.gridHeight + 1);
            else if (ny == -1 && colour[currNorm] == 2) // Top side
                DsuUnion(currNorm, GridManager.gridWidth * GridManager.gridHeight + 2);
            else if (ny == GridManager.gridHeight && colour[currNorm] == 2) // Bottom side
                DsuUnion(currNorm, GridManager.gridWidth * GridManager.gridHeight + 3);
            // Union with other cells
            if (nx == -1 || ny == -1 || nx == GridManager.gridWidth || ny == GridManager.gridHeight)
                continue;
            int nextNorm = Normalize(nx, ny);
            if (colour[nextNorm] == colour[currNorm])
                DsuUnion(currNorm, nextNorm);
        }

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
