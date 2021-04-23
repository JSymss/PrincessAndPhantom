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
    GamePlayState gameState = GamePlayState.InProgress;
    float secondsElapsed = 0;
    int deaths = 0;
    Dictionary<string, object> customParams = new Dictionary<string, object>();

    void Awake()
    {
        thisScene = SceneManager.GetActiveScene();
        Analytics.CustomEvent("gameStarted", new Dictionary<string, object>
	        {
	            { "value", "1" },
	        });
    }

    public void SetLevelPlayState (LevelPlayState newState)
    {
        this.levelState = newState;
    }

    public void IncrementDeaths()
    {
        deaths++;
    }

    public void VehicleState()
    {
        switch (GetComponent<CarController>().myState)
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
		        });
                break;
            case LevelPlayState.Lost:
                Analytics.CustomEvent("gameEnded", new Dictionary<string, object>
		        {
		            { "condition", "lost" },
		            { "seconds_played", secondsElapsed },
		            { "deaths", deaths },
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
		        });
                break;
        }

        if (this.gameState == GamePlayState.Finished)
        {
            Analytics.CustomEvent("gameEnded", new Dictionary<string, object>
		        {
		            { "condition", "gameOver?" },
		            { "seconds_played", secondsElapsed },
		            { "deaths", deaths },
		        });
        }
    }
}
