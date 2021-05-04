using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public bool paused = false;
    public GameObject menu, controls, winScreen, healthBar, heart1, heart2, heart3;
    public AudioSource carKeys;
    GameObject player;
    analyticsEventManager analytics;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (SceneManager.GetActiveScene().name == "Water_Level")
        {
            healthBar.gameObject.SetActive(true);
            heart1.gameObject.SetActive(true);
            heart2.gameObject.SetActive(true);
            heart3.gameObject.SetActive(true);
        }
        else
        {
            healthBar.gameObject.SetActive(false);
        }
        menu.gameObject.SetActive(false);
        controls.gameObject.SetActive(false);
        if (winScreen != null)
        {
            winScreen.gameObject.SetActive(false);
        }
        Cursor.visible = false;
        analytics = GameObject.FindGameObjectWithTag("Player").GetComponent<analyticsEventManager>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && winScreen.activeSelf == false)
        {
            if (paused == false)
            {
                menu.gameObject.SetActive(true);
                Time.timeScale = 0;
                paused = true;
                Cursor.visible = true;
                analytics.InteractionWithUI();
            }
            else
            {
                menu.gameObject.SetActive(false);
                Time.timeScale = 1;
                paused = false;
                Cursor.visible = false;
                analytics.InteractionWithUI();
            }
        }
        switch(player.GetComponent<CarController>().health)
        {
            case 3:
            heart1.gameObject.SetActive(true);
            heart2.gameObject.SetActive(true);
            heart3.gameObject.SetActive(true);
                break;
            case 2:
                heart1.gameObject.SetActive(true);
                heart2.gameObject.SetActive(true);
                heart3.gameObject.SetActive(false);
                break;
            case 1:
                heart1.gameObject.SetActive(true);
                heart2.gameObject.SetActive(false);
                heart3.gameObject.SetActive(false);
                break;

            default:
                break;
        }
    }
    
    public void ReturnToHub()
    {
        StartCoroutine(ReturnToHubCoroutine());
    }
    IEnumerator ReturnToHubCoroutine()
    {
        carKeys.Play();
        Time.timeScale = 1;
        paused = false;
        Cursor.visible = false;
        yield return new WaitForSeconds(.6f);
        SceneManager.LoadScene("Hub_Level");
        analytics.InteractionWithUI();
    }
    IEnumerator RestartGame()
    {
        carKeys.Play();
        Time.timeScale = 1;
        paused = false;
        Cursor.visible = false;
        CarController.sledgeUnlock = false;
        CarController.speedBoatUnlock = false;
        CarController.trainUnlock = false;
        DialogManager.intro1Cued = true;
        DialogManager.intro2Cued = true;
        F_DialogManager.f_clip1Cued = true;
        F_DialogManager.f_clip2Cued = true;
        F_DialogManager.f_clip3Cued = true;
        W_DialogManager.w_clip1Cued = true;
        W_DialogManager.w_clip2Cued = true;
        W_DialogManager.w_clip3Cued = true;
        W_DialogManager.w_clip4Cued = true;
        I_DialogManager.i_clip1Cued = true;
        I_DialogManager.i_clip2Cued = true;
        I_DialogManager.i_clip3Cued = true;
        I_DialogManager.i_clip4Cued = true;
        yield return new WaitForSeconds(.6f);
        SceneManager.LoadScene("Hub_Level");
        menu.gameObject.SetActive(false);
        analytics.InteractionWithUI();
    }
    public void PlayGame()
    {
        carKeys.Play();
        menu.gameObject.SetActive(false);
        Time.timeScale = 1;
        paused = false;
        Cursor.visible = false;
        analytics.InteractionWithUI();
    }
    public void Restart()
    {
        StartCoroutine(RestartGame());
    }
    public void QuitGame()
    {
        carKeys.Play();
        Debug.Log("Quit");
        Application.Quit();
    }
    public void Controls()
    {
        carKeys.Play();
        controls.gameObject.SetActive(true);
        analytics.InteractionWithUI();
    }
    public void BackToMenu()
    {
        carKeys.Play();
        controls.gameObject.SetActive(false);
        analytics.InteractionWithUI();
    }
}
