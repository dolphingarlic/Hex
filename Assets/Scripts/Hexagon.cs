using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Hexagon : MonoBehaviour
{
    public Color defaultColor;

    public int rotateAmount = 180;
    public int speed = 15;

    public AudioClip flipSound;

    [HideInInspector] public int x;
    [HideInInspector] public int y;

    public static int busyFlipping = 0;

    private SpriteRenderer spriteRenderer;

    private Color player1ActiveColor;
    private Color player2ActiveColor;
    private Color player1WinColor;
    private Color player2WinColor;
    public bool flipped = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        player1ActiveColor = PlayerPrefsX.GetColor("player1ActiveColor");
        player2ActiveColor = PlayerPrefsX.GetColor("player2ActiveColor");
        player1WinColor = PlayerPrefsX.GetColor("player1WinColor");
        player2WinColor = PlayerPrefsX.GetColor("player2WinColor");
    }

    public IEnumerator Flip(bool undo = false)
    {
        // Wait for colors to be loaded
        while (player1ActiveColor.Equals(Color.clear) || player2ActiveColor.Equals(Color.clear))
            yield return null;

        // Wait for previous flip to finish
        busyFlipping++;

        // Flipping sound effect with random pitch
        SoundManager.instance.PlaySingle(flipSound);

        // Set the color according to player
        Color color;
        if (undo)
        {
            color = defaultColor;
            flipped = false;
        }
        else if (GameManager.instance.player1Turn)
            color = (GameManager.instance.gameOver ? player1WinColor : player1ActiveColor);
        else
            color = (GameManager.instance.gameOver ? player2WinColor : player2ActiveColor);

        // Spin halfway
        for (int i = 0; i < rotateAmount; i += 2 * speed)
        {
            transform.Rotate(Vector3.up * speed);
            yield return null;
        }
        // Change the color
        spriteRenderer.color = color;
        // Spin halfway again
        for (int i = 0; i < rotateAmount; i += 2 * speed)
        {
            transform.Rotate(Vector3.down * speed);
            yield return null;
        }

        busyFlipping--;
        if (!GameManager.instance.gameOver && !undo)
            GameManager.instance.player1Turn = !GameManager.instance.player1Turn;
    }

    private void OnMouseDown()
    {
        // Can only flip a tile once
        if (flipped || busyFlipping != 0)
            return;
        // Don't flip on AI turn
        if ((GameManager.instance.player1Turn && PlayerPrefsX.GetBool("player1IsAI")) ||
            (!GameManager.instance.player1Turn && PlayerPrefsX.GetBool("player2IsAI")))
            return;
        // Don't flip when paused
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        // Flip and toggle the turn
        GameManager.instance.HandleFlip(x, y);
    }
}
