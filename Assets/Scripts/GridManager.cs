using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {
    public Transform hexPrefab;
    // public Transform redBorderPrefab;
    // public Transform blueBorderPrefab;

    public static int gridWidth = 11;
    public static int gridHeight = 11;
    public float gap = 0.1f;

    private Transform gridHolder;

    private float hexWidth = 0.866f;
    private float hexHeight = 1.0f;
    private Vector3 startPos;

    private void AddGap() {
        hexWidth += hexWidth * gap;
        hexHeight += hexHeight * gap;
    }

    private void CalcStartPos() {
        float offset = (gridHeight / 2 % 2 != 0 ? gridWidth / 2 * hexWidth / 2 : 0);

        float x = -hexWidth * (gridWidth / 2) - offset;
        float y = hexHeight * 0.75f * (gridHeight / 2);

        startPos = new Vector3(x, y, 0);
    }

    private Vector3 CalcWorldPos(Vector2 gridPos) {
        float offset = gridPos.y * hexWidth / 2;

        float x = startPos.x + gridPos.x * hexWidth + offset;
        float y = startPos.y - gridPos.y * hexHeight * 0.75f;

        return new Vector3(x, y, 0);
    }

    public void CreateGrid() {
        AddGap();
        CalcStartPos();

        gridHolder = new GameObject("Grid").transform;

        // Transform blueBorder1 = Instantiate(blueBorderPrefab) as Transform;
        // blueBorder1.position = new Vector3(9.7f, 0, 0.1f);
        // blueBorder1.Rotate(new Vector3(0, 60, 0));
        // blueBorder1.transform.SetParent(gridHolder);
        // Transform blueBorder2 = Instantiate(blueBorderPrefab) as Transform;
        // blueBorder2.position = new Vector3(-9.7f, 0, -0.1f);
        // blueBorder2.Rotate(new Vector3(0, -120, 0));
        // blueBorder2.transform.SetParent(gridHolder);
        
        // Transform redBorder1 = Instantiate(redBorderPrefab) as Transform;
        // redBorder1.position = new Vector3(-4.763f, 0, 8.45f);
        // redBorder1.transform.SetParent(gridHolder);
        // Transform redBorder2 = Instantiate(redBorderPrefab) as Transform;
        // redBorder2.position = new Vector3(4.763f, 0, -8.45f);
        // redBorder2.Rotate(new Vector3(0, 180, 0));
        // redBorder2.transform.SetParent(gridHolder);

        for (int y = 0; y < gridHeight; y++) {
            for (int x = 0; x < gridWidth; x++) {
                Transform tile = Instantiate(hexPrefab) as Transform;
                Vector2 gridPos = new Vector2(x, y);
                // Move to the correct location
                tile.position = CalcWorldPos(gridPos);
                // Set the parent to the grid for neatness
                tile.transform.SetParent(gridHolder);

                Hexagon hex = tile.GetComponent<Hexagon>();
                hex.x = x;
                hex.y = y;
            }
        }
    }
}
