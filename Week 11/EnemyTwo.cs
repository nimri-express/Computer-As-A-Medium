
using UnityEngine;

public class EnemyTwo : MonoBehaviour
{
    private float downwardSpeed = 3f;
    private float sidewaysSpeed = 4f;
    private float direction = 1f;

    private float switchInterval = 0.75f;
    private float timer = 0f;

    void Update()
    {
        // Move diagonally downward
        transform.Translate(
            new Vector3(direction * sidewaysSpeed, -downwardSpeed, 0f)
            * Time.deltaTime,
            Space.World
        );

        // Reverse horizontal direction to create a zigzag
        timer += Time.deltaTime;

        if (timer >= switchInterval)
        {
            direction *= -1f;
            timer -= switchInterval;
        }

        // Destroy enemy after leaving the bottom of the screen
        if (transform.position.y < -6.5f)
        {
            Destroy(gameObject);
        }
    }
}
