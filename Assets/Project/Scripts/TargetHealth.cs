
using UnityEngine;

public class TargetHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;

    private float currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        Debug.Log(
            $"{gameObject.name} took {damage} damage. " +
            $"Health: {Mathf.Max(currentHealth, 0f)}/{maxHealth}"
        );

        if (currentHealth <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
