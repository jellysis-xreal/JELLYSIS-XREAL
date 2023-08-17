using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public AudioClip[] Music = new AudioClip[2]; // 사용할 BGM
    AudioSource audioSource;

    void Awake()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    public void PlayGameBGM()
    {
        audioSource.clip = Music[0];
        audioSource.Play();
    }

    public void PlayEndingBGM()
    {
        audioSource.clip = Music[1];
        audioSource.Play();
    }
    
    // void Update()
    // {
    //     if (!audioSource.isPlaying)
    //         RandomPlay();
    // }
    //
    // void RandomPlay()
    // {
    //     audioSource.clip = Music[Random.Range(0, Music.Length)];
    //     audioSource.Play();
    // }
}
