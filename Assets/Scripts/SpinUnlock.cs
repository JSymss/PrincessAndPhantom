using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinUnlock : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(0, 3f, 0);
    }
}
