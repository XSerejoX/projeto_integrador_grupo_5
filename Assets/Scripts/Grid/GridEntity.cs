using System.Collections;
using UnityEngine;

public class GridEntity : MonoBehaviour
{
    [SerializeField] private float visualSpeed = 3f;

    [SerializeField] private Animator animator;

    private static readonly int DirX = Animator.StringToHash("DirX");
    private static readonly int DirY = Animator.StringToHash("DirY");
    private static readonly int IsMoving = Animator.StringToHash("IsMoving");
    
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
    public bool TryMove(Vector2Int dir)
    {
        Vector2Int target = Cell + dir;
        if (!GridManager.Instance.IsFree(target)) return false; //se a cell tiver cheia, false

        GridManager.Instance.Move(this, Cell, target); //movendo a entity
        Cell = target; //resetando a cell
        SlideToCurrentCell(dir);//deslocamento visual
        return true;
    }

    private void SlideToCurrentCell(Vector2Int dir)
    {
        if (anim != null) StopCoroutine(anim);
        anim = StartCoroutine(Slide(dir));
    }

    private IEnumerator Slide(Vector2Int dir)
    {
        IsAnimating = true;
        if (animator != null)
        {
            animator.SetFloat(DirX, dir.x); //settando as direções no animator
            animator.SetFloat(DirY, dir.y);
            animator.SetBool(IsMoving, true); //setta o bool de movimento
        }
        
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

        if (animator != null)
            animator.SetBool(IsMoving, false); // volta pro Idle
    
    }


}