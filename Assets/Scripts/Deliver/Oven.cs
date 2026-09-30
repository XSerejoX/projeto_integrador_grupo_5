using UnityEngine;

public class Oven : GridEntity
{
    
        //maquina de estados simples para o forno
    private enum State { Idle, Baking, Ready }

    [SerializeField] private int bakeDuration = 3; // turnos totais pra assar
    [SerializeField] private NewPlayerController player;

    private State current = State.Idle;
    private int turnsRemaining; // turnos que faltam pra assar

    private void OnEnable()
    {
        //referenciando o script do player no forno
        if (player == null) 
            player = FindAnyObjectByType<NewPlayerController>();

        if (player != null)
        {
            //inscrevendo player em eventos
            player.Moved += Tick; //para alertar quando ele se moveu
            player.Moved += TryInteract;
        }
    }

    private void OnDisable()
    {
        //desinscrevendo quando forno for desativado
        if (player != null)
        {
            player.Moved -= Tick;
            player.Moved -= TryInteract;
        }
    }

    private void Tick()
    {
        if (current != State.Baking) return; //so funciona se tiver baking

        turnsRemaining--; 
        Debug.Log($"[Delivery] Forno em {Cell} assando... faltam {turnsRemaining} movimentos.");

        if (turnsRemaining <= 0) //quando termina o periodo o forno fica pronto
            Debug.Log($"[Delivery] Forno em {Cell}: pizza PRONTA! Pode retirar.");
            current = State.Ready;
    }

    // Chamado automaticamente toda vez que o player termina de mover
    private void TryInteract()
    {
        if (!IsAdjacent(player.Cell)) return; 
        
        // se player está adjacente:

        if (current == State.Idle) //forno ocioso
        {
            if (player.Inventory.TryConsumeRawPizza())
            {
                current = State.Baking; //põe forno pra assar
                turnsRemaining = bakeDuration; //setta o periodo de assamento
            }
        }
        else if (current == State.Ready) //caso teja perto e o forno teja pronto:
        {
            player.Inventory.AddBakedPizza();
            DeliveryManager.Instance.NotifyBakedPizzaAcquired();
            
            Debug.Log($"[Delivery] Pizza assada retirada do forno em {Cell}. Total assado no inventário: {player.Inventory.BakedPizzaCount}");
            current = State.Idle; //volta pra ocioso/idle
        }
    }

    private bool IsAdjacent(Vector2Int otherCell)
    {
        //se a distancia da celula x/y do player com a atual for 1: true

        Vector2Int diff = otherCell - Cell;
        return Mathf.Abs(diff.x) + Mathf.Abs(diff.y) == 1;
    }
}