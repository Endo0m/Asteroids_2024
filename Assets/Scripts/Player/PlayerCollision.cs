using UnityEngine;

public class PlayerCollision : ScreenWrapper, ICollidable
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        GameObject[] boundaries = GameObject.FindGameObjectsWithTag("Boundary");
        for (int i = 0; i < boundaries.Length; i++)
        {
            boundaries[i].SetActive(!screenWrapping);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
      
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            if (!GetComponent<PlayerShield>().isShieldActive)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = 0f;
                SoundManager.Instance.PlayExplosionSound();
                GameManager.Instance.OnPlayerDeath(this);
            }
        }
        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            if (!GetComponent<PlayerShield>().isShieldActive)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = 0f;
                SoundManager.Instance.PlayExplosionSound();
                GameManager.Instance.OnPlayerDeath(this);
            }
        }

    }
}