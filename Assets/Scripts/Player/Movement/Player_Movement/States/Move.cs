using UnityEngine;
using UnityEngine.InputSystem;
public class Move : IState
{
        //definindo variaveis 
    public Vector2 currentPosition;
    public Vector2 targetPosition;
    public float distanceToTarget;
    private bool targetIsInsideMap;

    public PlayerController playerController;

    
    public Move(PlayerController playerController)
    {
        this.playerController = playerController;
    }

    public void Enter()
    {
        currentPosition = playerController.transform.position;
        targetPosition = playerController.CalculateTargetPosition();
        targetIsInsideMap = playerController.IsInsideMap(targetPosition); 

    }


    public void Update()
    {
        if (!targetIsInsideMap) //checando se o jogador está dentro do mapa
        {
            playerController.stateMachine.ChangeState(playerController.idleState); // caso não esteja, ir para idle
            return;
        }

        currentPosition = playerController.transform.position;

        MovePlayer();

        distanceToTarget = Vector2.Distance(playerController.transform.position, targetPosition); //medindo a distancia do player até o alvo
                                                                                                                 
        if (HasReachedTarget()) // assim que o player chega a posição alvo
        {
            FinishMovement();
        }
    
    }
    public void Exit()
    {
        
    }

    private void FinishMovement()
    {
        playerController.transform.position = targetPosition; // snap da posição
        playerController.turnManager.isPlayerTurn = false; // desliga o turno do jogador
        playerController.stateMachine.ChangeState(playerController.idleState); // muda pra idle
    }

    private void MovePlayer() // movimenta o player
    {
        playerController.transform.position = Vector2.MoveTowards(
            playerController.transform.position,
            targetPosition,
            playerController.speed * Time.deltaTime);
    }

    private bool HasReachedTarget() // checa se player chegou ao alvo
    {
        return Vector2.Distance(
            playerController.transform.position,
            targetPosition) <= 0.01f;
    }   


}
