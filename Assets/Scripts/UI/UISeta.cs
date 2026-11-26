using UnityEngine;

public class UISeta : MonoBehaviour
{
    
    [SerializeField] private RectTransform rectTransform;
   
    public Vector2 posicaoSeta;
    public Vector2 resetPosicaoSeta;
    public Vector2 offset = new Vector2(0f, -40f);    
    
    void Awake()
    {

        posicaoSeta = new Vector2(rectTransform.anchoredPosition.x,
    rectTransform.anchoredPosition.y);
        resetPosicaoSeta = posicaoSeta;
    }
    
    public void MoverSeta()
    {
        posicaoSeta += offset;
        rectTransform.anchoredPosition = posicaoSeta;          
    }
    public void ResetSeta()
    {
        posicaoSeta = resetPosicaoSeta;
        rectTransform.anchoredPosition = posicaoSeta;
    } 



}
