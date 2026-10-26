using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Todo inimigo que joga no turno dos inimigos implementa isso.
public interface IEnemyTurn
{
    int TurnOrder { get; }                          // menor = joga primeiro
    IEnumerator TakeTurn(GridEntity player);
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

                // um inimigo só começa quando o anterior terminou totalmente
                yield return enemy.TakeTurn(player);

                var entity = enemy as GridEntity;
                while ((entity != null && entity.IsAnimating) || player.IsAnimating)
                    yield return null;
            }
        }

        isRunning = false;
        turnManager.isPlayerTurn = true;
    }
}
