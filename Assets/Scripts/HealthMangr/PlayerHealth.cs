using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public int maxHealth = 1;
    private int currentHealth;

    public int damageAmount = 1; // temporariamente 1, porque tem 1 inimgo
    
    [SerializeField] private Animator animator;

    private static readonly int DeathX = Animator.StringToHash("DeathX");
    private static readonly int DeathY = Animator.StringToHash("DeathY");
    private static readonly int IsDead = Animator.StringToHash("IsDead");

    void Awake()
    {
        currentHealth = maxHealth;
    }
    
    public void TakeDamage(Vector2Int attackDirection, EnemyHazard attacker)
    {
        if (attacker == null) return;

        currentHealth -= damageAmount;
        AudioManager.Instance.PlaySFX(AudioManager.Instance.gobrinaSofreDano);
        Debug.Log($"Player tomou dano! Vida atual: {currentHealth}", this);

        if (currentHealth <= 0)
            Die(attackDirection);
    }

     private void Die(Vector2Int attackDirection)
    {   
        Debug.Log($"[Death] Time.timeScale no momento da morte: {Time.timeScale}");
        
        if (animator != null)
        {
            animator.SetFloat(DeathX, attackDirection.x);
            animator.SetFloat(DeathY, attackDirection.y);
            animator.SetBool(IsDead, true);
        }
        AudioManager.Instance.PlaySFX(AudioManager.Instance.death);
        GameManager.Instance.TriggerGameOver();
    }

}
