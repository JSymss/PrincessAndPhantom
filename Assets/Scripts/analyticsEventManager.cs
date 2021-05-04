using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;


public class analyticsEventManager : MonoBehaviour
{
   	public enum LevelPlayState { InProgress, Won, Lost, Quit}
    public enum GamePlayState { InProgress, Finished }
    Scene thisScene;
    LevelPlayState levelState = LevelPlayState.InProgress;
    float secondsElapsed = 0;
    int deaths = 0, cannonballStrikes = 0;
    Dictionary<string, object> customParams = new Dictionary<string, object>();
    MenuScript menuUI;

    void Awake()
    {
        thisScene = SceneManager.GetActiveScene();
        Analytics.CustomEvent("gameStarted", new Dictionary<string, object>
	        {
	            { "value", "1" },
	        });
        menuUI = GameObject.FindGameObjectWithTag("MenuManager").GetComponent<MenuScript>();
    }

    public void SetLevelPlayState (LevelPlayState newState)
    {
        this.levelState = newState;
    }

    public void ActiveScene()
    {
        thisScene = SceneManager.GetActiveScene();
        Analytics.CustomEvent("current_scene", new Dictionary<string, object>
            {
                { "scene", thisScene },
            });
    }

    public void IncrementDeaths()
    {
        deaths++;
    }

    public void TakeCannonDamage()
    {
        cannonballStrikes++;
    }

    public void VehicleState()
    {
        switch (CarController.myState)
        {
            case CarController.State.Sledge:
                Analytics.CustomEvent("vehicle_change", new Dictionary<string, object>
		        {
		            { "type", "sledge" }
		        });
                break;
            case CarController.State.SpeedBoat:
                Analytics.CustomEvent("vehicle_change", new Dictionary<string, object>
		        {
		            { "type", "speedBoat" }
		        });
                break;
            case CarController.State.Train:
                Analytics.CustomEvent("vehicle_change", new Dictionary<string, object>
		        {
		            { "type", "train" }
		        });
                break;
            case CarController.State.Car:
            default:
                Analytics.CustomEvent("vehicle_change", new Dictionary<string, object>
		        {
		            { "type", "car" }
		        });
                break;
        }
    }

    public void InteractionWithUI()
    {
        if (menuUI.paused == false)
        {
            Analytics.CustomEvent("game_paused", new Dictionary<string, object>
        {
            {"game_is_paused", "false" }
        });
        }
        else if (menuUI.paused == true)
        {
            Analytics.CustomEvent("game_paused", new Dictionary<string, object>
        {
            {"game_is_paused", "true" }
        });
        }

        if (menuUI.controls.activeSelf == true)
        {
            Analytics.CustomEvent("controls_viewed", new Dictionary<string, object>
        {
            {"controls", "viewed" }
        });
        }
        else if (menuUI.controls.activeSelf == false)
        {
            Analytics.CustomEvent("controls_viewed", new Dictionary<string, object>
        {
            {"controls", "not_viewed" }
        });
        }

        if (thisScene.name == "Hub_Level")
        {
            Analytics.CustomEvent("restart_or_returnToHub", new Dictionary<string, object>
        {
            {"gave_up", "true" }
        });
        }
    }

    public void VehiclesUnlocked()
    {
        if (CarController.sledgeUnlock == true)
        {
            Analytics.CustomEvent("vehicle_unlocked", new Dictionary<string, object>
                {
                    { "type", "sledge" }
                });
        }
        else if (CarController.trainUnlock == true)
        {
            Analytics.CustomEvent("vehicle_unlocked", new Dictionary<string, object>
                {
                    { "type", "train" }
                });
        }
        else if (CarController.speedBoatUnlock == true)
        {
            Analytics.CustomEvent("vehicle_unlocked", new Dictionary<string, object>
                {
                    { "type", "speedboat" }
                });
        }
    }

    void Update()
    {
        secondsElapsed += Time.deltaTime;
    }

    void OnDestroy()
    {


        switch (this.levelState)
        {
            case LevelPlayState.Won:
                Analytics.CustomEvent("gameEnded", new Dictionary<string, object>
		        {
		            { "condition", "win" },
		            { "seconds_played", secondsElapsed },
		            { "deaths", deaths },
                    {"cannonball_strikes", cannonballStrikes },
		        });
                break;
            case LevelPlayState.Lost:
                Analytics.CustomEvent("gameEnded", new Dictionary<string, object>
		        {
		            { "condition", "lost" },
		            { "seconds_played", secondsElapsed },
		            { "deaths", deaths },
                    {"cannonball_strikes", cannonballStrikes },
                });
                break;
            case LevelPlayState.InProgress:
            case LevelPlayState.Quit:
            default:
                Analytics.CustomEvent("gameEnded", new Dictionary<string, object>
		        {
		            { "condition", "quit" },
		            { "seconds_played", secondsElapsed },
		            { "deaths", deaths },
                    {"cannonball_strikes", cannonballStrikes },
                });
                break;
        }
    }
}
