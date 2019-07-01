using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerBehaviour : MonoBehaviour
{
    private Text timerText;
    private float remainingTime = 0.0f;
    private bool calibrate = false;
    private bool running = false;

    public bool inUse = false;
    //public GameObject hand;
    public TorsoPushBehaviour pusher;
    public MeshDeformerStatic mesurer;
    public DisplayInfo display;

    // Start is called before the first frame update
    void Start()
    {
        timerText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if(running) {
            if (!calibrate) {
                if (remainingTime > 0.0f) {
                    remainingTime -= Time.deltaTime;
                    UpdateText(remainingTime);
                } else {
                    OnTimerEnd();
                    UpdateText(0.0f);
                    running = false;
                }
            } else {
                if(pusher) {
                    if (remainingTime > 0.0f) {
                        if(pusher.active) {
                            remainingTime -= Time.deltaTime;
                            UpdateText(remainingTime);
                        }
                    } else {
                        OnTimerEnd();
                        UpdateText(0.0f);
                        running = false;
                    }
                } else {
                    Debug.Log("Error, torso push behaviour script not found");
                }
            }
        } 
    }

    public void StartTimer(float time) {
        if(!running) {
            remainingTime = time;
            running = true;
        } else {
            if(display) {
                display.DisplayTimed("Wait until the timer has finished before starting \na new calibration or simulation process.", 3.0f);
            }
        }
    }

    public void EnterCalibrationMode() {
        display.StopDisplaying();
        mesurer.StopTraining();
        display.Display("Please, lay your hands in CPR position on the \nmannequin, and remain still for 5 seconds");
        StartTimer(5.0f);
        calibrate = true;
        inUse = true;
    }

    public void EnterSimulationMode() {
        display.StopDisplaying();
        mesurer.StopTraining();
        mesurer.StartOnNextPush();
        inUse = true;
    }

    public void EnterTrainingMode() {
        display.StopDisplaying();
        display.DisplayTimed("Training without time constraint",2.0f);
        mesurer.StartTraining();
    }

    void UpdateText(float time) {
        string min = Mathf.FloorToInt(time * 0.01666666666f).ToString("00.");
        string sec = Mathf.FloorToInt(time % 60.0f).ToString("00.");
        string cent = Mathf.FloorToInt((time * 100) % 100).ToString("00.");
        timerText.text = min + ":" + sec + ":" + cent;
    }

    void OnTimerEnd() {
        if(calibrate) {
            if(pusher) {
                pusher.callibOffset = pusher.gameObject.transform.position.y;
            } else {
                Debug.Log("Error, torso push behaviour script not found");
            }
            display.StopDisplaying();
            display.DisplayTimed("Calibration Done",2.0f);
            calibrate = false;
        } else {
            mesurer.EndOfSimulation();
        }
        inUse = false;
    }

    
}
