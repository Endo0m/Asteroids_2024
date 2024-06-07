using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Bullet : ScreenWrapper
{
    private Animator anim;
    private bool isExploding = false;
    public float speed = 500f;
    public float maxLifetime = 1f;
    protected override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        maxLifetime -= Time.deltaTime;
        if (maxLifetime <= 0f)
        {
            Destroy(gameObject);

        }
    }

    public void Shoot(Vector2 direction)
    {
        // Установка угла на основе направления
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        rb.AddForce(direction * speed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isExploding)
        {
            TriggerDestroy();
        }
    }
    private void TriggerDestroy()
    {
        isExploding = true;
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        anim.SetTrigger("Destroy");
        StartCoroutine(DestroyAfterDelay(0.5f));
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}