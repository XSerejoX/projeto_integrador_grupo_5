using UnityEngine;
using UnityEngine.EventSystems;

public class StarManager : MonoBehaviour
{
    public GameObject starReader;

    void OnMouseOver()
    {
       
      starReader.SetActive(true);
        
    }

    void OnMouseExit()
    {
            starReader.SetActive(false);
    }

}
