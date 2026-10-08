using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("------------Audio Source------------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
    private AudioSource loopSFXSource;

    [Header("------------Clips------------")]
    public AudioClip background;
    public AudioClip death;
    public AudioClip pickPizzaEnlatada;
    public AudioClip pizzaPronta;
    public AudioClip pickMoeda;
    public AudioClip startForno;
    public AudioClip dash;
    public AudioClip vitoria;
    public AudioClip gobrinaSofreDano;

    private void Awake()
    {
        Instance = this;
        loopSFXSource = gameObject.AddComponent<AudioSource>();
        loopSFXSource.playOnAwake = false;
        if (SFXSource != null)
        {
            loopSFXSource.outputAudioMixerGroup = SFXSource.outputAudioMixerGroup;
            loopSFXSource.volume = SFXSource.volume;
        }
    }

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void PlayLoopSFX(AudioClip clip)
    {
        if (clip == null || (loopSFXSource.clip == clip && loopSFXSource.isPlaying)) return;

        loopSFXSource.clip = clip;
        loopSFXSource.loop = true;
        loopSFXSource.volume = SFXSource != null ? SFXSource.volume : loopSFXSource.volume;
        loopSFXSource.Play();
    }

    public void StopLoopSFX(AudioClip clip)
    {
        if (loopSFXSource.clip != clip) return;

        loopSFXSource.Stop();
        loopSFXSource.clip = null;
    }
}