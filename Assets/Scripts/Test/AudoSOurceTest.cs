using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudoSOurceTest : MonoBehaviour
{
    public AudioSource audioSouce;

    private void Awake()
    {
        audioSouce = GetComponent<AudioSource>();
        AudioClip clip = Resources.Load("Music/Background", typeof(AudioClip)) as AudioClip;
        audioSouce.clip = clip;
        audioSouce.loop = true;
        audioSouce.Play();
    }

}
