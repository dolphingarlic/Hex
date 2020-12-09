using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour {
    public Toggle player1AiToggle;
    public Toggle player2AiToggle;
    public Slider player1LevelSlider;
    public Slider player2LevelSlider;
    public GameObject player1LevelNumber;
    public GameObject player2LevelNumber;

    private void Start() {
        // Human or AI settings
        player1AiToggle.isOn = PlayerPrefs.GetInt("player1IsAI") == 1;
        player2AiToggle.isOn = PlayerPrefs.GetInt("player2IsAI") == 1;

        player1AiToggle.onValueChanged.AddListener(delegate {
            SetPlayer1IsAI(player1AiToggle);
        });
        player2AiToggle.onValueChanged.AddListener(delegate {
            SetPlayer2IsAI(player2AiToggle);
        });

        // AI level settings
        if (PlayerPrefs.GetInt("player1AILevel") == 0) {
            PlayerPrefs.SetInt("player1AILevel", 2);
            PlayerPrefs.SetInt("player2AILevel", 2);
            PlayerPrefs.Save();
        }

        int player1Level = PlayerPrefs.GetInt("player1AILevel");
        int player2Level = PlayerPrefs.GetInt("player2AILevel");
        player1LevelNumber.GetComponent<TextMeshProUGUI>().text = player1Level.ToString();
        player1LevelSlider.value = player1Level;
        player2LevelNumber.GetComponent<TextMeshProUGUI>().text = player2Level.ToString();
        player2LevelSlider.value = player2Level;

        player1LevelSlider.onValueChanged.AddListener(delegate {
            SetPlayer1AILevel(player1LevelSlider);
        });
        player2LevelSlider.onValueChanged.AddListener(delegate {
            SetPlayer2AILevel(player2LevelSlider);
        });
    }

    private void SetPlayer1IsAI(Toggle aiToggle) {
        PlayerPrefs.SetInt("player1IsAI", aiToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void SetPlayer2IsAI(Toggle aiToggle) {
        PlayerPrefs.SetInt("player2IsAI", aiToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void SetPlayer1AILevel(Slider levelSlider) {
        int player1Level = (int)levelSlider.value;
        PlayerPrefs.SetInt("player1AILevel", player1Level);
        PlayerPrefs.Save();
        player1LevelNumber.GetComponent<TextMeshProUGUI>().text = player1Level.ToString();
    }

    private void SetPlayer2AILevel(Slider levelSlider) {
        int player2Level = (int)levelSlider.value;
        PlayerPrefs.SetInt("player2AILevel", player2Level);
        PlayerPrefs.Save();
        player2LevelNumber.GetComponent<TextMeshProUGUI>().text = player2Level.ToString();
    }
}
