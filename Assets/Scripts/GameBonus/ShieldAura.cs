using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldAura : MonoBehaviour
{
    public float rotationSpeed = 100f;

    private void Update()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}
