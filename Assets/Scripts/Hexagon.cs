using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Hexagon : MonoBehaviour {
    public Color player1Color;
    public Color player2Color;
    public Color player1WinColor;
    public Color player2WinColor;
    
    public int rotateAmount = 180;
    public int speed = 15;

    public AudioClip flipSound;

    [HideInInspector] public int x;
    [HideInInspector] public int y;

    public static bool busyFlipping = false;

    private SpriteRenderer spriteRenderer;
    private bool flipped = false;

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public IEnumerator Flip() {
        // Wait for previous flip to finish
        busyFlipping = true;

        // Flipping sound effect with random pitch
        SoundManager.instance.PlaySingle(flipSound);

        // Set the color according to player
        Color color;
        if (GameManager.instance.player1Turn)
            color = (GameManager.instance.gameOver ? player1WinColor : player1Color);
        else
            color = (GameManager.instance.gameOver ? player2WinColor : player2Color);

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

        busyFlipping = false;
        if (!GameManager.instance.gameOver)
            GameManager.instance.player1Turn = !GameManager.instance.player1Turn;
    }

    private void OnMouseDown() {
        // Can only flip a tile once
        if (flipped || busyFlipping || EventSystem.current.IsPointerOverGameObject())
            return;
        // Don't flip again
        flipped = true;
        // Flip and toggle the turn
        GameManager.instance.HandleFlip(this);
    }
}
