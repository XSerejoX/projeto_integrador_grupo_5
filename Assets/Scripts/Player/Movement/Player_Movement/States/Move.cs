using UnityEngine;
using UnityEngine.InputSystem;
public class Move : IState
{
        //definindo variaveis 
    public Vector2 movementVector;
    public Vector2 directionVector;
    public Vector2 currentPosition;
    public Vector2 targetPosition;
    public float distanceToTarget;
    
    public PlayerController playerController;
    public bool playerTurn;
    
    public bool playerIsMoving;
    
    public Move(PlayerController playerController)
    {
        this.playerController = playerController;
    }

    public void Enter()
    {
        currentPosition = playerController.transform.position;
        targetPosition = currentPosition + playerController.directionVector * playerController.tilesToMove;
    }


    public void Update()
    {
        
        currentPosition = playerController.transform.position;
        
        playerController.transform.position = Vector2.MoveTowards(
           currentPosition,
           targetPosition,
           playerController.speed * Time.deltaTime);

        distanceToTarget = Vector2.Distance(playerController.transform.position, targetPosition);
       
        
        if (distanceToTarget <= 0.01f)
        {
            playerController.transform.position = targetPosition;
            playerController.stateMachine.ChangeState(playerController.idleState);

            playerController.turnManager.isPlayerTurn = false;
        
        }
        

    
    }
    public void Exit()
    {
        
    }


}
