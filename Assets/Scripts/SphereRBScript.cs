using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SphereRBScript : MonoBehaviour
{
    public Animator anim;
    GameObject jettyTrack, jettyTarget;
    GameObject[] jettyJumps;
    public float hiddenDistance;

    private void Start()
    {
        jettyTrack = GameObject.FindGameObjectWithTag("JettyTrack");
        jettyTarget = GameObject.FindGameObjectWithTag("Target");
        jettyJumps = GameObject.FindGameObjectsWithTag("JettyJump");
        jettyTarget.transform.position = new Vector3(jettyTrack.transform.position.x, -300f, jettyTrack.transform.position.z);
        CarController.speedBoatUnlock = false;
        CarController.respawnCheckpoint = 1;
    }

    private void OnTriggerEnter(Collider col)
    {
        print(col.gameObject.name);
        if (col.gameObject.name == "Sledge Unlock")
        {
            Destroy(col.gameObject);
            CarController.sledgeUnlock = true;
            CarController.respawnCheckpoint = 2;
            StartCoroutine(Wait());
        }
        else if (col.gameObject.name == "TownTrigger")
        {
            Destroy(col.gameObject);
            foreach (GameObject ramp in jettyJumps)
            {
                Destroy(ramp);
            }
            CarController.speedBoatUnlock = true;
            CarController.respawnCheckpoint = 2;
            jettyTrack.transform.position = Vector3.MoveTowards(jettyTrack.transform.position, jettyTarget.transform.position, hiddenDistance);
            StartCoroutine(Wait());
        }
        if(col.gameObject.name == "DoorToIceLevel")
        {
            SceneManager.LoadScene("Ice_Level");
        }
        if (col.gameObject.name == "DoorToHubLevel")
        {
            SceneManager.LoadScene("Hub_Level");
        }
    }
    IEnumerator Wait()
    {
        anim.SetBool("UnlockBool", true);
        yield return new WaitForSeconds(3);
        anim.SetBool("UnlockBool", false);
    }
}

