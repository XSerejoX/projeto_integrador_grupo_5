using TMPro;
using UnityEngine;


public class CardManager : MonoBehaviour
{
    [SerializeField] private NewPlayerController player;
    [SerializeField] private Animator animCard1;
    [SerializeField] private Animator animCard2;


  
    void Update()
    {
       if(player.DashCharges == 1)
        {
            animCard1.SetTrigger(name: "Gastou");
        }

        if (player.DashCharges == 0)
        {
            animCard2.SetTrigger(name: "Gastou");
        }
    }
}
