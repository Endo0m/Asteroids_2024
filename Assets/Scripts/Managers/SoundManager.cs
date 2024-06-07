using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Sound Clips")]
    public AudioClip explosionSound;
    public AudioClip shootSound;
    public AudioClip lifePickupSound;
    public AudioClip weaponUpgradeSound;
    public AudioClip shieldSound;
    public AudioClip exitGameSound;
    public AudioClip openWindowSound;
    public AudioClip closeWindowSound;
    public AudioClip engineSound;
    public AudioClip leaderboardSound;
    public AudioClip gameOverSound;
    public AudioClip readySound;

    private AudioSource audioSource;
    [SerializeField]
    private AudioSource engineAudioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
        engineAudioSource = gameObject.AddComponent<AudioSource>();
        engineAudioSource.clip = engineSound;
        engineAudioSource.loop = true;
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    public void PlayExplosionSound() => PlaySound(explosionSound);
    public void PlayShootSound() => PlaySound(shootSound);
    public void PlayLifePickupSound() => PlaySound(lifePickupSound);
    public void PlayWeaponUpgradeSound() => PlaySound(weaponUpgradeSound);
    public void PlayShieldSound() => PlaySound(shieldSound);
    public void PlayExitGameSound() => PlaySound(exitGameSound);
    public void PlayOpenWindowSound() => PlaySound(openWindowSound);
    public void PlayCloseWindowSound() => PlaySound(closeWindowSound);
    public void PlayLeaderBoardSound() => PlaySound(leaderboardSound);
    public void GameOverSound() => PlaySound(gameOverSound);
    public void ReadySound() => PlaySound(readySound);

    public void PlayEngineSound()
    {
        if (!engineAudioSource.isPlaying)
        {
            engineAudioSource.Play();
        }
    }

    public void StopEngineSound()
    {
        if (engineAudioSource.isPlaying)
        {
            engineAudioSource.Stop();
        }
    }
}