using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameEndState { None, GameOver, Victory }
    public GameEndState EndState { get; private set; } = GameEndState.None;
    public bool IsGameOver => EndState != GameEndState.None;

    [SerializeField] private float dramaticDelay = 3f;

    [Header("Progressão de fases")]
    [Tooltip("Cena carregada após vencer a última fase do Build Settings (0 = primeira cena, ex.: menu)")]
    [SerializeField] private int sceneAfterLastLevel = 0;

    private bool endSequenceStarted;

    void Awake()
    {
        Instance = this;
        EndState = GameEndState.None;
        endSequenceStarted = false;
        Time.timeScale = 1f;
    }

    void Start()
    {
        var collectibles = FindObjectsByType<Collectible>(FindObjectsInactive.Exclude);
        DeliveryManager.Instance.SetPizzaCollectionGoal(collectibles);
    }

    public void TriggerGameOver()
    {
        if (endSequenceStarted) return;
        endSequenceStarted = true;
        StartCoroutine(EndSequence(GameEndState.GameOver));
    }

    public void TriggerVictory()
    {
        if (endSequenceStarted) return;
        endSequenceStarted = true;
        AudioManager.Instance.PlaySFX(AudioManager.Instance.vitoria);
        StartCoroutine(EndSequence(GameEndState.Victory));
    }

    private IEnumerator EndSequence(GameEndState result)
    {
        Debug.Log(result == GameEndState.GameOver ? "[Game] Derrota..." : "[Game] Vitória!");
        EndState = result;
        yield return new WaitForSeconds(dramaticDelay);

        // derrota: refaz a fase. vitória: vai para a próxima.
        if (result == GameEndState.Victory)
            LoadNextLevel();
        else
            Restart();
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
