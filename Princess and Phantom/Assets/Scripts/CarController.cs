using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public Rigidbody sphereRB;

    float moveInput;
    float turnInput;
    bool isCarGrounded;

    public float airDrag;
    public float groundDrag;
    public float smoothCarRotation = 1f;
    public float gravity;

    public float fwdSpeed;
    public float revSpeed;
    public float turnSpeed;
    public LayerMask groundLayer;

    public Transform leftFrontWheel, rightFrontWheel;
    public float maxWheelTurn = 25;

    void Start()
    {
        // detatch the rigidbody from the car
        sphereRB.transform.parent = null;
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

        // raycast ground check
        RaycastHit hit;
        isCarGrounded = Physics.Raycast(transform.position, -transform.up, out hit, 1f, groundLayer);

        // rotate car to be parallel to ground
        
        //transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation, smoothCarRotation*Time.deltaTime);

        // turn wheels
        leftFrontWheel.localRotation = Quaternion.Euler(leftFrontWheel.localRotation.eulerAngles.x, turnInput * maxWheelTurn, leftFrontWheel.localRotation.eulerAngles.z);
        rightFrontWheel.localRotation = Quaternion.Euler(rightFrontWheel.localRotation.eulerAngles.x, turnInput * maxWheelTurn, rightFrontWheel.localRotation.eulerAngles.z);
        rightFrontWheel.transform.Rotate(1f, 0, 0);
        if (isCarGrounded)
        {
            sphereRB.drag = groundDrag;
        }
        else
        {
            sphereRB.drag = airDrag;
        }

    }
    private void FixedUpdate()
    {
        // add extra gravity
        sphereRB.AddForce(0, -gravity, 0);

        if (isCarGrounded)
        {
            // move car
            turnSpeed = 100f;
            sphereRB.AddForce(transform.forward * moveInput, ForceMode.Acceleration);
        }
        else
        {
            turnSpeed = 30f;
            // add engine weight
            transform.Rotate(.8f, 0, 0);
        }
        
        
    }
}
