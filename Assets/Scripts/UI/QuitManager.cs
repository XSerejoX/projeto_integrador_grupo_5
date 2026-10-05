using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Button botaoSair;

    void Start()
    {
        botaoSair.onClick.AddListener(SairDoJogo);
    }

    public void SairDoJogo()
    {
        Debug.Log("Saindo do jogo...");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}