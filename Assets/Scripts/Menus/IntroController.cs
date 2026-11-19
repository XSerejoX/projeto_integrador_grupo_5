using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroController : MonoBehaviour
{
    public void IntroStarter(string homeScene)
    {
        SceneManager.LoadScene(homeScene);
    }
}
