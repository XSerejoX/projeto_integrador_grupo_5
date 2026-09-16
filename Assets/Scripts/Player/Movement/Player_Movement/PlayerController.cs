using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(MoveStateMachine))]
[RequireComponent(typeof(TurnManager))]
public class PlayerController : MonoBehaviour
{
    //definindo variaveis
       
        //configura o tamanho do mapa
    public Vector2 minMaxMapScaleX = new Vector2(1f,8f); // largura minima: 1, depois largura máxima: 8
    public Vector2 minMaxMapScaleY = new Vector2(1f,8f); // altura minima: 1, depois altura máxima: 8
    
    public float speed = 5f;
    public float tilesToMove = 1f;
    public Vector2 directionVector;
    //referencias:
        
        //states
    public IState moveState;
    public IState idleState;
        
        //input
    public InputAction moveAction;
    public InputActions inputActions;
        
        //state machine
    public MoveStateMachine stateMachine;
        
        //turn manager
    public TurnManager turnManager;
    void Awake()
    {
        
        stateMachine = GetComponent<MoveStateMachine>();   
        
        inputActions = new InputActions();
        
        turnManager = GetComponent<TurnManager>();
        
        //instanciando estados de movimento
        moveState = new Move(this);
        idleState = new Idle(this);

        moveAction = inputActions.Player.Move; //especificando a ação de movimento do input actions
  
    }

    // ligando e desligando o input
    public void OnEnable()
    {
        moveAction.Enable();
    }

    public void OnDisable()
    {
        moveAction.Disable();
    }

    void Start()
    {
        stateMachine.ChangeState(idleState);
    }
    
    void Update()
    {
        
        directionVector = moveAction.ReadValue<Vector2>();
        stateMachine.Update();
    }

        // metodo para checar se uma posição está dentro do mapa
    public bool IsInsideMap(Vector2 position) 
    {
    return position.x >= minMaxMapScaleX.x // largura minima
        && position.x <= minMaxMapScaleX.y // largura maxima
        && position.y >= minMaxMapScaleY.x // altura minima
        && position.y <= minMaxMapScaleY.y;// altura maxima

    }

        // metodo que calcula a proxima posição dp player
    public Vector2 CalculateTargetPosition()
    {
        return (Vector2)transform.position
            + directionVector * tilesToMove;
    }

}