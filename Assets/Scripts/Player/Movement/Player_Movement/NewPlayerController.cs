using System;
using UnityEngine;

public class NewPlayerController : GridEntity
{
    private InputActions inputActions;
    
    [SerializeField] private Animator animator;

    private static readonly int DirX = Animator.StringToHash("DirX");
    private static readonly int DirY = Animator.StringToHash("DirY");
    private static readonly int IsMoving = Animator.StringToHash("IsMoving");

    public TurnManager turnManager;
    public PlayerInventory Inventory;
    public Collectible[] collectibles; 

    public event Action Moved;
    

    [Header("Dash")]
    [SerializeField] private int dashSteps = 2;      // quantas células o dash percorre
    [SerializeField] private int maxDashCharges = 2; // quantidade de dashes disponíveis
    private int dashCharges;

        //metodos virtuais que foram sobrescritos
        //lida com a animação do player, setando os parametros no animator
    protected override void OnMoveStart(Vector2Int dir)
    {
        if (animator == null) return;
        animator.SetFloat(DirX, dir.x);
        animator.SetFloat(DirY, dir.y);
        animator.SetBool(IsMoving, true);
    }

    protected override void OnMoveEnd()
    {
        if (animator == null) return;
        animator.SetBool(IsMoving, false);
    }
    
    protected override void Start()
    {
        base.Start();
        dashCharges = maxDashCharges;
    }

    private void Awake()
    {
        inputActions = new InputActions();
    }

    private void OnEnable()
    {
        inputActions?.Player.Enable(); //ligando as actions do player
    }

    private void OnDisable()
    {
        inputActions?.Player.Disable();
    }

    private void OnDestroy()
    {
        inputActions?.Dispose();
    }

    private void Update()
    {
        //retorna nada se nao for vez do player ou se tiver animando   
        if (!turnManager.isPlayerTurn || IsAnimating) return;

        var moveAction = inputActions.Player.Move; //acao move
        if (!moveAction.WasPressedThisFrame()) return;
            // lendo o input da action move
        Vector2 input = moveAction.ReadValue<Vector2>(); 
            //caso aperte duas teclas de uma vez, priorize a direção dominante
        Vector2Int direction = Mathf.Abs(input.x) > Mathf.Abs(input.y)
            ? new Vector2Int((int)Mathf.Sign(input.x), 0)
            : new Vector2Int(0, (int)Mathf.Sign(input.y));

        if (direction == Vector2Int.zero) return;
            //bool de se apertou dash e ainda tiver carga
        bool dashRequested = inputActions.Player.Dash.IsPressed() && IsDashAvailable();
            //se dashRequest true, lance dash normal, se false, ande apenas 1
        int steps = dashRequested ? dashSteps : 1;

        int moved = 0;
        for (int i = 0; i < steps; i++)
        {
            if (!TryMove(direction)) break; // não moveu - encontrou célula bloqueada
            moved++; // caso a celula esteja livre, moveu
        }

        if (moved == 0) return; // se a celula ta bloqueada: não gasta turno

        Moved?.Invoke(); // dispara Tick() do forno + TryInteract() do forno/bag

        foreach (var c in collectibles) //para cada collectible(pizza,coin)
            if (c != null && c.TryCollect(Inventory, Cell)) break;

        if (dashRequested) ConsumeDash();//se usou dash, diminui carga

        
        turnManager.EndPlayerTurn();
    
    }

    public int DashCharges => dashCharges;
    private bool IsDashAvailable() => dashCharges > 0;

    private void ConsumeDash()
    {
        dashCharges--;
    }

    
    public void RefillDash(int amount)
    {
        dashCharges = Mathf.Min(dashCharges + amount, maxDashCharges);
    }
}