using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    public Toggle player1AiToggle;
    public Toggle player2AiToggle;
    public Slider player1LevelSlider;
    public Slider player2LevelSlider;
    public GameObject player1LevelNumber;
    public GameObject player2LevelNumber;

    private void Start()
    {
        // Human or AI settings
        player1AiToggle.isOn = PlayerPrefsX.GetBool("player1IsAI");
        player2AiToggle.isOn = PlayerPrefsX.GetBool("player2IsAI");

        player1AiToggle.onValueChanged.AddListener(delegate
        {
            SetPlayer1IsAI(player1AiToggle);
        });
        player2AiToggle.onValueChanged.AddListener(delegate
        {
            SetPlayer2IsAI(player2AiToggle);
        });

        // AI level settings
        if (!PlayerPrefs.HasKey("player1AILevel"))
        {
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

        player1LevelSlider.onValueChanged.AddListener(delegate
        {
            SetPlayer1AILevel(player1LevelSlider);
        });
        player2LevelSlider.onValueChanged.AddListener(delegate
        {
            SetPlayer2AILevel(player2LevelSlider);
        });

        // Colors
        PlayerPrefsX.SetColor("player1ActiveColor", new Color(26 / 255f, 164 / 255f, 242 / 255f));
        PlayerPrefsX.SetColor("player2ActiveColor", new Color(225 / 255f, 45 / 255f, 37 / 255f));
        PlayerPrefsX.SetColor("player1WinColor", new Color(167 / 255f, 214 / 255f, 247 / 255f));
        PlayerPrefsX.SetColor("player2WinColor", new Color(255 / 255f, 90 / 255f, 94 / 255f));
        PlayerPrefs.Save();
    }

    private void SetPlayer1IsAI(Toggle aiToggle)
    {
        PlayerPrefsX.SetBool("player1IsAI", aiToggle.isOn);
        PlayerPrefs.Save();
    }

    private void SetPlayer2IsAI(Toggle aiToggle)
    {
        PlayerPrefsX.SetBool("player2IsAI", aiToggle.isOn);
        PlayerPrefs.Save();
    }

    private void SetPlayer1AILevel(Slider levelSlider)
    {
        int player1Level = (int)levelSlider.value;
        PlayerPrefs.SetInt("player1AILevel", player1Level);
        PlayerPrefs.Save();
        player1LevelNumber.GetComponent<TextMeshProUGUI>().text = player1Level.ToString();
    }

    private void SetPlayer2AILevel(Slider levelSlider)
    {
        int player2Level = (int)levelSlider.value;
        PlayerPrefs.SetInt("player2AILevel", player2Level);
        PlayerPrefs.Save();
        player2LevelNumber.GetComponent<TextMeshProUGUI>().text = player2Level.ToString();
    }

    public void SetColor(Toggle colorToggle)
    {
        if (!colorToggle.isOn) return;

        ColorInfo colorInfo = colorToggle.gameObject.GetComponent<ColorInfo>();
        PlayerPrefsX.SetColor("player1ActiveColor", colorInfo.player1ActiveColor);
        PlayerPrefsX.SetColor("player2ActiveColor", colorInfo.player2ActiveColor);
        PlayerPrefsX.SetColor("player1WinColor", colorInfo.player1WinColor);
        PlayerPrefsX.SetColor("player2WinColor", colorInfo.player2WinColor);
        PlayerPrefs.Save();
    }
}
