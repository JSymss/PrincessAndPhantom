using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SphereRBScript : MonoBehaviour
{
    public Animator anim;

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

