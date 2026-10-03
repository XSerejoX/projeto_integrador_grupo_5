using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public int maxHealth = 1;
    private int currentHealth;

    public int damageAmount = 1; // temporariamente 1, porque tem 1 inimgo
    
    public PerseguidorController perseguidorController;

    [SerializeField] private Animator animator;

    private static readonly int DeathX = Animator.StringToHash("DeathX");
    private static readonly int DeathY = Animator.StringToHash("DeathY");
    private static readonly int IsDead = Animator.StringToHash("IsDead");

    void Awake()
    {
        currentHealth = maxHealth;
    }
    
       private void OnEnable()
    {
        if (perseguidorController == null)
        {
            perseguidorController = FindAnyObjectByType<PerseguidorController>();
        }  
    } 

    public void TakeDamage(Vector2Int attackDirection)
    {
        if (perseguidorController.isContactWithEnemy)
        {
            currentHealth -= damageAmount;
            Debug.Log($"Player tomou dano! Vida atual: {currentHealth}", this);
            perseguidorController.isContactWithEnemy = false; 
        
            if (currentHealth <= 0)
            {
                Die(attackDirection);
            
            }
        }
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

        GameManager.Instance.TriggerGameOver();
    }

}
