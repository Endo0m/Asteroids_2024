using UnityEngine;

public class PlayerMovement : MonoBehaviour, IThrustable, IRotatable
{
    [SerializeField]
    private float thrustSpeed = 1f;
    [SerializeField]
    private float rotationSpeed = 0.1f;
    [SerializeField]
    private float boostSpeed = 2f;

    private Rigidbody2D rb;
    private bool thrusting;
    private bool boosting;
    private float turnDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        thrusting = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        boosting = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            turnDirection = 1f;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            turnDirection = -1f;
        }
        else
        {
            turnDirection = 0f;
        }
        if (thrusting)
        {
            SoundManager.Instance.PlayEngineSound();
        }
        else
        {
            SoundManager.Instance.StopEngineSound();
        }
    }

    private void FixedUpdate()
    {
        Thrust();
        Rotate(turnDirection);
    }

    public void Thrust()
    {
        if (thrusting)
        {

            float speed = boosting ? thrustSpeed + boostSpeed : thrustSpeed;
            rb.AddForce(transform.up * speed);
        }
    }

    public void Rotate(float direction)
    {
        if (direction != 0f)
        {
            rb.AddTorque(rotationSpeed * direction);
        }
    }
}