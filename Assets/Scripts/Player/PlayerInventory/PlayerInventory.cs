using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PlayerInventory : MonoBehaviour
{
    public int CoinsAquired {get => _coinsAquired; set => _coinsAquired = value;}
    public int PizzasAquired{get => _pizzasAquired; set => _pizzasAquired = value;}
    public int BakedPizzasAquired{get => _bakedPizzasAquired; set => _bakedPizzasAquired = value;}
    public int _coinsAquired = 0;
    public int _pizzasAquired = 0;   
    public int _bakedPizzasAquired = 0;


}   
