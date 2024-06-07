using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private PlayerCollision player;
    [SerializeField] private PlayerShooting playerShooting;
    [SerializeField] private PlayerRespawn playerRespawn;
    [SerializeField] private PlayerShield playerShield;
    [SerializeField] private ParticleSystem explosionEffect;

    private int score;
    private int lives;

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        NewGame();
    }

    private void Update()
    {
        if (lives <= 0 && Input.GetKeyDown(KeyCode.Space) && IsLeaderboardActive())
        {
            Time.timeScale = 1;
            NewGame();
        }
    }

    private void NewGame()
    {
        ClearGameObjects<Asteroid>();
        ClearGameObjects<EnemyShip>();
        ClearGameObjects<PowerUp>();

        UIManager.Instance.HideGameOverUI();
        UIManager.Instance.HideScoreOverUI();
        UIManager.Instance.HidePauseMenuUI();

        SetScore(0);
        SetLives(3);
        playerShooting.ResetMultiplyBullets();
        Respawn();
    }

    private void ClearGameObjects<T>() where T : MonoBehaviour
    {
        T[] gameObjects = FindObjectsOfType<T>();
        foreach (T gameObject in gameObjects)
        {
            Destroy(gameObject.gameObject);
        }
    }

    private void SetScore(int score)
    {
        if (score >= 999999)
        {
            this.score = 999999;
            UIManager.Instance.UpdateScore(this.score);
            Time.timeScale = 0;
            UIManager.Instance.ShowGameOverUI();
            return;
        }

        this.score = score;
        UIManager.Instance.UpdateScore(score);
    }

    public void SetLives(int lives)
    {
        this.lives = lives;
        UIManager.Instance.UpdateLives(lives);
    }

    private void Respawn()
    {
        SoundManager.Instance.ReadySound();

        playerShield.ActivateShield();
        player.transform.position = Vector3.zero;
        player.gameObject.SetActive(true);
    }

    public void OnAsteroidDestroyed(Asteroid asteroid)
    {
        PlayExplosionEffect(asteroid.transform.position);
        SetScore(score + GetScoreForAsteroid(asteroid.size));
    }

    public void OnEnemyDestroyed(EnemyShip enemy)
    {
        PlayExplosionEffect(enemy.transform.position);
        SetScore(score + enemy.scoreValue);
    }

    public void OnEnemyDestroyedAsteroid(EnemyShip enemy)
    {
        PlayExplosionEffect(enemy.transform.position);
    }

    private void PlayExplosionEffect(Vector3 position)
    {
        explosionEffect.transform.position = position;
        explosionEffect.Play();
    }

    private int GetScoreForAsteroid(float size)
    {
        if (size < 0.7f)
        {
            return 100;
        }
        else if (size < 1.4f)
        {
            return 50;
        }
        else
        {
            return 25;
        }
    }

    public void OnPlayerDeath(PlayerCollision playerCollision)
    {
        player.gameObject.SetActive(false);
        PlayExplosionEffect(player.transform.position);
        SetLives(lives - 1);
        SoundManager.Instance.StopEngineSound();

        if (lives <= 0)
        {
            Time.timeScale = 0;
            UIManager.Instance.ShowGameOverUI();
            SoundManager.Instance.GameOverSound();
            SoundManager.Instance.StopEngineSound();
        }
        else
        {
            Invoke(nameof(Respawn), playerRespawn.RespawnDelay);
        }
    }

    public int GetScore()
    {
        return score;
    }

    public int GetLives()
    {
        return lives;
    }

    public bool IsGameOver()
    {
        return lives <= 0;
    }

    public bool IsLeaderboardActive()
    {
        return UIManager.Instance.IsLeaderboardActive();
    }
}