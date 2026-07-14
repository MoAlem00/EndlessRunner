using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private AudioClip[] gameMusic;
    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioMixer mixer;
    private System.Random musicRandom = new System.Random();
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void Start()
    {
        float bgm = PlayerPrefs.GetFloat("BGMVolume", 1f);
        float sfx = PlayerPrefs.GetFloat("SFXVolume", 1f);
        SetBGMVolume(bgm);
        SetSFXVolume(sfx);
    }

    public void PlaySfx(AudioClip clip, float volume = 1f)
    {
        if(clip == null) return;
        sfxSource.volume = volume;
        sfxSource.PlayOneShot(clip);
    }

    private void PlayMusic(AudioClip clip)
    {
        if(clip==null) return;
        musicSource.clip = clip;
        musicSource.loop = false;
        musicSource.Play();
    }
    
    public IEnumerator PlayShuffleMusic()
    {
        if (gameMusic == null || gameMusic.Length == 0) yield break; 
        while (true)
        {
            int random = musicRandom.Next(0, gameMusic.Length);
            PlayMusic(gameMusic[random]);
            yield return new WaitWhile(() => musicSource.isPlaying);
        }
    }
    
    public void SetBGMVolume(float value)
    {
        float dB = value <= 0.0001f ? -80f : Mathf.Log10(value) * 20f;
        mixer.SetFloat("BGMVolume", dB);
        PlayerPrefs.SetFloat("BGMVolume", value);
    }

    public void SetSFXVolume(float value)
    {
        float dB = value <= 0.0001f ? -80f : Mathf.Log10(value) * 20f;
        mixer.SetFloat("SFXVolume", dB);
        PlayerPrefs.SetFloat("SFXVolume", value);
    }
}
