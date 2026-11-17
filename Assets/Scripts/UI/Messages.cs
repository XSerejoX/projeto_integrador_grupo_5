using UnityEngine;

public class Messages : MonoBehaviour
{
     
    public int turnCounter;
    

    [SerializeField] private TurnCounter turnCounterScript;

    
    void OnGUI()
    {
            //acessando contador de turnos
        turnCounterScript = FindAnyObjectByType<TurnCounter>();
        if (turnCounterScript != null)
        {
            turnCounter = turnCounterScript.Turns;
        }
        
        if (!GameManager.Instance.IsGameOver) return;


        GUIStyle style = new GUIStyle(GUI.skin.label)
        
        {
            fontSize = 32,
            alignment = TextAnchor.MiddleCenter
            
        };
        
        style.normal.textColor = Color.black;
        
        string message = GameManager.Instance.EndState == GameManager.GameEndState.Victory
            ? $"Vitória! {turnCounter} turnos"
            : $"FIM DE JOGO {turnCounter} turnos";

        Rect textRect = new Rect(0, Screen.height / 2 - 40, Screen.width, 80);
        GUI.Label(textRect, message, style);
    }
}