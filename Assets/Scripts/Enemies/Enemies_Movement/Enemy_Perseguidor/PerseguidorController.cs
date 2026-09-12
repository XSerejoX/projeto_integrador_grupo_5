using UnityEngine;
[RequireComponent(typeof(MoveStateMachine))]
public class PerseguidorController : MonoBehaviour
{
    //definindo variaveis
    public float speed = 3f;
    public float tilesToMove = 1f;
   
    public TurnManager turnManager;
    public PlayerController playerController;
    public MoveStateMachine stateMachine;
    
    public IState perseguindoState;
    public IState perseguidorIdleState;

    public Perseguindo perseguindo;
    public IdlePerseguidor idlePerseguidor;

    void Awake()
    {
        playerController = FindFirstObjectByType<PlayerController>(); //conectando com player controller
        stateMachine = GetComponent<MoveStateMachine>(); //state machine dos inimigos
        turnManager = playerController.turnManager; //usa o turno compartilhado com o jogador
        
        perseguindo = new Perseguindo(this);// Instanciando o estado de perseguição
        idlePerseguidor = new IdlePerseguidor(this);// Instanciando o estado idle

        perseguindoState = perseguindo;
        perseguidorIdleState = idlePerseguidor;

    
    }

    void Start()
    {
        // Iniciando o estado inicial do inimigo
        stateMachine.ChangeState(perseguidorIdleState);
    }

}
