using UnityEngine;
using UnityEngine.InputSystem;

public class OldPlayerController : MonoBehaviour
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
        
        //input
    public InputAction moveAction;
    public InputAction dashAction;
    private InputActions inputActions;
        

        
        //turn manager
    public TurnManager turnManager;
    
        //collision handler
    public Transform rayOrigin;
    
    void Awake()
    {
    
        
        inputActions = new InputActions();
        
        turnManager = GetComponent<TurnManager>();
        

        //removi instancians dos estados de movimento
        
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

    }
    
    void Update()
    {
        directionVector = moveAction.ReadValue<Vector2>();
        
    }

    
        public bool IsDashAvailable()
        {
            return dashAction.IsPressed() && currentDashCharges > 0;
        }

        public void ConsumeDash()
        {
            currentDashCharges--;
        }    


}