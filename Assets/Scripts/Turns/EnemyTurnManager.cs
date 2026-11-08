using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Resultado do planejamento de um inimigo. Calculado sem mexer no grid.
public struct EnemyPlan
{
    public List<Vector2Int> Path;   // células que ele vai pisar (sem a origem)
    public bool HitsPlayer;         // a última célula é o player?
    public bool HitsWall;           // só o Imparável usa
}

// Todo inimigo que joga no turno dos inimigos implementa isso.
public interface IEnemyTurn
{
    int TurnOrder { get; }                                   // menor = joga primeiro
    EnemyPlan PlanTurn(GridEntity player);                   // puro: só lê o grid
    IEnumerator ExecuteTurn(GridEntity player, EnemyPlan plan); // segue o plano
}


public class EnemyTurnManager : MonoBehaviour
{
    [SerializeField] private NewPlayerController player;
    [SerializeField] private TurnManager turnManager;

    // lista estática: os inimigos se registram no OnEnable, sem depender da ordem de Awake
    private static readonly List<IEnemyTurn> enemies = new();

    public static void Register(IEnemyTurn e)
    {
        if (!enemies.Contains(e)) enemies.Add(e);
    }

    public static void Unregister(IEnemyTurn e) => enemies.Remove(e);

    private bool isRunning;
    private bool hasPendingTurn;

    private void OnEnable()
    {
        if (player == null) player = FindAnyObjectByType<NewPlayerController>();
        if (turnManager == null) turnManager = FindAnyObjectByType<TurnManager>();
        if (player != null) player.Moved += HandlePlayerMoved;
    }

    private void OnDisable()
    {
        if (player != null) player.Moved -= HandlePlayerMoved;
    }

    private void HandlePlayerMoved()
    {
        hasPendingTurn = true;
        if (!isRunning)
            StartCoroutine(RunEnemyTurns());
    }

    private IEnumerator RunEnemyTurns()
    {
        isRunning = true;
        yield return null; // garante que o EndPlayerTurn() do player rode antes de devolvermos o turno

        while (hasPendingTurn && player != null)
        {
            hasPendingTurn = false;

            // espera a animação do player acabar
            while (player.IsAnimating) yield return null;

            // cópia ordenada: a lista pode mudar durante o turno (inimigo destruído/desativado)
            var order = new List<IEnemyTurn>(enemies);
            order.Sort((a, b) => a.TurnOrder.CompareTo(b.TurnOrder));

            foreach (var enemy in order)
            {
                if (GameManager.Instance != null && GameManager.Instance.IsGameOver) break;

                var mb = enemy as MonoBehaviour;
                if (mb == null || !mb.isActiveAndEnabled) continue;

                // 1) planeja (o grid já reflete os inimigos anteriores)
                EnemyPlan plan = enemy.PlanTurn(player);

                // 2) mostra o caminho piscando; o tempo vem do próprio PathPreview
                bool hasPath = plan.Path != null && plan.Path.Count > 0;
                if (hasPath && PathPreview.Instance != null)
                {
                    PathPreview.Instance.Show(plan.Path);
                    yield return new WaitForSeconds(PathPreview.Instance.Duration);
                    PathPreview.Instance.Hide();
                }

                // game over durante a prévia: não executa o turno
                if (GameManager.Instance != null && GameManager.Instance.IsGameOver) break;

                // 3) executa seguindo o plano
                // um inimigo só começa quando o anterior terminou totalmente
                yield return enemy.ExecuteTurn(player, plan);

                var entity = enemy as GridEntity;
                while ((entity != null && entity.IsAnimating) || player.IsAnimating)
                    yield return null;
            }
        }

        isRunning = false;
        turnManager.isPlayerTurn = true;
    }
}
