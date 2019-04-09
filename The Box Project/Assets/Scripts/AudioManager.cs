using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public enum SFXType
    {
        JUMP,
        PLATFORMCHANGE
    }


    #region Static Instance
    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<AudioManager>();
                if (instance == null)
                {
                    instance = new GameObject("Spawned Audio Manager", typeof(AudioManager)).GetComponent<AudioManager>();
                }
            }
            return instance;
        }
        private set
        {
            instance = value;
        }
    }
    #endregion

    #region Fields
    private AudioSource musicSource;
    private AudioSource sfxSource;

    #endregion


    private void Awake()
    {
        //make sure we don't destroy this instance
        DontDestroyOnLoad(this.gameObject);

        //create audio sources
        musicSource = this.gameObject.AddComponent<AudioSource>();
        sfxSource = this.gameObject.AddComponent<AudioSource>();

        //loop the music track
        musicSource.loop = true;

    }
    
    public void PlayMusic()
    {
        musicSource.clip = music;
        musicSource.volume = musicVolume;
        musicSource.Play();
    }


    public void PlaySFX(SFXType SFXType)
    {
        switch (SFXType)
        {
            case SFXType.JUMP:
                sfxSource.Stop();
                sfxSource.PlayOneShot(jumpSFX);
                break;
            case SFXType.PLATFORMCHANGE:
                sfxSource.Stop();
                sfxSource.PlayOneShot(platformChange, platformChangeSFXVolume);
                break;
            default:
                break;
        }
        
    }

    public void PlaySFX(SFXType SFXType, float volume)
    {
        switch (SFXType)
        {
            case SFXType.JUMP:
                sfxSource.Stop();
                sfxSource.PlayOneShot(jumpSFX, volume);
                break;
            case SFXType.PLATFORMCHANGE:
                sfxSource.Stop();
                sfxSource.PlayOneShot(platformChange, volume);
                break;
            default:
                break;
        }
    }


    #region Clips
    [SerializeField] private AudioClip music;
    [SerializeField] private AudioClip jumpSFX;
    [SerializeField] private AudioClip platformChange;
    #endregion


    [SerializeField] private float musicVolume = 1.0f;
    [SerializeField] private float platformChangeSFXVolume = 1.0f;

}
