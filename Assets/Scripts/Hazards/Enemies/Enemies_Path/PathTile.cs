using System.Collections;
using UnityEngine;

// Fica no prefab do tile de prévia (um GameObject com SpriteRenderer).
// Pisca ligando/desligando o alpha. Quem define quantas vezes e em que
// intervalo é o PathPreview, que chama Blink().
[RequireComponent(typeof(SpriteRenderer))]
public class PathTile : MonoBehaviour
{
    [Range(0f, 1f)]
    [SerializeField] private float onAlpha = 0.5f; // opacidade "ligado"

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        SetAlpha(0f); // começa invisível até o Blink começar
    }

    public void Blink(int count, float interval)
    {
        StartCoroutine(BlinkRoutine(count, interval));
    }

    private IEnumerator BlinkRoutine(int count, float interval)
    {
        for (int i = 0; i < count; i++)
        {
            SetAlpha(onAlpha);
            yield return new WaitForSeconds(interval);
            SetAlpha(0f);
            yield return new WaitForSeconds(interval);
        }
    }

    private void SetAlpha(float a)
    {
        Color c = sr.color;
        c.a = a;
        sr.color = c;
    }
}
