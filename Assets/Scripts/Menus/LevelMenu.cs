using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LevelMenu : MonoBehaviour
{

    public void OpenLevel(int levelId)
    {
        string levelName = "Fase" + levelId;
        SceneManager.LoadScene(levelName);
    }

}
