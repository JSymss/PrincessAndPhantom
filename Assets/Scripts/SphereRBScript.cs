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
    public CarController ccReference;
    GameObject[] rocks;

    private void Start()
    {
        jettyTrack = GameObject.FindGameObjectWithTag("JettyTrack");
        jettyJumps = GameObject.FindGameObjectsWithTag("JettyJump");
        rocks = GameObject.FindGameObjectsWithTag("Rocks");



        

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

            // destroys the jetty track, unlocks the speedboat, sets respawn point to the town, stops pirates shooting at player when they're in the town, and calls the unlock animation
            case "TownTrigger":
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

            case "DoorToWaterLevel":
                SceneManager.LoadScene("Water_Level");
                break;

            case "Respawn Point 3":
                CarController.respawnCheckpoint = 3;
                break;

            case "DoorToIceTerrain":
                ccReference.TeleportToIceTerrain();
                break;

            case "Rocks":
                if (CarController.myState == CarController.State.Train)
                {
                    foreach (GameObject stone in rocks)
                    {
                        Rigidbody rb = stone.GetComponent<Rigidbody>();
                        rb.isKinematic = false;
                    }
                }
                break;

            default:
                break;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        // restarts pirates shooting at player when they leave the town
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

