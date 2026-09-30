using UnityEngine;
using System;

public class TurnManager : MonoBehaviour
{
    public bool isPlayerTurn = true;
    public bool isPlayerPathFree = true;

    public event Action OnPlayerTurnEnded;

    public void EndPlayerTurn()
    {
        if (!isPlayerTurn) return;

        isPlayerTurn = false;
        OnPlayerTurnEnded?.Invoke();
    }
}
