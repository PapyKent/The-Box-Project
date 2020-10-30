using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yube;

public class AudioManager : Singleton<AudioManager>
{
	public enum SFXType
	{
		JUMP,
		PLATFORMCHANGE
	}

	protected override void Awake()
	{
		base.Awake();

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

	[Header("Audio clips")]
	[SerializeField]
	private AudioClip music = null;
	[SerializeField]
	private AudioClip jumpSFX = null;
	[SerializeField]
	private AudioClip platformChange = null;

	[Header("Audio settings")]
	[SerializeField]
	private float musicVolume = 1.0f;
	[SerializeField]
	private float platformChangeSFXVolume = 1.0f;

	[NonSerialized]
	private AudioSource musicSource = null;
	[NonSerialized]
	private AudioSource sfxSource = null;
}