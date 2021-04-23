using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public Rigidbody sphereRB;

    float moveInput;
    float turnInput;

    public float groundDrag;
    public float iceDrag;
    public float waterDrag;
    public float tracksDrag;
    public float airDrag;

    public float smoothCarRotation = 1f;
    public float gravity;

    public float fwdSpeed;
    public float revSpeed;
    public float turnSpeed;
    public LayerMask groundLayer;
    public LayerMask iceLayer;
    public LayerMask waterLayer;
    public LayerMask tracksLayer;

    public Transform leftFrontWheel, rightFrontWheel;
    public float maxWheelTurn = 25;

    public GameObject carMesh;
    public GameObject wheelMesh;
    public GameObject sledgeMesh;
    public GameObject trainMesh;
    public GameObject speedBoatMesh;

    public enum Surface { Ground, Ice, Water, Tracks, Air };
    Surface mySurface;
    public enum State { Car, Sledge, SpeedBoat, Train };
    public State myState;
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
    }


    void Update()
    {
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
            fwdSpeed = 250f;
            revSpeed = 150f;
        }
        if (mySurface == Surface.Ice)
        {
            sphereRB.drag = iceDrag;
            turnSpeed = 75f;
            fwdSpeed = 250f;
            revSpeed = 150f;
        }
        if (mySurface == Surface.Water)
        {
            sphereRB.drag = waterDrag;
            turnSpeed = 75f;
            fwdSpeed = 250f;
            revSpeed = 150f;
        }
        if (mySurface == Surface.Tracks)
        {
            sphereRB.drag = tracksDrag;
            turnSpeed = 50f;
            fwdSpeed = 150f;
            revSpeed = 75f;
        }
        if (mySurface == Surface.Air)
        {
            sphereRB.drag = airDrag;
            turnSpeed = 20f;
        }
        // change vehicles
        if (Input.GetKey(KeyCode.Alpha1))
        {
            carMesh.SetActive(true);
            wheelMesh.SetActive(true);
            sledgeMesh.SetActive(false);
            speedBoatMesh.SetActive(false);
            trainMesh.SetActive(false);
            myState = State.Car;
            Debug.Log(myState);
            GetComponent<analyticsEventManager>().VehicleState();
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            carMesh.SetActive(false);
            wheelMesh.SetActive(false);
            sledgeMesh.SetActive(true);
            speedBoatMesh.SetActive(false);
            trainMesh.SetActive(false);
            myState = State.Sledge;
            Debug.Log(myState);
            GetComponent<analyticsEventManager>().VehicleState();
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            carMesh.SetActive(false);
            wheelMesh.SetActive(false);
            sledgeMesh.SetActive(false);
            speedBoatMesh.SetActive(true);
            trainMesh.SetActive(false);
            myState = State.SpeedBoat;
            Debug.Log(myState);
            GetComponent<analyticsEventManager>().VehicleState();
        }
        if (Input.GetKey(KeyCode.Alpha4))
        {
            carMesh.SetActive(false);
            wheelMesh.SetActive(false);
            sledgeMesh.SetActive(false);
            speedBoatMesh.SetActive(false);
            trainMesh.SetActive(true);
            myState = State.Train;
            Debug.Log(myState);
            GetComponent<analyticsEventManager>().VehicleState();
        }
    }
    private void FixedUpdate()
    {
        // spin the wheels
        // SETINDIVIDUALWHEELSHERE.transform.Rotate(1f, 0, 0);

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
            // add engine weight
            if (transform.rotation.x < 30 && transform.rotation.x > -85)
            {
                transform.Rotate(1f, 0, 0);
            }
        }

    }
}
