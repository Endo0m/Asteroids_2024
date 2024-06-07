using UnityEngine;

public class PlayerShield : MonoBehaviour, IShieldable
{
    internal bool isShieldActive;
    [SerializeField]
    private GameObject shieldAuraPrefab;
    [SerializeField]
    private float shieldDuration = 3f;

    private GameObject shieldAuraInstance;
    

    private void Update()
    {
    }
    public void ActivateShield()
    {
        isShieldActive = true;
        if (shieldAuraInstance == null)
        {
            shieldAuraInstance = Instantiate(shieldAuraPrefab, transform);
        }
        shieldAuraInstance.SetActive(true);
        Invoke(nameof(DeactivateShield), shieldDuration);
    }

    public void DeactivateShield()
    {
        isShieldActive = false;
        if (shieldAuraInstance != null)
        {
            shieldAuraInstance.SetActive(false);
        }
    }
}