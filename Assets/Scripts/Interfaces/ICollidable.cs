using UnityEngine;

public interface ICollidable
{
    void OnCollisionEnter2D(Collision2D collision);
}