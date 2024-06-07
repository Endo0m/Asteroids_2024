using UnityEngine;

public class EnemyShip : ScreenWrapper
{
    [Header("Movement Settings")]
    [SerializeField]
    private float moveSpeed = 2f;
    [SerializeField]
    private float shootDistance = 5f;
    [SerializeField]
    private float retreatDistance = 10f;
    [SerializeField]
    private float avoidanceDistance = 2f;
    [SerializeField]
    private float avoidanceForce = 5f;
    private Vector2 randomDirection;
    private float changeDirectionInterval = 2f;
    private float changeDirectionTimer;

    [Header("Shooting Settings")]
    [SerializeField]
    private float shootInterval = 1f;
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private float bulletSpeed = 5f;
    [SerializeField]
    private Transform bulletSpawnPoint;
    private float shootTimer;

    [Header("Score Settings")]
    public int scoreValue;

    [Header("Animation Settings")]
    [SerializeField]
    private Animator engineAnimator;

    private Transform player;
    private Vector2 currentVelocity;

    protected override void Start()
    {
        base.Start();
        FindPlayer();
        shootTimer = shootInterval;
        changeDirectionTimer = changeDirectionInterval;
        randomDirection = Random.insideUnitCircle.normalized;
        currentVelocity = Vector2.zero;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        FindPlayer();
        if (player != null)
        {
            Vector2 direction = player.position - transform.position;
            float distanceToPlayer = direction.magnitude;

            if (distanceToPlayer <= shootDistance)
            {
                MoveAndAvoid(direction);
                ShootAtPlayer(direction);
            }
            else if (distanceToPlayer > retreatDistance)
            {
                MoveTowardsPlayer(direction);
            }
            else
            {
                MoveRandomly();
            }
        }
        else
        {
            MoveRandomly();
        }
        UpdateEngineAnimation();
    }

    private void MoveAndAvoid(Vector2 direction)
    {
        Vector2 avoidanceDirection = Vector2.zero;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, avoidanceDistance);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Asteroid"))
            {
                Vector2 avoidance = (transform.position - collider.transform.position).normalized;
                avoidanceDirection += avoidance;
            }
        }
        Vector2 targetVelocity = (-direction.normalized + avoidanceDirection.normalized * avoidanceForce) * moveSpeed;
        rb.velocity = Vector2.Lerp(rb.velocity, targetVelocity, Time.fixedDeltaTime * 2f);
    }

    private void ShootAtPlayer(Vector2 direction)
    {
        shootTimer -= Time.fixedDeltaTime;
        if (shootTimer <= 0f)
        {
            Shoot();
            shootTimer = shootInterval;
        }
    }

    private void MoveTowardsPlayer(Vector2 direction)
    {
        Vector2 targetVelocity = direction.normalized * moveSpeed;
        rb.velocity = Vector2.Lerp(rb.velocity, targetVelocity, Time.fixedDeltaTime * 2f);
    }

    private void MoveRandomly()
    {
        changeDirectionTimer -= Time.fixedDeltaTime;
        if (changeDirectionTimer <= 0f)
        {
            randomDirection = Random.insideUnitCircle.normalized;
            changeDirectionTimer = changeDirectionInterval;
        }

        Vector2 avoidanceDirection = Vector2.zero;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, avoidanceDistance);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Asteroid"))
            {
                Vector2 avoidance = (transform.position - collider.transform.position).normalized;
                avoidanceDirection += avoidance;
            }
        }

        Vector2 targetVelocity = (randomDirection + avoidanceDirection.normalized * avoidanceForce) * moveSpeed;
        rb.velocity = Vector2.Lerp(rb.velocity, targetVelocity, Time.fixedDeltaTime * 2f);
    }

    private void FindPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Shoot()
    {
        if (player == null) return;
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

        Vector2 directionToPlayer = (player.position - bulletSpawnPoint.position).normalized;
        SoundManager.Instance.PlayShootSound();

        bullet.GetComponent<Rigidbody2D>().velocity = directionToPlayer * bulletSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            SoundManager.Instance.PlayExplosionSound();
            GameManager.Instance.OnEnemyDestroyed(this);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Asteroid"))
        {
            GameManager.Instance.OnEnemyDestroyedAsteroid(this);
            SoundManager.Instance.PlayExplosionSound();
            Destroy(gameObject);
        }
    }

    private void UpdateEngineAnimation()
    {
        if (engineAnimator != null)
        {
            if (rb.velocity.magnitude > 0.2)
            {
                engineAnimator.SetBool("NormalEngine", true);
                engineAnimator.SetBool("IdleEngine", false);
            }
            else
            {
                engineAnimator.SetBool("NormalEngine", false);
                engineAnimator.SetBool("IdleEngine", true);
            }
        }
    }
}