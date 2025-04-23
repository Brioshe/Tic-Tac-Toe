using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour {
    public TicTacToeGame game;
    public TextMeshProUGUI victoryTxt;

    void Start() {
        if (victoryTxt != null) {
            victoryTxt.text = "";
        }
        if (game != null) {
            game.SetUI(this);
        }
    }
    public void OnReset() {
        if (game != null && victoryTxt != null) {
            victoryTxt.text = "";
            game.ResetGame();
        }
    }
}