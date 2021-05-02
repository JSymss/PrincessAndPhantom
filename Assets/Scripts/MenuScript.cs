using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    bool paused = false;
    public GameObject menu;
    public AudioSource carKeys;
    public GameObject controls;
    public GameObject winScreen;
    private void Start()
    {
        menu.gameObject.SetActive(false);
        controls.gameObject.SetActive(false);
        if (winScreen != null)
        {
            winScreen.gameObject.SetActive(false);
        }
        Cursor.visible = false;
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
            }
            else
            {
                menu.gameObject.SetActive(false);
                Time.timeScale = 1;
                paused = false;
                Cursor.visible = false;
            }
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
        yield return new WaitForSeconds(.6f);
        SceneManager.LoadScene("Hub_Level");
        menu.gameObject.SetActive(false);
    }
    public void PlayGame()
    {
        carKeys.Play();
        menu.gameObject.SetActive(false);
        Time.timeScale = 1;
        paused = false;
        Cursor.visible = false;
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
    }
    public void BackToMenu()
    {
        carKeys.Play();
        controls.gameObject.SetActive(false);
    }
}
