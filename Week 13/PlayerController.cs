using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public int lives;
    private float speed;
    private int weaponType = 1;
    private bool invincible = false;
    private bool hasShield = false;
    private Coroutine speedBoost;
    private Coroutine weaponBoost;
    public GameObject shieldPrefab;
    public GameObject thrusterPrefab;

    private GameManager gameManager;

    private float horizontalInput;
    private float verticalInput;

    public GameObject bulletPrefab;
    public GameObject explosionPrefab;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        lives = 3;
        speed = 5.0f;
        gameManager.ChangeLivesText(lives);
        // Shield and thruster are child objects added to the existing Player prefab.
        if (shieldPrefab != null) shieldPrefab.SetActive(false);
        if (thrusterPrefab != null) thrusterPrefab.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Shooting();
    }

    public void LoseALife()
    {
        if (invincible) return;
        if (hasShield)
        {
            hasShield = false;
            if (shieldPrefab != null) shieldPrefab.SetActive(false);
            gameManager.ManagePowerupText(0);
            gameManager.PlaySound(2);
            return; // the shield absorbed this hit; no life lost
        }

        lives--;
        gameManager.ChangeLivesText(lives);
        if (lives <= 0)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
            return;
        }
        invincible = true;
        SpriteRenderer body = GetComponent<SpriteRenderer>();
        if (body != null) body.color = new Color(1, 1, 1, 0.4f);
        StartCoroutine(InvincibleOff());
    }

    IEnumerator InvincibleOff()
    {
        yield return new WaitForSeconds(2f);
        invincible = false;
        SpriteRenderer body = GetComponent<SpriteRenderer>();
        if (body != null) body.color = Color.white;
        gameManager.PlaySound(2);
    }

    IEnumerator SpeedPowerDown()
    {
        yield return new WaitForSeconds(3f);
        speed = 5f;
        if (thrusterPrefab != null) thrusterPrefab.SetActive(false);
        gameManager.ManagePowerupText(0);
        gameManager.PlaySound(2);
        speedBoost = null;
    }

    IEnumerator WeaponPowerDown()
    {
        yield return new WaitForSeconds(3f);
        weaponType = 1;
        gameManager.ManagePowerupText(0);
        gameManager.PlaySound(2);
        weaponBoost = null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Coin collection is still handled separately by Week 12's CoinPickup.cs.
        if (!other.CompareTag("Powerup")) return;
        Destroy(other.gameObject);
        gameManager.PlaySound(1);

        int choice = Random.Range(1, 5);
        switch (choice)
        {
            case 1:
                speed = 10f;
                if (speedBoost != null) StopCoroutine(speedBoost);
                if (thrusterPrefab != null) thrusterPrefab.SetActive(true);
                speedBoost = StartCoroutine(SpeedPowerDown());
                gameManager.ManagePowerupText(1);
                break;
            case 2:
            case 3:
                weaponType = choice;
                if (weaponBoost != null) StopCoroutine(weaponBoost);
                weaponBoost = StartCoroutine(WeaponPowerDown());
                gameManager.ManagePowerupText(choice);
                break;
            case 4:
                hasShield = true;
                if (shieldPrefab != null) shieldPrefab.SetActive(true);
                gameManager.ManagePowerupText(4);
                break;
        }
    }

    void Shooting()
    {
        if (!Input.GetKeyDown(KeyCode.Space)) return;
        switch (weaponType)
        {
            case 1:
                Instantiate(bulletPrefab, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
                break;
            case 2:
                Instantiate(bulletPrefab, transform.position + new Vector3(-0.5f, 0.5f, 0), Quaternion.identity);
                Instantiate(bulletPrefab, transform.position + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
                break;
            case 3:
                Instantiate(bulletPrefab, transform.position + new Vector3(-0.5f, 0.5f, 0), Quaternion.Euler(0, 0, 45));
                Instantiate(bulletPrefab, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
                Instantiate(bulletPrefab, transform.position + new Vector3(0.5f, 0.5f, 0), Quaternion.Euler(0, 0, -45));
                break;
        }
    }

    void Movement()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * Time.deltaTime * speed);

        float horizontalScreenSize = gameManager.horizontalScreenSize;
        float verticalScreenSize = gameManager.verticalScreenSize;

        if (transform.position.x <= -horizontalScreenSize || transform.position.x > horizontalScreenSize)
        {
            transform.position = new Vector3(transform.position.x * -1, transform.position.y, 0);
        }

        if (transform.position.y <= -verticalScreenSize || transform.position.y > verticalScreenSize)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y * -1, 0);
        }

    }
}
