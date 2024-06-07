using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private int maxLives = 3;

    private event System.Action OnDestroyed;

    public void AddExtraLife()
    {
        if (GameManager.Instance.GetLives() < maxLives)
        {
            GameManager.Instance.SetLives(GameManager.Instance.GetLives() + 1);
        }
    }

    public void OnDestroy()
    {
        OnDestroyed?.Invoke();
    }
}