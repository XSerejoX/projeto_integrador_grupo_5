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

    private bool endSequenceStarted;

    void Awake()
    {
        Instance = this;
        EndState = GameEndState.None;
        endSequenceStarted = false;
        Time.timeScale = 1f;
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
        StartCoroutine(EndSequence(GameEndState.Victory));
    }

    private IEnumerator EndSequence(GameEndState result)
    {
        Debug.Log(result == GameEndState.GameOver ? "[Game] Derrota..." : "[Game] Vitória!");

        // Jogo continua rodando
        yield return new WaitForSeconds(dramaticDelay);

        EndState = result;
        if (result == GameEndState.GameOver)
        {
            Restart();
            yield break;
        }

        Time.timeScale = 0f;
    
        if (result == GameEndState.Victory)
        {
            Restart();
            yield break;
        }

        Time.timeScale = 0f;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    

}