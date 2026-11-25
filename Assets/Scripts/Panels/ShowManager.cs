using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShowManager : MonoBehaviour
{
    [SerializeField] private GameObject VictoryLevelPanel;
    [SerializeField] private GameObject LostlevelPanel;
    [SerializeField] private Button returnToLevelSelectButton;
    [SerializeField] private Button returnToLevelSelectButton2;
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button restartButton2;
    [SerializeField] private string levelSelectSceneName = "Menu";
    [SerializeField] private TMP_Text uiScoreText;
    [SerializeField] private TurnCounter turnCounter;
    [SerializeField] private int sceneAfterLastLevel = 0;

    public int turns;

    private void Awake()
    {
        VictoryLevelPanel.SetActive(false);
        LostlevelPanel.SetActive(false);
        if (returnToLevelSelectButton != null)
        {
            returnToLevelSelectButton.gameObject.SetActive(false);
            returnToLevelSelectButton.onClick.AddListener(ReturnToLevelSelect);
        }
        if (returnToLevelSelectButton2 != null)
        {
            returnToLevelSelectButton2.gameObject.SetActive(false);
            returnToLevelSelectButton2.onClick.AddListener(ReturnToLevelSelect);
        }
        if (nextLevelButton != null)
        {
            nextLevelButton.gameObject.SetActive(false);
        }
        if (restartButton != null)
        {
            restartButton.gameObject.SetActive(false);
        }

    }

    private void OnDestroy()
    {
        if (returnToLevelSelectButton != null)
            returnToLevelSelectButton.onClick.RemoveListener(ReturnToLevelSelect);
        if (returnToLevelSelectButton2 != null)
        {
            returnToLevelSelectButton2.onClick.RemoveListener(ReturnToLevelSelect);
        }
    }
    
    private void OnEnable()
    {
        if (turnCounter == null) 
            turnCounter = FindAnyObjectByType<TurnCounter>();
    }
    
    public void ShowInVictory()
    {
        VictoryLevelPanel.SetActive(true);

        if (turnCounter == null)
            turnCounter = FindAnyObjectByType<TurnCounter>();

        if (turnCounter == null)
        {
            Debug.LogError("TurnCounter was not found when showing the victory panel.", this);
            return;
        }

        turns = turnCounter.Turns;
        uiScoreText.text = $"Congratulations! You won in {turns} turns";

        if (returnToLevelSelectButton != null)
            returnToLevelSelectButton.gameObject.SetActive(true);
        if (nextLevelButton != null)
        {
            nextLevelButton.gameObject.SetActive(true);
        }
        if (restartButton != null)
        {
            restartButton.gameObject.SetActive(true);
        }

    }

    public void ShowInLosing()
    {
        LostlevelPanel.SetActive(true);

        if (turnCounter == null)
            turnCounter = FindAnyObjectByType<TurnCounter>();

        if (turnCounter == null)
        {
            Debug.LogError("TurnCounter was not found when showing the victory panel.", this);
            return;
        }

        turns = turnCounter.Turns;
        uiScoreText.text = $"Sorry you lost in {turns} rounds";

        if (returnToLevelSelectButton2 != null)
            returnToLevelSelectButton2.gameObject.SetActive(true);

        if (restartButton2 != null)
        {
            restartButton2.gameObject.SetActive(true);
        }

    }

    public void ReturnToLevelSelect()
    {
        Time.timeScale = 1f;
        PlayerPrefs.SetInt("OpenLevelSelect", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene(levelSelectSceneName);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Carrega a próxima cena do Build Settings
    // Se era a última, volta para sceneAfterLastLevel.
    public void LoadNextLevel()
    {
        Time.timeScale = 1f;

        int next = SceneManager.GetActiveScene().buildIndex + 1;
        if (next >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log("[Game] Última fase concluída.");
            next = sceneAfterLastLevel;
        }

        SceneManager.LoadScene(next);
    }
}
