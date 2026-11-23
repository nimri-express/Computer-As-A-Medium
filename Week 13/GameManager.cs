using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{

    public GameObject playerPrefab;
    public GameObject enemyOnePrefab;
    public GameObject cloudPrefab;
    public GameObject coinPrefab; // Week 12 Task 2

    // Week 13: additions only
    public GameObject powerupPrefab;
    public GameObject audioPlayer;
    public AudioClip powerupSound;
    public AudioClip powerdownSound;
    public TextMeshProUGUI powerupText;

    public TextMeshProUGUI livesText;
    public TextMeshProUGUI scoreText; // Week 12 Task 1

    public float horizontalScreenSize;
    public float verticalScreenSize;

    public int score;

    // Start is called before the first frame update
    void Start()
    {
        horizontalScreenSize = 10f;
        verticalScreenSize = 6.5f;
        score = 0;
        if (scoreText != null) scoreText.text = "Score: 0";
        Instantiate(playerPrefab, transform.position, Quaternion.identity);
        CreateSky();
        InvokeRepeating("CreateEnemy", 1, 3);
        InvokeRepeating(nameof(CreateCoin), 2f, 5f); // Week 12 Task 2
        if (powerupText != null) powerupText.text = ""; // No intrusive permanent message
        StartCoroutine(SpawnPowerup());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateEnemy()
    {
        Instantiate(enemyOnePrefab, new Vector3(Random.Range(-horizontalScreenSize, horizontalScreenSize) * 0.9f, verticalScreenSize, 0), Quaternion.Euler(180, 0, 0));
    }

    // Spawn one coin at a random place inside the visible play area.
    void CreateCoin()
    {
        if (coinPrefab == null) return;
        float x = Random.Range(-horizontalScreenSize * 0.75f, horizontalScreenSize * 0.75f);
        float y = Random.Range(-verticalScreenSize * 0.60f, verticalScreenSize * 0.60f);
        Instantiate(coinPrefab, new Vector3(x, y, 0f), Quaternion.identity);
    }


    void CreateSky()
    {
        for (int i = 0; i < 30; i++)
        {
            Instantiate(cloudPrefab, new Vector3(Random.Range(-horizontalScreenSize, horizontalScreenSize), Random.Range(-verticalScreenSize, verticalScreenSize), 0), Quaternion.identity);
        }
        
    }
    public void AddScore(int earnedScore)
    {
        score = score + earnedScore;
        if (scoreText != null) scoreText.text = "Score: " + score;
    }

    // Week 13: the Powerup prefab is the course-provided one.
    void CreatePowerup()
    {
        if (powerupPrefab == null) return;
        Instantiate(powerupPrefab,
            new Vector3(Random.Range(-horizontalScreenSize * 0.8f,
            horizontalScreenSize * 0.8f), verticalScreenSize, 0f),
            Quaternion.identity);
    }

    IEnumerator SpawnPowerup()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(3f, 5f));
            CreatePowerup();
        }
    }

    public void ManagePowerupText(int kind)
    {
        if (powerupText == null) return;
        switch (kind)
        {
            case 1: powerupText.text = "Speed!"; break;
            case 2: powerupText.text = "Double Weapon!"; break;
            case 3: powerupText.text = "Triple Weapon!"; break;
            case 4: powerupText.text = "Shield!"; break;
            default: powerupText.text = ""; break;
        }
    }

    public void PlaySound(int kind)
    {
        AudioSource speaker = audioPlayer != null ? audioPlayer.GetComponent<AudioSource>() : null;
        if (speaker == null) return;
        if (kind == 1 && powerupSound != null) speaker.PlayOneShot(powerupSound);
        if (kind == 2 && powerdownSound != null) speaker.PlayOneShot(powerdownSound);
    }

    public void ChangeLivesText (int currentLives)
    {
        livesText.text = "Lives: " + currentLives;
    }
}
