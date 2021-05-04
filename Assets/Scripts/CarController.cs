using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CarController : MonoBehaviour
{
    public Rigidbody sphereRB;
    float moveInput, turnInput;
    public int health = 3;
    public float groundDrag, iceDrag, waterDrag, tracksDrag, airDrag, smoothCarRotation = 1f, gravity, fwdSpeed, revSpeed, turnSpeed, jumpPower = 3000f, maxWheelTurn = 25;
    public static int respawnCheckpoint=1;
    public static bool sledgeUnlock = false, speedBoatUnlock = false, trainUnlock = false;
    bool jumping;
    public LayerMask groundLayer, iceLayer, waterLayer, tracksLayer;
    public Transform leftFrontWheel, rightFrontWheel;
    public GameObject carMesh, wheelMesh, sledgeMesh, trainMesh, speedBoatMesh, respawnPoint, respawnPoint2, respawnPoint3, iceTerrainCheckpoint;
    GameObject[] trainTracks;
    public RawImage carImage, sledgeImage, speedboatImage, trainImage, sledgeImageLock, speedboatImageLock, trainImageLock;
    public enum Surface { Ground, Ice, Water, Tracks, Air };
    Surface mySurface;
    public enum State { Car, Sledge, SpeedBoat, Train };
    public static State myState;
    analyticsEventManager analytics;
    void Start()
    {
        // detatch the rigidbody from the car
        sphereRB.transform.parent = null;
        mySurface = Surface.Ground;

        carMesh.SetActive(true);
        wheelMesh.SetActive(true);
        sledgeMesh.SetActive(false);
        speedBoatMesh.SetActive(false);
        trainMesh.SetActive(false);
        myState = State.Car;
        // set UI 
        carImage.color = new Color32(255, 255, 255, 255);
        sledgeImage.color = new Color32(80, 80, 80, 150);
        speedboatImage.color = new Color32(80, 80, 80, 150);
        trainImage.color = new Color32(80, 80, 80, 150);

        respawnCheckpoint = 1;
        health = 3;
        trainTracks = GameObject.FindGameObjectsWithTag("TrainTracks");
        analytics = GetComponent<analyticsEventManager>();
    }


    void Update()
    {
        // set UI lock blackouts
        if (sledgeUnlock)
        {
            sledgeImageLock.color = new Color32(0, 0, 0, 0);
        }
        if (speedBoatUnlock)
        {
            speedboatImageLock.color = new Color32(0, 0, 0, 0);
        }
        if (trainUnlock)
        {
            trainImageLock.color = new Color32(0, 0, 0, 0);
        }

        moveInput = Input.GetAxisRaw("Vertical");
        turnInput = Input.GetAxisRaw("Horizontal");
        
        // adjust speed for car
        moveInput *= moveInput > 0 ? fwdSpeed:revSpeed;

        // set cars position to sphere
        transform.position = sphereRB.transform.position;

        // sets car rotation
        float newRotation = turnInput * turnSpeed * Time.deltaTime * Input.GetAxisRaw("Vertical");
        transform.Rotate(0, newRotation, 0,Space.World);

        
        // change surface state
        if(Physics.Raycast(transform.position, -transform.up, 1f, groundLayer))
        {
            mySurface = Surface.Ground;
        }
        if (Physics.Raycast(transform.position, -transform.up, 1f, iceLayer))
        {
            mySurface = Surface.Ice;
        }
        if (Physics.Raycast(transform.position, -transform.up, 1f, waterLayer))
        {
            mySurface = Surface.Water;
        }
        if (Physics.Raycast(transform.position, -transform.up, 1f, tracksLayer))
        {
            mySurface = Surface.Tracks;
        }
        if (Physics.Raycast(transform.position, -transform.up, 1f) == false)
        {
            mySurface = Surface.Air;
        }

        // raycast ground check
        RaycastHit hit;
        Physics.Raycast(transform.position, -transform.up, out hit, 1f);
        // rotate car to be parallel to ground
        // stiff rotation in comment below
        // transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation, smoothCarRotation*Time.deltaTime);

        // turn wheels
        leftFrontWheel.localRotation = Quaternion.Euler(leftFrontWheel.localRotation.eulerAngles.x, turnInput * maxWheelTurn, leftFrontWheel.localRotation.eulerAngles.z);
        rightFrontWheel.localRotation = Quaternion.Euler(rightFrontWheel.localRotation.eulerAngles.x, turnInput * maxWheelTurn, rightFrontWheel.localRotation.eulerAngles.z);



        // set variables based on surface type
        if (mySurface == Surface.Ground)
        {
            sphereRB.drag = groundDrag;
            turnSpeed = 100f;
            fwdSpeed = 225f;
            revSpeed = 125f;
        }
        if (mySurface == Surface.Ice)
        {
            sphereRB.drag = iceDrag;
            turnSpeed = 120f;
            fwdSpeed = 175f;
            revSpeed = 100f;
        }
        if (mySurface == Surface.Water)
        {
            sphereRB.drag = waterDrag;
            turnSpeed = 120f;
            fwdSpeed = 150f;
            revSpeed = 75f;
            if(myState != State.SpeedBoat)
            {
                // respawn at this point if you're in water and not in a speedboat
                if (respawnCheckpoint == 1)
                {
                    transform.position = respawnPoint.transform.position;
                    transform.rotation = respawnPoint.transform.rotation;
                    sphereRB.velocity = new Vector3(0, 0, 0);
                    sphereRB.position = respawnPoint.transform.position;
                    sphereRB.transform.rotation = respawnPoint.transform.rotation;
                    analytics.IncrementDeaths();
                    analytics.ActiveScene();
                }
                else if (respawnCheckpoint == 2)
                {
                    transform.position = respawnPoint2.transform.position;
                    transform.rotation = respawnPoint2.transform.rotation;
                    sphereRB.velocity = new Vector3(0, 0, 0);
                    sphereRB.position = respawnPoint2.transform.position;
                    sphereRB.transform.rotation = respawnPoint2.transform.rotation;
                    analytics.IncrementDeaths();
                    analytics.ActiveScene();
                }
                else if (respawnCheckpoint == 3)
                {
                    transform.position = respawnPoint3.transform.position;
                    transform.rotation = respawnPoint3.transform.rotation;
                    sphereRB.velocity = new Vector3(0, 0, 0);
                    sphereRB.position = respawnPoint3.transform.position;
                    sphereRB.transform.rotation = respawnPoint3.transform.rotation;
                    analytics.IncrementDeaths();
                    analytics.ActiveScene();
                }
            }
        }
        if (mySurface == Surface.Tracks)
        {
            sphereRB.drag = tracksDrag;
            turnSpeed = 25f;
            fwdSpeed = 200f;
            revSpeed = 100f;
        }
        if (mySurface == Surface.Air)
        {
            sphereRB.drag = airDrag;
            turnSpeed = 20f;
        }
        // change speeds based on state and surface
        if (myState == State.Car)
        {
            if (mySurface != Surface.Ground)
            {
                fwdSpeed = 50f;
                revSpeed = 25f;
            }
        }
        if (myState == State.SpeedBoat)
        {
            if(mySurface != Surface.Water)
            {
                fwdSpeed = 50f;
                revSpeed = 25f;
            }
        }
        if (myState == State.Sledge)
        {
            if (mySurface != Surface.Ice)
            {
                fwdSpeed = 50f;
                revSpeed = 25f;
            }
        }
         if (myState == State.Train)
        {
            fwdSpeed = 300f;
            revSpeed = 150f;
            turnSpeed = 25f;
            if (mySurface != Surface.Ground && mySurface != Surface.Tracks)
            {
                fwdSpeed = 50f;
                revSpeed = 25f;
                turnSpeed = 15f;
            }
        }
        // change vehicles
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            carMesh.SetActive(true);
            wheelMesh.SetActive(true);
            sledgeMesh.SetActive(false);
            speedBoatMesh.SetActive(false);
            trainMesh.SetActive(false);
            myState = State.Car;
            Debug.Log(myState);
            // set UI 
            carImage.color = new Color32(255, 255, 255, 255);
            sledgeImage.color = new Color32(80, 80, 80, 150);
            speedboatImage.color = new Color32(80, 80, 80, 150);
            trainImage.color = new Color32(80, 80, 80, 150);
            foreach (GameObject trainTrack in trainTracks)
            {
                trainTrack.GetComponentInChildren<BoxCollider>().enabled = false;
            }
            GetComponent<analyticsEventManager>().VehicleState();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)&&sledgeUnlock==true)
        {
            carMesh.SetActive(false);
            wheelMesh.SetActive(false);
            sledgeMesh.SetActive(true);
            speedBoatMesh.SetActive(false);
            trainMesh.SetActive(false);
            myState = State.Sledge;
            Debug.Log(myState);
            // set UI 
            sledgeImage.color = new Color32(255, 255, 255, 255);
            carImage.color = new Color32(80, 80, 80, 150);
            speedboatImage.color = new Color32(80, 80, 80, 150);
            trainImage.color = new Color32(80, 80, 80, 150);
            foreach (GameObject trainTrack in trainTracks)
            {
                trainTrack.GetComponentInChildren<BoxCollider>().enabled = false;
            }
            GetComponent<analyticsEventManager>().VehicleState();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && speedBoatUnlock == true)
        {
            carMesh.SetActive(false);
            wheelMesh.SetActive(false);
            sledgeMesh.SetActive(false);
            speedBoatMesh.SetActive(true);
            trainMesh.SetActive(false);
            myState = State.SpeedBoat;
            Debug.Log(myState);
            // set UI 
            speedboatImage.color = new Color32(255, 255, 255, 255);
            carImage.color = new Color32(80, 80, 80, 150);
            sledgeImage.color = new Color32(80, 80, 80, 150);
            trainImage.color = new Color32(80, 80, 80, 150);
            foreach (GameObject trainTrack in trainTracks)
            {
                trainTrack.GetComponentInChildren<BoxCollider>().enabled = false;
            }
            GetComponent<analyticsEventManager>().VehicleState();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && trainUnlock == true)
        {
            carMesh.SetActive(false);
            wheelMesh.SetActive(false);
            sledgeMesh.SetActive(false);
            speedBoatMesh.SetActive(false);
            trainMesh.SetActive(true);
            myState = State.Train;
            Debug.Log(myState);
            // set UI 
            trainImage.color = new Color32(255, 255, 255, 255);
            carImage.color = new Color32(80, 80, 80, 150);
            sledgeImage.color = new Color32(80, 80, 80, 150);
            speedboatImage.color = new Color32(80, 80, 80, 150);
            foreach (GameObject trainTrack in trainTracks)
            {
                trainTrack.GetComponentInChildren<BoxCollider>().enabled = true;
            }
            GetComponent<analyticsEventManager>().VehicleState();
        }
        if (Input.GetKeyDown(KeyCode.Space)&& mySurface != Surface.Air)
        {
            Debug.Log("jumping");
            sphereRB.AddForce(transform.up * jumpPower);
            StartCoroutine(JumpBool());
            AudioSource jump = GetComponent<AudioSource>();
            jump.Play();
        }
        // press R to restart from a checkpoint
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (respawnCheckpoint == 1)
            {
                transform.position = respawnPoint.transform.position;
                transform.rotation = respawnPoint.transform.rotation;
                sphereRB.velocity = new Vector3(0,0,0);
                sphereRB.position = respawnPoint.transform.position;
                sphereRB.transform.rotation = respawnPoint.transform.rotation;
                analytics.IncrementDeaths();
                analytics.ActiveScene();
            }
            else if (respawnCheckpoint == 2)
            {
                transform.position = respawnPoint2.transform.position;
                transform.rotation = respawnPoint2.transform.rotation;
                sphereRB.velocity = new Vector3(0, 0, 0);
                sphereRB.position = respawnPoint2.transform.position;
                sphereRB.transform.rotation = respawnPoint2.transform.rotation;
                analytics.IncrementDeaths();
                analytics.ActiveScene();
            }
            else if (respawnCheckpoint == 3)
            {
                transform.position = respawnPoint3.transform.position;
                transform.rotation = respawnPoint3.transform.rotation;
                sphereRB.velocity = new Vector3(0, 0, 0);
                sphereRB.position = respawnPoint3.transform.position;
                sphereRB.transform.rotation = respawnPoint3.transform.rotation;
                analytics.IncrementDeaths();
                analytics.ActiveScene();
            }
        }
        // when health = 0, respawn
        if (health == 0)
        {
            if (respawnCheckpoint == 1)
            {
                transform.position = respawnPoint.transform.position;
                transform.rotation = respawnPoint.transform.rotation;
                sphereRB.velocity = new Vector3(0, 0, 0);
                sphereRB.position = respawnPoint.transform.position;
                sphereRB.transform.rotation = respawnPoint.transform.rotation;
                analytics.IncrementDeaths();
                analytics.ActiveScene();
            }
            else if (respawnCheckpoint == 2)
            {
                transform.position = respawnPoint2.transform.position;
                transform.rotation = respawnPoint2.transform.rotation;
                sphereRB.velocity = new Vector3(0, 0, 0);
                sphereRB.position = respawnPoint2.transform.position;
                sphereRB.transform.rotation = respawnPoint2.transform.rotation;
                analytics.IncrementDeaths();
                analytics.ActiveScene();
            }
            else if (respawnCheckpoint == 3)
            {
                transform.position = respawnPoint3.transform.position;
                transform.rotation = respawnPoint3.transform.rotation;
                sphereRB.velocity = new Vector3(0, 0, 0);
                sphereRB.position = respawnPoint3.transform.position;
                sphereRB.transform.rotation = respawnPoint3.transform.rotation;
                analytics.IncrementDeaths();
                analytics.ActiveScene();
            }
            health = 3;
        }
    }
    private void FixedUpdate()
    {
        // spin the wheels
        // NOTHING WORKS OMG

        // add extra gravity
        sphereRB.AddForce(0, -gravity, 0);

        // car handling for each surface
        if (mySurface != Surface.Air)
        {
            // move car
            sphereRB.AddForce(transform.forward * moveInput, ForceMode.Acceleration);
        }
        // if raycast hits nothing (air)
        if(mySurface == Surface.Air)
        {
            Debug.Log("Airbourne");
            // add engine weight
            if (jumping == false)
            {
                Debug.Log("jumping = false");
                transform.Rotate(1f, 0, 0);
            }
        }

    }
    IEnumerator JumpBool()
    {
        Debug.Log("start jump");
        jumping = true;
        yield return new WaitForSeconds(.5f);
        jumping = false;
        Debug.Log("end jump");
    }
    public void TeleportToIceTerrain()
    {
        transform.position = iceTerrainCheckpoint.transform.position;
        transform.rotation = iceTerrainCheckpoint.transform.rotation;
        sphereRB.velocity = new Vector3(0, 0, 0);
        sphereRB.position = iceTerrainCheckpoint.transform.position;
        sphereRB.transform.rotation = iceTerrainCheckpoint.transform.rotation;
    }
   
}

