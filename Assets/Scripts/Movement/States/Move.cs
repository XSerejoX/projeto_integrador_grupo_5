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
   
    public Move(PlayerController playerController)
    {
        this.playerController = playerController;
    }

    public void Enter()
    {
        
        if (directionVector.y == 1)
        {
            directionVector.x = 0;
        }
        else if (directionVector.x == 1)
        {
            directionVector.y = 0;
        }
        else if (directionVector.y == -1)
        {
            directionVector.x = 0;
        }
        else if (directionVector.x == -1)
        {
            directionVector.y = 0;    
        }
        
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
        }
    }

    public void Exit()
    {
        Debug.Log("Saiu do estado de movimento");
    }


}
