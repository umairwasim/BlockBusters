using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Clips")]
    public AudioClip collisionSfx;
    public AudioClip matchSfx;
    public AudioClip fossilizeSfx;
    public AudioClip explosionSfx;
    public AudioClip bonusSfx;
    public AudioClip buttonClickSfx;
    public AudioClip outOfBoundaSfx;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }


    //Play sound with custom volume level
    public void PlaySound(AudioClip clip, float vol = 1)
    {
        sfxSource.PlayOneShot(clip, vol);
    }

    //Play sound at position
    public void PlaySoundAtPosition(AudioClip clip, Vector3 pos, float vol = 1)
    {
        sfxSource.transform.position = pos;
        PlaySound(clip, vol);
    }
}
