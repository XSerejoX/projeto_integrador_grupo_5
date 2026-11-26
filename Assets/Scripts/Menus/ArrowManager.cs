using UnityEngine;
using UnityEngine.InputSystem;

public class ArrowManager : MonoBehaviour
{

    [SerializeField] private GameObject Arrow;
    void Update()
    {

        if (Keyboard.current !=null && Keyboard.current.qKey.wasPressedThisFrame)
        {

                Arrow.SetActive(true);
        }

        if (Keyboard.current != null && Keyboard.current.qKey.wasReleasedThisFrame)
        {

            Arrow.SetActive(false);
        }

    }
}
