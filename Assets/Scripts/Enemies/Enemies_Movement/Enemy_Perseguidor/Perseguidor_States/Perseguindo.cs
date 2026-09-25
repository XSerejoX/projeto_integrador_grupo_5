using UnityEngine;

public class Perseguindo : IState
{
    
    // difinindo variaveis

    public float playerPositionX;
    public float playerPositionY;
    
    public float enemyPositionX;
    public float enemyPositionY;
    
    public float targetPositionX;
    public float targetPositionY;

    public float distanceX;
    public float distanceY;

    public float newEnemyPositionX;
    public float newEnemyPositionY;
    public Vector2 enemyPosition;
    
    public float tileIncrement = 1f;
    
    public float distanceCorrection;
    
    public float axis; 
    private Vector2 targetPosition;
   
    public PerseguidorController perseguidorController;

    public Perseguindo(PerseguidorController controller)
    {
        perseguidorController = controller;
    }

    public void Enter()
    {
        playerPositionX = perseguidorController.playerController.transform.position.x;
        playerPositionY = perseguidorController.playerController.transform.position.y;
        enemyPositionX = perseguidorController.transform.position.x;
        enemyPositionY = perseguidorController.transform.position.y;
        axis = ChooseAxis();

        if (axis == 0)
        {
            targetPosition = new Vector2(
                Mathf.MoveTowards(enemyPositionX, playerPositionX, perseguidorController.tilesToMove),
                enemyPositionY);
        }
        else
        {
            targetPosition = new Vector2(
                enemyPositionX,
                Mathf.MoveTowards(enemyPositionY, playerPositionY, perseguidorController.tilesToMove));
        }

    }

    public void Update()
    {
        //lógica de perseguição, movendo o inimigo em direção ao jogador
       
        if (!perseguidorController.turnManager.isPlayerTurn)
        {
            Vector2 currentPosition = perseguidorController.transform.position;
            Vector2 newPosition = Vector2.MoveTowards(
                currentPosition,
                targetPosition,
                perseguidorController.speed * Time.deltaTime);

            perseguidorController.transform.position = newPosition;

            if (Vector2.Distance(newPosition, targetPosition) <= 0.01f)
            {
                perseguidorController.transform.position = targetPosition; //snap a position
                perseguidorController.turnManager.isPlayerTurn = true;
                perseguidorController.stateMachine.ChangeState(perseguidorController.perseguidorIdleState);
            }
        }

    }


    public void Exit()
    {
        // Código a ser executado quando o inimigo sai do estado de perseguição
       
    }

        //Código para escolher aleatoriamente o eixo a ser movimentado pelo inimigo
        // eixo x = 0 eixo y = 1
    public int ChooseAxis()
    {
        if (Mathf.Approximately(enemyPositionX, playerPositionX))
            return 1;

        if (Mathf.Approximately(enemyPositionY, playerPositionY))
            return 0;

        int axis = UnityEngine.Random.Range(0, 2);

        return axis;
    }



}
