using UnityEngine;
using UnityEngine.InputSystem;
public class Move : IState
{
        //definindo variaveis 
    public Vector2 currentPosition;
    public Vector2 targetPosition;
    public float distanceToTarget;
    private bool pathIsClear;
    private float movementSpeedMultiplier;
    public PlayerController playerController;

    public Move(PlayerController playerController)
    {
        this.playerController = playerController;
    }
    
    public void Enter()
    {
        currentPosition = playerController.transform.position;

        bool dashRequested = playerController.IsDashAvailable();
        
            // se dash estiver disponivel (true), dash multiplier será o valor padrão, senão, será 1f  
        float distanceMultiplier = dashRequested ? playerController.dashMultiplier : 1f; 

            //considera distancia do dash
        targetPosition = playerController.CalculateTargetPosition(distanceMultiplier);
        

        float distance = Vector2.Distance(currentPosition, targetPosition);
        
            //cast origin é a origem do ray, que começa a partir de um empty centralizado
        Vector2 castOrigin = playerController.rayOrigin != null
            ? (Vector2)playerController.rayOrigin.position
            : currentPosition;

        pathIsClear = playerController.collisionHandler.CanMoveTo(castOrigin, playerController.directionVector, distance, targetPosition);

        bool isDashing = dashRequested && pathIsClear;
        if (isDashing)
        {
            playerController.ConsumeDash();
        }

        movementSpeedMultiplier = isDashing ? playerController.speedMultiplier : 1f;
    }


    public void Update()
    {
         if (!pathIsClear) //checando se o jogador está dentro do mapa e se o caminho não está bloqueado
        {
            playerController.stateMachine.ChangeState(playerController.idleState); // caso não esteja, ir para idle (turno continua ativo)
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
        playerController.turnManager.EndPlayerTurn(); // desliga o turno do jogador 
        playerController.stateMachine.ChangeState(playerController.idleState); // muda pra idle
    }

    private void MovePlayer() // movimenta o player
    {
        playerController.transform.position = Vector2.MoveTowards(
            playerController.transform.position,
            targetPosition,
            playerController.speed * movementSpeedMultiplier * Time.deltaTime);
    }

    private bool HasReachedTarget() // checa se player chegou ao alvo
    {
        return Vector2.Distance(
            playerController.transform.position,
            targetPosition) <= 0.01f;
    }   


}
