using System.Collections;
using UnityEngine;

public class PerseguidorController : GridEntity
{
    
    [SerializeField] private NewPlayerController player;
    [SerializeField] private TurnManager turnManager;
    [SerializeField] private Animator animator;
    
    private bool isTakingTurn;
    private bool hasPendingTurn;

    public bool isContactWithEnemy = false; // se encostar no player
   
    private static readonly int PersDirX = Animator.StringToHash("PersDirX");
    private static readonly int PersDirY = Animator.StringToHash("PersDirY");
    private static readonly int IsPersMoving = Animator.StringToHash("IsPersMoving");

    private void OnEnable()
    {
        if (player == null)
            player = FindAnyObjectByType<NewPlayerController>();

        if (turnManager == null)
            turnManager = FindAnyObjectByType<TurnManager>();

        if (player != null)
            player.Moved += HandlePlayerMoved;
    }

    private void OnDisable()
    {
        if (player != null)
            player.Moved -= HandlePlayerMoved;
    }

    private void HandlePlayerMoved()
    {
        hasPendingTurn = true;
        if (!isTakingTurn)
            StartCoroutine(ProcessTurns());
    }

    private IEnumerator ProcessTurns()
    {
        isTakingTurn = true;
        
        while (hasPendingTurn && player != null)
        {
            hasPendingTurn = false;
            while (player.IsAnimating) yield return null;
            yield return TakeTurn(player);
        }
        isTakingTurn = false;
        // devolve o turno pro player, senão o Update dele nunca mais aceita input
        OnMoveEnd();
        turnManager.isPlayerTurn = true;

    }

    public IEnumerator TakeTurn(GridEntity player) // exemplo: player x2 - inimigo x4 = inimigo -x2
    {
        Vector2Int diff = player.Cell - Cell;
        
            //math.Sign extrai o apenas o sinal 
        Vector2Int stepX = new Vector2Int(System.Math.Sign(diff.x), 0); 
        Vector2Int stepY = new Vector2Int(0, System.Math.Sign(diff.y));

        // Ordem das tentativas: eixo sorteado primeiro, o outro como fallback
        bool xFirst = Random.value < 0.5f;
        Vector2Int first  = xFirst ? stepX : stepY; // primeiro sorteio escolhe entre x e y
        Vector2Int second = xFirst ? stepY : stepX; 
            // se precisar de fall back escolhe o outro eixo que aproxima mais do player

        foreach (var dir in new[] { first, second }) //cria um array e implementa a direction
        {

            if (dir == Vector2Int.zero) continue;

            Vector2Int target = Cell + dir; // target do peseguidor

            if (target == player.Cell) //knock back se chegar no player
            {
                isContactWithEnemy = true; // player pode tomar dano
                
                PlayerHealth health = player.GetComponent<PlayerHealth>();
                if (health != null)
                health.TakeDamage(dir);//aciona o TakeDamage do PlayerHealth
                
                TryKnockback(player, dir); 
                break;
            }

            if (TryMove(dir)) break; // andou; senão tenta o próximo eixo            

        
        }

        // Espera as animações acabarem antes de liberar o turno
        while (IsAnimating || player.IsAnimating) yield return null;
    }

    private void TryKnockback(GridEntity player, Vector2Int dir)
    {
        // Se o player tem espaço atrás, é empurrado e o perseguidor ocupa a célula dele.
        // Se tem parede atrás, nada acontece (o perseguidor fica onde está).
        if (player.TryMove(dir))
            TryMove(dir); // esse dir é a direção do perseguidor
    }

        //metodos virtuais que foram sobrescritos
        //lida com a animação do perseguidor, setando os parametros no animator
      protected override void OnMoveStart(Vector2Int dir)
    {
        if (animator == null) return;
        animator.SetFloat(PersDirX, dir.x);
        animator.SetFloat(PersDirY, dir.y);
        animator.SetBool(IsPersMoving, true);
    }

    protected override void OnMoveEnd()
    {
        if (animator == null) return;
        animator.SetBool(IsPersMoving, false);
    }
}