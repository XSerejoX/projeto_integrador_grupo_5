using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    private const string OpenLevelSelectKey = "OpenLevelSelect";

    public Button[] buttons;
    public GameObject levelButtons;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject levelPanel;
    [SerializeField] private LevelLoader levelLoader;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void RegisterSceneLoadedHandler()
    {
        SceneManager.sceneLoaded -= HandleSceneLoaded;
        SceneManager.sceneLoaded += HandleSceneLoaded;
    }

    private static void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (PlayerPrefs.GetInt(OpenLevelSelectKey, 0) != 1)
            return;

        foreach (LevelMenu levelMenu in Resources.FindObjectsOfTypeAll<LevelMenu>())
        {
            if (levelMenu.gameObject.scene != scene)
                continue;

            levelMenu.OpenLevelSelect();
            return;
        }

        Debug.LogError($"Could not find a LevelMenu in scene '{scene.name}'.");
    }

    private void Start()
    {
        ButtonsToArray();
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }
        for (int i = 0; i < unlockedLevel; i++)
        {
            buttons[i].interactable = true;
        }
    }

    private void OpenLevelSelect()
    {
        if (mainMenu == null || levelPanel == null)
        {
            Debug.LogError("Assign Main Menu and Level Panel on the LevelMenu component.", this);
            return;
        }

        mainMenu.SetActive(false);
        levelPanel.SetActive(true);
        PlayerPrefs.DeleteKey(OpenLevelSelectKey);
        PlayerPrefs.Save();
    }

    public void Load(int levelid)
    {
        levelLoader.OpenLevel(levelid);
    }

    void ButtonsToArray()
    {
        int childCount = levelButtons.transform.childCount;
        buttons = new Button[childCount];
        for (int i = 0; i < childCount; i++)
        {
            buttons[i] = levelButtons.transform.GetChild(i).gameObject.GetComponent<Button>();
        }
    }

}
