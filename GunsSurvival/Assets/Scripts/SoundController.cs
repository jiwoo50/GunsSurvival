using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundController : MonoBehaviour
{
    public AudioClip reload;
    public AudioClip fireBazooka;
    public AudioClip getItem;
    public AudioClip fireMachineGun;
    public AudioClip upgrade;

    AudioSource audioSource;

    public static SoundController Instance
    {
        get
        {
            if (!instance) return null;
            return instance;
        }
    }
    private static SoundController instance = null;

    void Awake()
    {
        if (!instance) instance = this;
        else Destroy(this.gameObject);
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();   
    }

    public void PlayReloadSound()
    {
        audioSource.PlayOneShot(reload);
    }

    public void PlayBazookaSound()
    {
        audioSource.PlayOneShot(fireBazooka);
    }

    public void PlayGetItemSound()
    {
        audioSource.PlayOneShot(getItem);
    }

    public void PlayMachineGunSound()
    {
        audioSource.PlayOneShot(fireMachineGun);
    }

    public void PlayUpgradeSound()
    {
        audioSource.PlayOneShot(upgrade);
    }
}
