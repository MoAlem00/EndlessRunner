using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private AudioClip[] gameMusic;
    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioSource musicSource;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(this);
    }

    public void PlaySfx(AudioClip clip, float volume = 1f)
    {
        if(clip == null) return;
        sfxSource.volume = volume;
        sfxSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip clip)
    {
        if(clip==null) return;
        musicSource.clip = clip;
        musicSource.loop = true;
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
}
