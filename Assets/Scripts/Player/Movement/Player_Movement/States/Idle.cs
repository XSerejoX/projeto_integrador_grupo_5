using UnityEngine;

public class Idle : IState
{
    public PlayerController playerController;
    public bool playerTurn; 
    public Idle(PlayerController playerController)
    {
        this.playerController = playerController;
    }



    public void Enter()
    {
        playerController.turnManager.isPlayerTurn = true; //recebe o estado de turno do player
        
    }

    public void Update()
    {
            //lidando com as diagonais. caso o jogador aperte uma diagonal, ele vai se mover na horizontal
        if (playerController.moveAction.WasPressedThisFrame() && playerController.turnManager.isPlayerTurn)
        {
            if (playerController.directionVector.x > 0 && playerController.directionVector.y > 0)
            {
                playerController.directionVector = new Vector2(1, 0);
                playerController.stateMachine.ChangeState(playerController.moveState);
            
            }

            else if (playerController.directionVector.x < 0 && playerController.directionVector.y < 0)
            {
                playerController.directionVector = new Vector2(-1, 0);
                playerController.stateMachine.ChangeState(playerController.moveState);
            }

           else if (playerController.directionVector.x > 0 && playerController.directionVector.y < 0)
            {
                playerController.directionVector = new Vector2(1, 0);
                playerController.stateMachine.ChangeState(playerController.moveState);
            }

            else if (playerController.directionVector.x < 0 && playerController.directionVector.y > 0)
            {
                playerController.directionVector = new Vector2(-1, 0);
                playerController.stateMachine.ChangeState(playerController.moveState);
            }
           
            else
            {
                playerController.stateMachine.ChangeState(playerController.moveState);
            }  
        }
        else {return ;}
    }

    public void Exit()
    {
        
    }


}
