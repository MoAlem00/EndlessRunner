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
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
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
            int random = Random.Range(0, gameMusic.Length);
            PlayMusic(gameMusic[random]);
            yield return new WaitForSeconds(gameMusic[random].length);
        }
    }
    
    public void SetBGMVolume(float value)
    {
        float dB = value <= 0.0001f ? -80f : Mathf.Log10(value) * 20f;
        mixer.SetFloat("BGMVolume", dB);
    }

    public void SetSFXVolume(float value)
    {
        float dB = value <= 0.0001f ? -80f : Mathf.Log10(value) * 20f;
        mixer.SetFloat("SFXVolume", dB);
    }
}
