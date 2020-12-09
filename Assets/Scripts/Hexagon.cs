using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Hexagon : MonoBehaviour {
    public Color defaultColor;
    public static Color player1ActiveColor = new Color(26/255f, 164/255f, 242/255f);
    public static Color player2ActiveColor = new Color(225/255f, 45/255f, 37/255f);
    public static Color player1WinColor = new Color(167/255f, 214/255f, 247/255f);
    public static Color player2WinColor = new Color(255/255f, 90/255f, 94/255f);
    
    public int rotateAmount = 180;
    public int speed = 15;

    public AudioClip flipSound;

    [HideInInspector] public int x;
    [HideInInspector] public int y;

    public static int busyFlipping = 0;

    private SpriteRenderer spriteRenderer;
    public bool flipped = false;

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public IEnumerator Flip(bool undo = false) {
        // Wait for previous flip to finish
        busyFlipping++;

        // Flipping sound effect with random pitch
        SoundManager.instance.PlaySingle(flipSound);

        // Set the color according to player
        Color color;
        if (undo) {
            color = defaultColor;
            flipped = false;
        } else if (GameManager.instance.player1Turn)
            color = (GameManager.instance.gameOver ? player1WinColor : player1ActiveColor);
        else
            color = (GameManager.instance.gameOver ? player2WinColor : player2ActiveColor);

        // Spin halfway
        for (int i = 0; i < rotateAmount; i += 2 * speed) {
            transform.Rotate(Vector3.up * speed);
            yield return null;
        }
        // Change the color
        spriteRenderer.color = color;
        // Spin halfway again
        for (int i = 0; i < rotateAmount; i += 2 * speed) {
            transform.Rotate(Vector3.down * speed);
            yield return null;
        }

        busyFlipping--;
        if (!GameManager.instance.gameOver && !undo)
            GameManager.instance.player1Turn = !GameManager.instance.player1Turn;
    }

    private void OnMouseDown() {
        // Can only flip a tile once
        if (flipped || busyFlipping != 0 || EventSystem.current.IsPointerOverGameObject())
            return;
        // Flip and toggle the turn
        GameManager.instance.HandleFlip(x, y);
    }
}
