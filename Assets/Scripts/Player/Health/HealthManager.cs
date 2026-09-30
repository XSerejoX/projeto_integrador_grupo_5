using UnityEngine;

public interface IDamage
{
    public void Damage();
}


public class HealthManager : MonoBehaviour
{
  
    public int PlayerHealth {get => _playerHealth; set => _playerHealth = value; }
    public int collisionDamage = 1;
    public int _playerHealth = 3;
    
    public int currentHealth;


    void Awake()
    {
        currentHealth = _playerHealth;
        Debug.Log($"Player health initialized: {currentHealth}", this);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"Player current health: {currentHealth}", this);
    }



}
