using UnityEngine;

[RequireComponent(typeof(FornoStateMachine))]
public class Forno : MonoBehaviour
{
        //states
    public IState idleState;
    public IState bakingState;

        //referencias
    public FornoStateMachine stateMachine;
    public DeliveryManager deliveryManager; 
    public TurnManager turnManager; 

    void Awake()
    {
        stateMachine = GetComponent<FornoStateMachine>();

        idleState = new FornoIdle(this);
        bakingState = new Baking(this);

        if (deliveryManager == null)
            Debug.LogError("[Forno] DeliveryManager nao foi atribuido no Inspector.", this);
        if (turnManager == null)
            Debug.LogError("[Forno] TurnManager nao foi atribuido no Inspector.", this);
    }

    void Start()
    {
        stateMachine.ChangeState(idleState);
        Debug.Log("[Forno] Inicializado e aguardando uma pizza crua.", this);
    }

    void Update()
    {
        stateMachine.Update();
    }
}
