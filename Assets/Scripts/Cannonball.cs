using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannonball : MonoBehaviour
{
    public float speed, projectileLifetime;
    Transform player;
    Vector3 target;
    analyticsEventManager analytics;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //make the player's position the target for cannonballs
        target = new Vector3(player.position.x, player.position.y, player.position.z);
        // destroy cannonballs after their set lifetime
        Destroy(gameObject, projectileLifetime);
        analytics = GameObject.FindGameObjectWithTag("Player").GetComponent<analyticsEventManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //shoot cannonballs at player's position when they're shot
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        // destroy projectiles if they reach their target
        if (transform.position.x == target.x && transform.position.y == target.y && transform.position.z == target.z)
        {
            DestroyProjectile();
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            //player takes damage
            player.GetComponent<CarController>().health--;
            analytics.TakeCannonDamage();
            DestroyProjectile();
        }
    }

    void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
