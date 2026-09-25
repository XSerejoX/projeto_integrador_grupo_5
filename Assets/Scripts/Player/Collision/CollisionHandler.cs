using UnityEngine;

// centraliza toda a logica de colisão do player: limites do mapa e obstaculos (paredes, MovementLimiter etc)
public class CollisionHandler : MonoBehaviour
{
        //configura o tamanho do mapa
    public Vector2 minMaxMapScaleX = new Vector2(1f, 10f); // largura minima: 1, depois largura máxima: 8
    public Vector2 minMaxMapScaleY = new Vector2(1f, 10f); // altura minima: 1, depois altura máxima: 8

        //ray de detecção de obstaculos (parede, movement limiter etc)
    public string obstacleTag = "Obstacle";
    public LayerMask obstacleLayer = ~0; 
    public RaycastHit2D hit;  
    
    
     public bool IsInsideMap(Vector2 position)
    {
        return position.x >= minMaxMapScaleX.x // largura minima
            && position.x <= minMaxMapScaleX.y // largura maxima
            && position.y >= minMaxMapScaleY.x // altura minima
            && position.y <= minMaxMapScaleY.y;// altura maxima
    }
    public float raySkin = 0.1f; // pequena margem pra lidar com borda exata

    public bool IsPathBlocked(Vector2 origin, Vector2 direction, float distance)
    {
        if (direction == Vector2.zero) return false;

        float castDistance = Mathf.Max(0f, distance - raySkin);
        hit = Physics2D.Raycast(origin, direction.normalized, castDistance, obstacleLayer);

        return hit.collider != null && hit.collider.CompareTag(obstacleTag);
    }

        // combina as duas checagens: dentro do mapa E sem obstaculo bloqueando o caminho
    public bool CanMoveTo(Vector2 origin, Vector2 direction, float distance, Vector2 targetPosition)
    {
        return IsInsideMap(targetPosition) && !IsPathBlocked(origin, direction, distance);
    }

    


}