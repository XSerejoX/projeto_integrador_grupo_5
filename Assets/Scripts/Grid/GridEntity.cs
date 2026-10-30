using System.Collections;
using UnityEngine;

public class GridEntity : MonoBehaviour
{
    [SerializeField] private float visualSpeed = 4f;

    public Vector2Int LastMoveDirection { get; private set; }

    public Vector2Int Cell { get; private set; }
    public bool IsAnimating { get; private set; }

    private Coroutine anim;

    protected virtual void Start()
    {
        var gm = GridManager.Instance; 
        Cell = gm.WorldToCell(transform.position); //convertendo a posicao em int vector2
        gm.Place(this, Cell);//associando essa entity a cell
        transform.position = gm.CellToWorld(Cell); // alinha no centro da cell
    }

    // Move só se estiver livre. A lógica muda hora. A animação vem depois.
    public bool TryMove(Vector2Int dir, bool playMoveAnimation = true)
    {
        Vector2Int target = Cell + dir;
        if (!GridManager.Instance.IsFree(target)) return false; //se a cell tiver cheia, false

        LastMoveDirection = dir;
        GridManager.Instance.Move(this, Cell, target); //movendo a entity
        Cell = target; //resetando a cell
        SlideToCurrentCell(dir, playMoveAnimation);//deslocamento visual
        return true;
    }

    private void SlideToCurrentCell(Vector2Int dir, bool playMoveAnimation)
    {
        if (anim != null) StopCoroutine(anim);
        anim = StartCoroutine(Slide(dir, playMoveAnimation));
    }

    private IEnumerator Slide(Vector2Int dir, bool playMoveAnimation)
    {
        IsAnimating = true;
        if (playMoveAnimation) OnMoveStart(dir);
        
        Vector3 target = GridManager.Instance.CellToWorld(Cell);

            //movimento visual
        while ((transform.position - target).sqrMagnitude > 0.0001f) // enquanto haver distancia
        {
            transform.position = Vector3.MoveTowards(
                transform.position, target, visualSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = target; //resetando posicao
        IsAnimating = false;
        anim = null;

        if (playMoveAnimation) OnMoveEnd();

        GridManager.Instance.NotifyEntityStopped(this);
    }
        //metodos virtuais para serem sobrescritos
        //para individualizar o que acontece no inicio e fim do movimento
        //isso é util para animacoes    
    protected virtual void OnMoveStart(Vector2Int dir) { }
    protected virtual void OnMoveEnd() { }
}