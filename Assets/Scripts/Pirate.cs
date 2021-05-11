using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pirate : MonoBehaviour
{
    GameObject[] pirates;
    GameObject player;
    public float turnSpeed, startTimeBtwShots, nearDistance, stoppingDistance, speed;
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

                if (Vector3.Distance(pirates[i].transform.position, player.transform.position) < nearDistance)
                {
                    pirates[i].transform.position = Vector3.MoveTowards(pirates[i].transform.position, new Vector3(player.transform.position.x, pirates[i].transform.position.y, player.transform.position.z), -speed * Time.deltaTime);
                }
                else if (Vector3.Distance(pirates[i].transform.position, player.transform.position) > stoppingDistance)
                {
                    pirates[i].transform.position = Vector3.MoveTowards(pirates[i].transform.position, new Vector3(player.transform.position.x, pirates[i].transform.position.y, player.transform.position.z), speed * Time.deltaTime);
                }
                else if (Vector3.Distance(pirates[i].transform.position, player.transform.position) < stoppingDistance && Vector3.Distance(pirates[i].transform.position, player.transform.position) > nearDistance)
                {
                    pirates[i].transform.position = pirates[i].transform.position;
                }

                if (timeBtwShots <= 0)
                {
                    Instantiate(shot, pirates[i].transform.position, Quaternion.identity);
                    //StartCoroutine(CannonFire());
                    timeBtwShots = startTimeBtwShots;
                }
                else
                {
                    timeBtwShots -= Time.deltaTime;
                }
            }
        }
    }

    /*IEnumerator CannonFire()
    {
        for (int i = 0; i < pirates.Length; i++)
        {
            Instantiate(shot, pirates[i].transform.position, Quaternion.identity);
            yield return new WaitForSeconds(60f);
        }

        yield break;
    }*/
}
