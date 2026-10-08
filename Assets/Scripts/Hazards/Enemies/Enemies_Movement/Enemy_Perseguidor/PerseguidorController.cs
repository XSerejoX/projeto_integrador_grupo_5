using System.Collections;
using System.Collections.Generic;
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

    public IEnumerator TakeTurn(GridEntity player)
    {
        if (FindNextStep(player.Cell, out Vector2Int dir))
        {
            Vector2Int target = Cell + dir;

            if (target == player.Cell) // chegou do lado do player: ataca + knockback
            {
                isContactWithEnemy = true;

                PlayerHealth health = player.GetComponent<PlayerHealth>();
                if (health != null)
                    health.TakeDamage(dir);

                TryKnockback(player, dir);
            }
            else
            {
                TryMove(dir);
            }
        }
        // se não há caminho, ele simplesmente espera o próximo turno

        while (IsAnimating || player.IsAnimating) yield return null;
    }

    private void TryKnockback(GridEntity player, Vector2Int dir)
    {
        // Se o player tem espaço atrás, é empurrado e o perseguidor ocupa a célula dele.
        // Se tem parede atrás, nada acontece (o perseguidor fica onde está).
        if (player.TryMove(dir))
            TryMove(dir); // esse dir é a direção do perseguidor
    }

    // métodos virtuais que foram sobrescritos
    // lida com a animação do perseguidor, setando os parâmetros no animator
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

    //mecanismo de busca:

    private static readonly Vector2Int[] Directions =
    {
        Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
    };

    // BFS: devolve a direção do PRIMEIRO passo do menor caminho até o goal.
    // Retorna false se não existe caminho.
    private bool FindNextStep(Vector2Int goal, out Vector2Int firstStep)
    {
        firstStep = Vector2Int.zero;
        Vector2Int start = Cell;
        if (start == goal) return false;

        var cameFrom = new Dictionary<Vector2Int, Vector2Int>(); // célula -> de onde veio
        var queue = new Queue<Vector2Int>();

        cameFrom[start] = start;
        queue.Enqueue(start);

        // começa a olhar as direções em ordem aleatória, pra variar entre
        // caminhos de mesmo tamanho 
        int offset = Random.Range(0, Directions.Length);

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();
            if (current == goal) break;

            for (int i = 0; i < Directions.Length; i++)
            {
                Vector2Int next = current + Directions[(i + offset) % Directions.Length];

                if (cameFrom.ContainsKey(next)) continue;

                // a célula do player está "ocupada", mas é o destino, então vale
                bool isGoal = next == goal;
                if (!isGoal && !GridManager.Instance.IsFree(next)) continue;

                cameFrom[next] = current;
                queue.Enqueue(next);
            }
        }

        if (!cameFrom.ContainsKey(goal)) return false; // player inalcançável

        // volta do goal até o start pra descobrir o primeiro passo
        Vector2Int step = goal;
        while (cameFrom[step] != start)
            step = cameFrom[step];

        firstStep = step - start;
        return true;
    }
}