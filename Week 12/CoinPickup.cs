using UnityEngine;

// Week 12 Task 2: a coin is worth one point and expires after a few seconds.
public class CoinPickup : MonoBehaviour
{
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        Destroy(gameObject, 4f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>();

        if (gameManager != null)
            gameManager.AddScore(1);

        Destroy(gameObject);
    }
}
