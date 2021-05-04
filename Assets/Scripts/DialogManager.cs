using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    AudioSource hubSource;
    public AudioClip intro1;
    public AudioClip intro2;
    public static bool intro1Cued = true;
    public static bool intro2Cued = true;

    void Start()
    {
        hubSource = GetComponent<AudioSource>();
        hubSource.clip = intro1;
    }

    void Update()
    {
        if (intro1Cued)
        {
            StartCoroutine(Intro1());
        }
        if( intro1Cued == false && intro2Cued && hubSource.isPlaying == false)
        {
            StartCoroutine(Intro2());
        }
    }
    IEnumerator Intro1()
    {
        yield return new WaitForSeconds(1f);
        print(intro1Cued);
        intro1Cued = false;
        hubSource.Play();
    }
    IEnumerator Intro2()
    {
        yield return new WaitForSeconds(.5f);
        hubSource.clip = intro2;
        hubSource.Play();
        intro2Cued = false;
    }
}
