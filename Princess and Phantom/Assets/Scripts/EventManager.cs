using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour
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
        AnalyticsEvent.LevelStart(thisScene.buildIndex);
        AnalyticsEvent.GameStart();
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
                customParams.Add("vehicle_sledge", CarController.State.Sledge);
                break;
            case CarController.State.SpeedBoat:
                customParams.Add("vehicle_speedboat", CarController.State.SpeedBoat);
                break;
            case CarController.State.Train:
                customParams.Add("vehicle_train", CarController.State.Train);
                break;
            case CarController.State.Car:
            default:
                customParams.Add("vehicle_car", CarController.State.Car);
                break;
        }
    }

    void Update()
    {
        secondsElapsed += Time.deltaTime;
    }

    void OnDestroy()
    {
        customParams.Add("seconds_played", secondsElapsed);
        customParams.Add("deaths", deaths);

        switch (this.levelState)
        {
            case LevelPlayState.Won:
                AnalyticsEvent.LevelComplete(thisScene.buildIndex, customParams);
                break;
            case LevelPlayState.Lost:
                AnalyticsEvent.LevelFail(thisScene.buildIndex, customParams);
                break;
            case LevelPlayState.InProgress:
            case LevelPlayState.Quit:
            default:
                AnalyticsEvent.LevelQuit(thisScene.buildIndex, customParams);
                break;
        }

        if (this.gameState == GamePlayState.Finished)
        {
            AnalyticsEvent.GameOver();
        }
    }
}
