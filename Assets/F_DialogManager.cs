using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F_DialogManager : MonoBehaviour
{
    AudioSource forestSource;
    public AudioClip f_clip1;
    public AudioClip f_clip2;
    public AudioClip f_clip3;
    public static bool f_clip1Cued = true;
    public static bool f_clip2Cued = true;
    public static bool f_clip3Cued = true;
    void Start()
    {
        forestSource = GetComponent<AudioSource>();
    }

    public IEnumerator F_clip1()
    {
        if (f_clip1Cued)
        {
            forestSource.clip = f_clip1;
            forestSource.Play();
            f_clip1Cued = false;
            yield return null;
        }
    }
    public IEnumerator F_clip2()
    {
        if (f_clip2Cued)
        {
            forestSource.clip = f_clip2;
            forestSource.Play();
            f_clip2Cued = false;
            yield return null;
        }
    }
    public IEnumerator F_clip3()
    {
        if (f_clip3Cued)
        {
            forestSource.clip = f_clip3;
            forestSource.Play();
            f_clip3Cued = false;
            yield return null;
        }
    }
}
