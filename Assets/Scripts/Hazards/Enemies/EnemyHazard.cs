using System.Collections;
using UnityEngine;

public abstract class EnemyHazard : GridEntity
{
    // Executa um plano já calculado: anda célula por célula pela lista.
    // Efeitos de tile (gelo) podem empurrar o inimigo para além da célula planejada,
    // então depois de cada passo ele confere onde realmente está e se ressincroniza.
    protected IEnumerator FollowPlan(GridEntity player, EnemyPlan plan)
    {
        bool completed = true;

        if (plan.Path != null)
        {
            int i = 0;
            while (i < plan.Path.Count)
            {
                Vector2Int step = plan.Path[i];
                Vector2Int dir = step - Cell;

                // o passo precisa ser exatamente uma célula adjacente;
                // senão o plano ficou fora de sincronia com o grid
                if (Mathf.Abs(dir.x) + Mathf.Abs(dir.y) != 1)
                {
                    completed = false;
                    break;
                }

                if (step == player.Cell) // chegou no player: ataca + knockback
                {
                    PlayerHealth health = player.GetComponent<PlayerHealth>();
                    if (health != null)
                        health.TakeDamage(dir, this);

                    TryMove(dir);
                    completed = false; // o ataque encerra o turno, não é "bater na parede"
                    break;
                }

                if (!TryMove(dir))
                {
                    completed = false; // algo mudou no grid: para sem quebrar
                    break;
                }

                while (IsAnimating) yield return null;

                // o gelo pode ter deslizado o inimigo além da célula planejada
                if (Cell != step)
                {
                    int idx = plan.Path.IndexOf(Cell);
                    if (idx < 0)
                    {
                        completed = false; // deslizou para fora do plano: encerra
                        break;
                    }
                    i = idx; // pula as células que o deslize já cobriu
                }

                i++;
            }
        }

        if (completed && plan.HitsWall)
            OnHitWall();

        while (IsAnimating || player.IsAnimating) yield return null;
    }

    protected virtual void OnHitWall() { }
}
