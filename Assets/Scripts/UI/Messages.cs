using UnityEngine;

public class Messages : MonoBehaviour
{
    void OnGUI()
    {
        if (!GameManager.Instance.IsGameOver) return;

        GUIStyle style = new GUIStyle(GUI.skin.label)
        {
            fontSize = 40,
            alignment = TextAnchor.MiddleCenter
        };

        string message = GameManager.Instance.EndState == GameManager.GameEndState.Victory
            ? "Obrigado por jogar!"
            : "GAME OVER";

        Rect textRect = new Rect(0, Screen.height / 2 - 40, Screen.width, 80);
        GUI.Label(textRect, message, style);
    }
}