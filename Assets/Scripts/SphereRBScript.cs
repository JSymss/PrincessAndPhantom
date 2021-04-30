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
        CarController.respawnCheckpoint = 1;
    }

    private void OnTriggerEnter(Collider col)
    {
        print(col.gameObject.name);

        switch (col.gameObject.name) {
            
            case "Sledge Unlock":
                Destroy(col.gameObject);
                CarController.sledgeUnlock = true;
                CarController.respawnCheckpoint = 2;
                StartCoroutine(Wait());
                break;

            case "TownTrigger":
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
                break;

            case "DoorToIceLevel":
                SceneManager.LoadScene("Ice_Level");
                break;

            case "DoorToHubLevel":
                SceneManager.LoadScene("Hub_Level");
                break;

            case "Respawn Point 3":
                CarController.respawnCheckpoint = 3;
                break;

            default:
                break;
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

