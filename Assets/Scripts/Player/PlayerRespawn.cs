using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField]
    private float respawnDelay = 3f;
    [SerializeField]
    private float respawnInvulnerability = 3f;

    private void OnEnable()
    {
        TurnOffCollisions();
        Invoke(nameof(TurnOnCollisions), respawnInvulnerability);
    }
    public float RespawnDelay
    {
        get { return respawnDelay; }
    }

    private void TurnOffCollisions()
    {
        gameObject.layer = LayerMask.NameToLayer("Ignore Collisions");
    }

    private void TurnOnCollisions()
    {
        gameObject.layer = LayerMask.NameToLayer("Player");
    }
}