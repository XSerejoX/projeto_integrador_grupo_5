using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour

    
{

    public Animator transition;
    public float transitionTime = 1f;

    public void OpenLevel(int levelId)
    {
        string levelName = "Fase" + levelId;
        StartCoroutine(LoadLevel(levelName));
    }
  
    IEnumerator LoadLevel(string levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
