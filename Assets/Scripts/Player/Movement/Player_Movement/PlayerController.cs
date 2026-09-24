using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(MoveStateMachine))]
[RequireComponent(typeof(TurnManager))]
[RequireComponent(typeof(CollisionHandler))]

public class PlayerController : MonoBehaviour
{
    //definindo variaveis
    
    public float speed = 5f; 
    public float tilesToMove = 1f;
    public Vector2 directionVector;


        //dash (shift + WASD, com cargas limitadas)
    public float dashMultiplier = 2f; // dash = 2x o tile do move
    public int maxDashCharges = 3; // quantos dashes o jogador tem por partida
    public int currentDashCharges { get; private set; }
    public float speedMultiplier = 1.8f; // fazer o dash de forma rapida
    //referencias:
        
        //states
    public IState moveState;
    public IState idleState;
        
        //input
    public InputAction moveAction;
    public InputAction dashAction;
    public InputActions inputActions;
        
        //state machine
    public MoveStateMachine stateMachine;
        
        //turn manager
    public TurnManager turnManager;
    
        //collision handler
    public Transform rayOrigin;
    public CollisionHandler collisionHandler;
    public RaycastHit2D wasRaycastHit;

    void Awake()
    {
        
        stateMachine = GetComponent<MoveStateMachine>();   
        
        inputActions = new InputActions();
        
        turnManager = GetComponent<TurnManager>();
        
        collisionHandler = GetComponent<CollisionHandler>();

        //instanciando estados de movimento
        moveState = new Move(this);
        idleState = new Idle(this);

        moveAction = inputActions.Player.Move; //especificando a ação de movimento do input actions
        dashAction = inputActions.Player.Dash; //ação de dash (Shift), configurada no Input Actions asset

        currentDashCharges = maxDashCharges; //cargas cheias no início da partida

    }

    // ligando e desligando o input
    public void OnEnable()
    {
        moveAction.Enable();
        dashAction.Enable();
    }

    public void OnDisable()
    {
        moveAction.Disable();
        dashAction.Disable();
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

        public bool IsDashAvailable()
        {
            return dashAction.IsPressed() && currentDashCharges > 0;
        }

        public void ConsumeDash()
        {
            currentDashCharges--;
        }

    
    
 

        // metodo que calcula a proxima posição dp player (multiplier = 1 é move normal, dashMultiplier é dash)
    public Vector2 CalculateTargetPosition(float multiplier = 1f)
    {
        return (Vector2)transform.position
            + directionVector * tilesToMove * multiplier;
    }


}