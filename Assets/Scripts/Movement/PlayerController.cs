using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(MoveStateMachine))]

public class PlayerController : MonoBehaviour
{
    //definindo variaveis
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
    
    
    void Awake()
    {
        stateMachine = GetComponent<MoveStateMachine>();   
        
        inputActions = new InputActions();
        
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

}
