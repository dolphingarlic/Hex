using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour {
    public void RestartGame() {
        SceneManager.LoadScene("Game");
    }

    public void QuitToMenu() {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void SwapRule() {
        GameManager.instance.HandleSwapRule();
    }
}