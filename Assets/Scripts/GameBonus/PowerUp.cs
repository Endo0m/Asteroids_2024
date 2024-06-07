using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType
    {
        Shield,
        MultiplyBullets,
        ExtraLife
    }

    public PowerUpType type;
    public float lifetime = 3f;

    private void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerCollision player = other.GetComponent<PlayerCollision>();
            PlayerShield playerShield = other.GetComponent<PlayerShield>();
            PlayerShooting playerShooting = other.GetComponent<PlayerShooting>();
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (player != null)
            {
                switch (type)
                {
                    case PowerUpType.Shield:
                        playerShield.ActivateShield();
                        SoundManager.Instance.PlayShieldSound();
                        break;
                    case PowerUpType.MultiplyBullets:
                        playerShooting.ActivateMultiplyBullets();
                        SoundManager.Instance.PlayWeaponUpgradeSound();
                        break;
                    case PowerUpType.ExtraLife:
                        playerHealth.AddExtraLife();
                        SoundManager.Instance.PlayLifePickupSound();
                        break;
                }
                Destroy(gameObject);
            }
        }
    }
}