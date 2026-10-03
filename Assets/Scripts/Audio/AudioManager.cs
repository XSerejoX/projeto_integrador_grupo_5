using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("------------Audio Source------------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;


    [Header("------------Audio Source------------")]
    public AudioClip background;
    public AudioClip death;
    public AudioClip pickPizzaEnlatada;
    public AudioClip pizzaPronta;
    public AudioClip pickMoeda;
    public AudioClip startForno;
    public AudioClip dash;
    public AudioClip vitoria;
    public AudioClip gobrinaSofreDano;

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
