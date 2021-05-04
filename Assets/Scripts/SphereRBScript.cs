using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SphereRBScript : MonoBehaviour
{
    public Animator anim;
    GameObject jettyTrack;
    GameObject[] jettyJumps, rocks, rocksHub;
    public bool safeFromPirates = false;
    public CarController ccReference;
    public CameraController CameraController;
    public GameObject menu, winScreen;
    Rigidbody rb;
    Rigidbody[] rocksRB;
    AudioSource engine;
    bool doorOpen = false, doorOnce = true, rocksHubExploded = false;
    bool[] rocksExploded;
    analyticsEventManager analytics;
    public GameObject dialogForest;
    public GameObject dialogWater;
    public GameObject dialogIce;

    private void Start()
    {
        jettyTrack = GameObject.FindGameObjectWithTag("JettyTrack");
        jettyJumps = GameObject.FindGameObjectsWithTag("JettyJump");
        engine = GetComponent<AudioSource>();
        engine.Play();
        rocks = GameObject.FindGameObjectsWithTag("Rocks");
        rocksHub = GameObject.FindGameObjectsWithTag("RocksHub");
        rocksExploded = new bool[rocks.Length];
        for (int i = 0; i < rocksExploded.Length; i++)
        {
            rocksExploded[i] = false;
        }
        rocksRB = new Rigidbody[rocks.Length];
        for (int i = 0; i < rocksRB.Length; i++)
        {
            rocksRB[i] = rocks[i].GetComponent<Rigidbody>();
            rocksRB[i].isKinematic = true;
        }
        analytics = GameObject.FindGameObjectWithTag("Player").GetComponent<analyticsEventManager>();
    }
    private void Update()
    {
        rb = GetComponent<Rigidbody>();
        engine.volume = Mathf.Clamp01(rb.velocity.magnitude / 100);
    }

    private void OnTriggerEnter(Collider col)
    {
        print(col.gameObject.name);

        switch (col.gameObject.name) {

            case "DoorToFinalLevel":
                if (doorOnce && CarController.sledgeUnlock && CarController.speedBoatUnlock && CarController.trainUnlock)
                {
                    StartCoroutine(RotateDoor());
                    doorOnce = false;
                }
                analytics.ActiveScene();
                break;
            
            case "TunnelCamera":
                Vector3 newPosition = CameraController.cameraTarget.transform.position;
                newPosition.y -= 5;
                CameraController.cameraTarget.transform.position = newPosition;
                break;
            
            case "Sledge Unlock":
                Destroy(col.gameObject);
                CarController.sledgeUnlock = true;
                CarController.respawnCheckpoint = 2;
                GameObject unlockSledge = GameObject.FindGameObjectWithTag("Unlock");
                AudioSource sfxSledge = unlockSledge.GetComponent<AudioSource>();
                sfxSledge.Play();
                StartCoroutine(Wait());
                analytics.VehiclesUnlocked();
                break;

            case "Train Unlock":
                Destroy(col.gameObject);
                CarController.trainUnlock = true;
                CarController.respawnCheckpoint = 2;
                GameObject unlockTrain = GameObject.FindGameObjectWithTag("Unlock");
                AudioSource sfxTrain = unlockTrain.GetComponent<AudioSource>();
                sfxTrain.Play();
                StartCoroutine(Wait());
                analytics.VehiclesUnlocked();
                break;

            // destroys the jetty track, unlocks the speedboat, sets respawn point to the town, stops pirates shooting at player when they're in the town, and calls the unlock animation
            case "TownTrigger":
                foreach (GameObject ramp in jettyJumps)
                {
                    Destroy(ramp);
                }
                foreach (GameObject stone in rocks)
                {
                    Destroy(stone);
                }
                Destroy(jettyTrack);
                CarController.speedBoatUnlock = true;
                CarController.respawnCheckpoint = 2;
                safeFromPirates = true;
                GameObject unlockBoat = GameObject.FindGameObjectWithTag("Unlock");
                AudioSource sfxBoat = unlockBoat.GetComponent<AudioSource>();
                sfxBoat.Play();
                StartCoroutine(Wait());
                analytics.VehiclesUnlocked();
                break;

            case "DoorToIceLevel":
                SceneManager.LoadScene("Ice_Level");
                analytics.ActiveScene();
                break;

            case "DoorToHubLevel":
                SceneManager.LoadScene("Hub_Level");
                analytics.ActiveScene();
                break;

            case "DoorToWaterLevel":
                SceneManager.LoadScene("Water_Level");
                analytics.ActiveScene();
                break;
            case "DoorToForestLevel":
                SceneManager.LoadScene("Forest_Level");
                analytics.ActiveScene();
                break;

            case "Respawn Point 3":
                CarController.respawnCheckpoint = 3;
                break;

            case "DoorToIceTerrain":
                ccReference.TeleportToIceTerrain();
                analytics.ActiveScene();
                break;

            case "Rocks":
                for (int i = 0; i < rocks.Length; i++)
                {
                    if (CarController.myState == CarController.State.Train && rocksExploded[i] == false)
                    {
                        GameObject rocksExplode = GameObject.FindGameObjectWithTag("ExplodeRocks");
                        AudioSource rocksSfx = rocksExplode.GetComponent<AudioSource>();
                        rocksSfx.Play();
                        rocksRB[i].isKinematic = false;
                        rocksExploded[i] = true;
                    }
                }
                break;
            case "RocksHub":
                if (CarController.myState == CarController.State.Train && rocksHubExploded == false)
                {
                    GameObject rocksExplode = GameObject.FindGameObjectWithTag("ExplodeRocksHub");
                    AudioSource rocksSfx = rocksExplode.GetComponent<AudioSource>();
                    rocksSfx.Play();
                    foreach (GameObject stone in rocksHub)
                    {
                        Rigidbody rb = stone.GetComponent<Rigidbody>();
                        rb.isKinematic = false;
                    }
                    rocksHubExploded = true;
                }
                break;
            case "WinGameTrigger":
                menu.SetActive(true);
                winScreen.SetActive(true);
                Cursor.visible = true;
                break;
            case "F_Dialog1":
                StartCoroutine(dialogForest.GetComponent<F_DialogManager>().F_clip1());
                break;
            case "F_Dialog2":
                StartCoroutine(dialogForest.GetComponent<F_DialogManager>().F_clip2());
                break;
            case "F_Dialog3":
                StartCoroutine(dialogForest.GetComponent<F_DialogManager>().F_clip3());
                break;
            case "W_Dialog1":
                StartCoroutine(dialogWater.GetComponent<W_DialogManager>().W_clip1());
                break;
            case "W_Dialog2":
                StartCoroutine(dialogWater.GetComponent<W_DialogManager>().W_clip2());
                break;
            case "W_Dialog3":
                StartCoroutine(dialogWater.GetComponent<W_DialogManager>().W_clip3());
                break;
            case "W_Dialog4":
                StartCoroutine(dialogWater.GetComponent<W_DialogManager>().W_clip4());
                break;
            case "I_Dialog1":
                StartCoroutine(dialogIce.GetComponent<I_DialogManager>().I_clip1());
                break;
            case "I_Dialog2":
                StartCoroutine(dialogIce.GetComponent<I_DialogManager>().I_clip2());
                break;
            case "I_Dialog3":
                StartCoroutine(dialogIce.GetComponent<I_DialogManager>().I_clip3());
                break;
            case "I_Dialog4":
                StartCoroutine(dialogIce.GetComponent<I_DialogManager>().I_clip4());
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
    IEnumerator RotateDoor()
    {
        if (doorOpen == false)
        {
            GameObject door = GameObject.FindGameObjectWithTag("DoorToFinalLevel");
            for (int i = 0; i <= 90; i++)
            {
                door.transform.Rotate(0, -1, 0);
                yield return new WaitForSeconds(.05f);
            }
            doorOpen = true;
            yield return null;
        }else{ 
            yield return null;
        }
    }
}

