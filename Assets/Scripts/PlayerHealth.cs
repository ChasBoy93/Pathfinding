using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public Text deathText; // Assign a UI Text in Inspector
    public Transform respawnPoint; // Assign a respawn location in Inspector

    private void Start()
    {
        currentHealth = maxHealth;
        deathText.gameObject.SetActive(false);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        deathText.gameObject.SetActive(true);
        deathText.text = "You Died";

        StartCoroutine(Respawn());
    }

     IEnumerator Respawn()
    {
        yield return new WaitForSeconds(2f); // Show message for 2 seconds
        deathText.gameObject.SetActive(false);
        transform.position = respawnPoint.position; // Move player to respawn
        currentHealth = maxHealth;
    }
}
