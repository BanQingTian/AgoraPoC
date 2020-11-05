using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum AUDIOCLIPENUM
{
    HOVER = 0,
    CLICK
}

public class SoundController : MonoBehaviour
{
    public static SoundController Instance;

    private void Awake()
    {
        Instance = this;
    }

    public AudioSource AS;
    public AudioClip HoverClip;
    public AudioClip ClkClip;


    public void Play(AUDIOCLIPENUM ac)
    {
        switch (ac)
        {
            case AUDIOCLIPENUM.HOVER:
                AS.clip = HoverClip;
                break;
            case AUDIOCLIPENUM.CLICK:
                AS.clip = ClkClip;
                break;
            default:
                break;
        }

        AS.Play();

    }

}
