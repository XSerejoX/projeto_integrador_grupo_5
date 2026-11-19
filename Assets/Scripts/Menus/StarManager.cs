using UnityEngine;
using UnityEngine.EventSystems;

public class StarManager : MonoBehaviour
{
    [SerializeField] private GameObject starReader;

    private void OnMouseEnter()
    {
        starReader.GetComponent<ShowAndHide>().showObject();
    }

    private void OnMouseExit()
    {
        starReader.GetComponent<ShowAndHide>().hideObject();
    }
}
