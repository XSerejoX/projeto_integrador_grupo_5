using UnityEngine;
using UnityEngine.EventSystems;

public class StarManager : MonoBehaviour
{
    [SerializeField] private GameObject starReader;

    private void OnMouseEnter()
    {
        starReader.GetComponent<ShowAndHide>().showObject();
        Debug.LogError("erro3", this);
    }

    private void OnMouseExit()
    {
        starReader.GetComponent<ShowAndHide>().hideObject();
        Debug.LogError("erro4", this);
    }
}
