using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_DialogManager : MonoBehaviour
{
    AudioSource iceSource;
    public AudioClip i_clip1;
    public AudioClip i_clip2;
    public AudioClip i_clip3;
    public AudioClip i_clip4;
    public static bool i_clip1Cued = true;
    public static bool i_clip2Cued = true;
    public static bool i_clip3Cued = true;
    public static bool i_clip4Cued = true;
    void Start()
    {
        iceSource = GetComponent<AudioSource>();
    }

    public IEnumerator I_clip1()
    {
        if (i_clip1Cued)
        {
            iceSource.clip = i_clip1;
            iceSource.Play();
            i_clip1Cued = false;
            yield return null;
        }
    }
    public IEnumerator I_clip2()
    {
        if (i_clip2Cued)
        {
            iceSource.clip = i_clip2;
            iceSource.Play();
            i_clip2Cued = false;
            yield return null;
        }
    }
    public IEnumerator I_clip3()
    {
        if (i_clip3Cued)
        {
            iceSource.clip = i_clip3;
            iceSource.Play();
            i_clip3Cued = false;
            yield return null;
        }
    }
    public IEnumerator I_clip4()
    {
        if (i_clip4Cued)
        {
            iceSource.clip = i_clip4;
            iceSource.Play();
            i_clip4Cued = false;
            yield return null;
        }
    }
}
