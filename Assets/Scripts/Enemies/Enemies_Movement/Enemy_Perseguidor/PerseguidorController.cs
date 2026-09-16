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
    
    public IState perseguindoState; //Istate
    public IState perseguidorIdleState; //Istate

    public Perseguindo perseguindo; // tipo da classe
    public IdlePerseguidor idlePerseguidor; //tipo da classe

    void Awake()
    {
        playerController = FindFirstObjectByType<PlayerController>(); //conectando com player controller
        stateMachine = GetComponent<MoveStateMachine>(); //state machine dos inimigos
        turnManager = playerController.turnManager; //usa o turno compartilhado com o jogador
        
        perseguindo = new Perseguindo(this);// Instanciando o estado de perseguição
        idlePerseguidor = new IdlePerseguidor(this);// Instanciando o estado idle

        perseguindoState = perseguindo; // tipo Istate
        perseguidorIdleState = idlePerseguidor; // tipo Istate
    
    }

    void Start()
    {
        // Iniciando o estado inicial do inimigo
        stateMachine.ChangeState(perseguidorIdleState);
    }

    void Update()
    {
        stateMachine.Update();
    }

}
