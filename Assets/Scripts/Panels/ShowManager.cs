using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShowManager : MonoBehaviour
{
    [SerializeField] private GameObject EndLevelPanel;
    [SerializeField] private Button returnToLevelSelectButton;
    [SerializeField] private string levelSelectSceneName = "Menu";
    [SerializeField] private TMP_Text uiScoreText;
    [SerializeField] private TurnCounter turnCounter;
     
    public int turns;

    private void Awake()
    {
        EndLevelPanel.SetActive(false);
        if (returnToLevelSelectButton != null)
        {
            returnToLevelSelectButton.gameObject.SetActive(false);
            returnToLevelSelectButton.onClick.AddListener(ReturnToLevelSelect);
        }

    }

    private void OnDestroy()
    {
        if (returnToLevelSelectButton != null)
            returnToLevelSelectButton.onClick.RemoveListener(ReturnToLevelSelect);
    }
    
    private void OnEnable()
    {
        if (turnCounter == null) 
            turnCounter = FindAnyObjectByType<TurnCounter>();
    }
    
    public void ShowInVictory()
    {
        EndLevelPanel.SetActive(true);

        if (turnCounter == null)
            turnCounter = FindAnyObjectByType<TurnCounter>();

        if (turnCounter == null)
        {
            Debug.LogError("TurnCounter was not found when showing the victory panel.", this);
            return;
        }

        turns = turnCounter.Turns;
        uiScoreText.text = $"Parabéns! Você venceu em {turns} turnos";

        if (returnToLevelSelectButton != null)
            returnToLevelSelectButton.gameObject.SetActive(true);
    }

    public void ReturnToLevelSelect()
    {
        Time.timeScale = 1f;
        PlayerPrefs.SetInt("OpenLevelSelect", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene(levelSelectSceneName);
    }
}
