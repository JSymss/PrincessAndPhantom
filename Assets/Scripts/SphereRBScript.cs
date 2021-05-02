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
    GameObject[] rocksHub;
    public CameraController CameraController;

    private void Start()
    {
        jettyTrack = GameObject.FindGameObjectWithTag("JettyTrack");
        jettyJumps = GameObject.FindGameObjectsWithTag("JettyJump");
        rocks = GameObject.FindGameObjectsWithTag("Rocks");
        rocksHub = GameObject.FindGameObjectsWithTag("RocksHub");
    }

    private void OnTriggerEnter(Collider col)
    {
        print(col.gameObject.name);

        switch (col.gameObject.name) {

            case "TunnelCamera":
                Vector3 newPosition = CameraController.cameraTarget.transform.position;
                newPosition.y -= 5;
                CameraController.cameraTarget.transform.position = newPosition;
                break;
            
            case "Sledge Unlock":
                Destroy(col.gameObject);
                CarController.sledgeUnlock = true;
                CarController.respawnCheckpoint = 2;
                StartCoroutine(Wait());
                break;

            case "Train Unlock":
                Destroy(col.gameObject);
                CarController.trainUnlock = true;
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
            case "DoorToForestLevel":
                SceneManager.LoadScene("Forest_Level");
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
            case "RocksHub":
                if (CarController.myState == CarController.State.Train)
                {
                    foreach (GameObject stone in rocksHub)
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
        switch (col.gameObject.name) {
            
            case "TownTrigger":
            safeFromPirates = false;
                break;
            
            case "TunnelCamera":
                Vector3 newPosition = CameraController.cameraTarget.transform.position;
                newPosition.y += 5;
                CameraController.cameraTarget.transform.position = newPosition;
                
                break;
            default:
                break;
        }
    }
    IEnumerator Wait()
    {
        anim.SetBool("UnlockBool", true);
        yield return new WaitForSeconds(3);
        anim.SetBool("UnlockBool", false);
    }
}

