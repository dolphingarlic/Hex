using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour {
    public Toggle player1AiToggle;
    public Toggle player2AiToggle;

    private void Start() {
        // Same settings as last time
        player1AiToggle.isOn = PlayerPrefs.GetInt("player1IsAI") == 1;
        player2AiToggle.isOn = PlayerPrefs.GetInt("player2IsAI") == 1;

        player1AiToggle.onValueChanged.AddListener(delegate {
            SetPlayer1(player1AiToggle);
        });
        player2AiToggle.onValueChanged.AddListener(delegate {
            SetPlayer2(player2AiToggle);
        });
    }


    private void SetPlayer1(Toggle aiToggle) {
        PlayerPrefs.SetInt("player1IsAI", aiToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void SetPlayer2(Toggle aiToggle) {
        PlayerPrefs.SetInt("player2IsAI", aiToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();
    }
}
