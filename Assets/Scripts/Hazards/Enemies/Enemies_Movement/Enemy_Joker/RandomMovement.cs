using UnityEngine;
using System.Collections;

public class RandomMovement : EnemyHazard
{
    [SerializeField] private NewPlayerController player;
    [SerializeField] private TurnManager turnManager;
    [SerializeField] private Animator animator;

    private static readonly int JokerDirX = Animator.StringToHash("JokerDirX");
    private static readonly int JokerDirY = Animator.StringToHash("JokerDirY");
    private static readonly int IsJokerMoving = Animator.StringToHash("IsJokerMoving");

    private bool isTakingTurn;
    private bool hasPendingTurn;

    protected override void OnMoveStart(Vector2Int dir)
    {
        if (animator == null) return;
        animator.SetFloat(JokerDirX, dir.x);
        animator.SetFloat(JokerDirY, dir.y);
        animator.SetBool(IsJokerMoving, true);
    }

    protected override void OnMoveEnd()
    {
        if (animator == null) return;
        animator.SetBool(IsJokerMoving, false);
    }

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
                //espera a animção do player acabar
            while (player.IsAnimating) yield return null; 
            yield return TakeTurn(player);
        }

        isTakingTurn = false;
        // devolve o turno pro player, senão o Update dele nunca mais aceita input
        OnMoveEnd();
        turnManager.isPlayerTurn = true;
    }

    public IEnumerator TakeTurn(GridEntity player)
    {
        Vector2Int[] directions = new Vector2Int[]
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };

        
        Vector2Int dir = directions[Random.Range(0, directions.Length)];
        
        Vector2Int target = Cell + dir;

        if (target == player.Cell) // chegou do lado do player: ataca + knockback
        {
            PlayerHealth health = player.GetComponent<PlayerHealth>();
            if (health != null)
                health.TakeDamage(dir, this);

            TryKnockback(player, dir);
        }
        else
        {
                if (!TryMove(dir)) // se não conseguir mover
                     TryMove(-dir); // tenta mover na direção oposta
        }
        
        while (IsAnimating || player.IsAnimating) yield return null;
    }

    private void TryKnockback(GridEntity player, Vector2Int dir)
    {
        // Se o player tem espaço atrás, é empurrado e o inimigo ocupa a célula dele.
        // Se tem parede atrás, nada acontece (o inimigo fica onde está).
        if (player.TryMove(dir))
            TryMove(dir); // esse dir é a direção do inimigo
    }

}
