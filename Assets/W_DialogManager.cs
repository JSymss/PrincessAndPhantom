using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W_DialogManager : MonoBehaviour
{
    AudioSource waterSource;
    public AudioClip w_clip1;
    public AudioClip w_clip2;
    public AudioClip w_clip3;
    public AudioClip w_clip4;
    public static bool w_clip1Cued = true;
    public static bool w_clip2Cued = true;
    public static bool w_clip3Cued = true;
    public static bool w_clip4Cued = true;
    void Start()
    {
        waterSource = GetComponent<AudioSource>();
    }

    public IEnumerator W_clip1()
    {
        if (w_clip1Cued)
        {
            waterSource.clip = w_clip1;
            waterSource.Play();
            w_clip1Cued = false;
            yield return null;
        }
    }
    public IEnumerator W_clip2()
    {
        if (w_clip2Cued)
        {
            waterSource.clip = w_clip2;
            waterSource.Play();
            w_clip2Cued = false;
            yield return null;
        }
    }
    public IEnumerator W_clip3()
    {
        if (w_clip3Cued)
        {
            waterSource.clip = w_clip3;
            waterSource.Play();
            w_clip3Cued = false;
            yield return null;
        }
    }
    public IEnumerator W_clip4()
    {
        if (w_clip4Cued)
        {
            waterSource.clip = w_clip4;
            waterSource.Play();
            w_clip4Cued = false;
            yield return null;
        }
    }
}
