using UnityEngine;

public class ShowAndHide : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void showObject()
    {
        gameObject.SetActive(true);
        Debug.LogError("erro1", this);
    }

    public void hideObject()
    {
        gameObject.SetActive(false);
        Debug.LogError("erro2", this);
    }
}
