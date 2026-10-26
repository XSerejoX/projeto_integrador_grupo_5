using System.Collections;

public interface ITurnEnemy
{
    int TurnOrder { get; }
    void PlanTurn(GridEntity player);              // só lógica, sem esperar nada
    IEnumerator ResolveTurn(GridEntity player);    // dano/knockback, depois dos slides
}