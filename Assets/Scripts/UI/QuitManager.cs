using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void SairFase()
    {
        SceneManager.LoadSceneAsync(0);
    }
}