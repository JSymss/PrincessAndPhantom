using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SphereRBScript : MonoBehaviour
{
    public Animator anim;
    GameObject jettyTrack;
    GameObject[] jettyJumps;
    public bool safeFromPirates = false;

    private void Start()
    {
        jettyTrack = GameObject.FindGameObjectWithTag("JettyTrack");
        jettyJumps = GameObject.FindGameObjectsWithTag("JettyJump");
        CarController.speedBoatUnlock = false;
        CarController.respawnCheckpoint = 1;
    }

    private void OnTriggerEnter(Collider col)
    {
        print(col.gameObject.name);
        if (col.gameObject.name == "Sledge Unlock")
        {
            Destroy(col.gameObject);
            CarController.sledgeUnlock = true;
            CarController.respawnCheckpoint = 2;
            StartCoroutine(Wait());
        }
        else if (col.gameObject.name == "TownTrigger")
        {
            Destroy(col.gameObject);
            foreach (GameObject ramp in jettyJumps)
            {
                Destroy(ramp);
            }
            Destroy(jettyTrack);
            CarController.speedBoatUnlock = true;
            CarController.respawnCheckpoint = 2;
            safeFromPirates = true;
            StartCoroutine(Wait());
        }
        if(col.gameObject.name == "DoorToIceLevel")
        {
            SceneManager.LoadScene("Ice_Level");
        }
        if (col.gameObject.name == "DoorToHubLevel")
        {
            SceneManager.LoadScene("Hub_Level");
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.name == "TownTrigger")
        {
            safeFromPirates = false;
        }
    }
    IEnumerator Wait()
    {
        anim.SetBool("UnlockBool", true);
        yield return new WaitForSeconds(3);
        anim.SetBool("UnlockBool", false);
    }
}

