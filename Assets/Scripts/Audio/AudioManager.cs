using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("------------Audio Source------------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

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
}