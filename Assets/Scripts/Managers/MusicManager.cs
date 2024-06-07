using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip[] musicTracks;
    private AudioSource audioSource;
    private int currentTrackIndex = 0;
    private bool isMuted = false;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        if (musicTracks.Length > 0)
        {
            PlayTrack(currentTrackIndex);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1)) ToggleMute();
        if (Input.GetKeyDown(KeyCode.F2)) AdjustVolume(-0.1f);
        if (Input.GetKeyDown(KeyCode.F3)) AdjustVolume(0.1f);
        if (Input.GetKeyDown(KeyCode.F5)) PlayPreviousTrack();
        if (Input.GetKeyDown(KeyCode.F6)) PlayNextTrack();

        if (!audioSource.isPlaying)
        {
            PlayNextTrack();
        }
    }

    private void ToggleMute()
    {
        isMuted = !isMuted;
        audioSource.mute = isMuted;
    }

    private void AdjustVolume(float adjustment)
    {
        audioSource.volume = Mathf.Clamp(audioSource.volume + adjustment, 0f, 1f);
    }

    private void PlayTrack(int index)
    {
        audioSource.clip = musicTracks[index];
        audioSource.Play();
    }

    private void PlayPreviousTrack()
    {
        currentTrackIndex = (currentTrackIndex - 1 + musicTracks.Length) % musicTracks.Length;
        PlayTrack(currentTrackIndex);
    }

    private void PlayNextTrack()
    {
        currentTrackIndex = (currentTrackIndex + 1) % musicTracks.Length;
        PlayTrack(currentTrackIndex);
    }
}