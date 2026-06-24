using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    AudioSource sfxSource;
    AudioSource musicSource;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(this);
    }

    public void PlaySfx(AudioClip clip)
    {
        if(clip == null) return;   
        sfxSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip clip)
    {
        if(clip==null) return;
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }
}
