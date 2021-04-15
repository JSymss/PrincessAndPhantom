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

    public float fwdSpeed;
    public float revSpeed;
    public float turnSpeed;
    public LayerMask groundLayer;

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
        transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;

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
        if (isCarGrounded)
        {
            // move car
            sphereRB.AddForce(transform.forward * moveInput, ForceMode.Acceleration);
        }
        else
        {
            // add extra gravity
            sphereRB.AddForce(transform.up * -30f);
        }
        
        
    }
}
