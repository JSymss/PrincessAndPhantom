using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pirate : MonoBehaviour
{
    GameObject[] pirates;
    GameObject player;
    public float turnSpeed, startTimeBtwShots;
    float timeBtwShots;
    public GameObject shot;
    bool safe;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        pirates = GameObject.FindGameObjectsWithTag("Pirate");
        // calling the safeFromPirates bool from the SphereRBScript to stop pirates shooting at player when in town
        safe = GameObject.FindGameObjectWithTag("MotorSphere").GetComponent<SphereRBScript>().safeFromPirates;
    }

    // Update is called once per frame
    void Update()
    {
        // if player isn't safe, rotate towards their position and shoot cannonballs
        if (safe == false)
        {
            for (int i = 0; i < pirates.Length; i++)
            {
                Vector3 vectorToTarget = player.transform.position - pirates[i].transform.position;
                float step = turnSpeed * Time.deltaTime;
                Vector3 newDirection = Vector3.RotateTowards(pirates[i].transform.forward, vectorToTarget, step, 0.0f);
                pirates[i].transform.rotation = Quaternion.LookRotation(newDirection);

                if (timeBtwShots <= 0)
                {
                    Instantiate(shot, pirates[i].transform.position, Quaternion.identity);
                    timeBtwShots = startTimeBtwShots;
                }
                else
                {
                    timeBtwShots -= Time.deltaTime;
                }
            }
        }
    }
}
