using UnityEngine;

public class PlayerShooting : MonoBehaviour, IShootable
{
    [SerializeField]
    private Bullet bulletPrefab;
    [SerializeField]
    private int fanBulletsCount = 5;

    private bool isMultiplyBulletsActive;
    private void Update()
    {
        Shoot();
    }
    public void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isMultiplyBulletsActive && fanBulletsCount > 0)
            {
                float angleStep = 30f;
                float startAngle = -60f;
                for (int i = 0; i < 5; i++)
                {
                    float angle = startAngle + i * angleStep;
                    Quaternion rotation = Quaternion.Euler(0f, 0f, transform.eulerAngles.z + angle);
                    Bullet bullet = Instantiate(bulletPrefab, transform.position, rotation);
                    bullet.Shoot(rotation * Vector2.up);
                }
                fanBulletsCount--;
                if (fanBulletsCount == 0)
                {
                    isMultiplyBulletsActive = false;
                }
            }
            else
            {
                Bullet bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
                bullet.Shoot(transform.up);
            }

            SoundManager.Instance.PlayShootSound();
        }
    }

    public void ActivateMultiplyBullets()
    {
        isMultiplyBulletsActive = true;
        fanBulletsCount = 5;
    }

    public void ResetMultiplyBullets()
    {
        isMultiplyBulletsActive = false;
        fanBulletsCount = 5;
    }
}