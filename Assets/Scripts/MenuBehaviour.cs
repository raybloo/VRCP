using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuBehaviour : MonoBehaviour
{
    public int behaviour;
    public bool blocked = false;
    public Sprite greenCircle;
    public Sprite redCircle;
    //public bool gazedAt = false;
    public float delay = 1.0f;
    public float accTime = 0.0f;
    public Image loadCircle;
    public Text menuText;
    public GameObject graph;
    public bool graphIsHidden = false;
    public TimerBehaviour timer;

    // Start is called before the first frame update
    void Start()
    {
        //HideGraph();
    }

    // Update is called once per frame
    void Update()
    {
        if (!blocked) {
            if(timer) {
                if (timer.inUse) {
                    Block();
                }
            } 
            if (loadCircle) {
                loadCircle.fillAmount = (delay - accTime) / delay;
            }
        } else if(timer) {
            if (!timer.inUse) {
                UnBlock();
            }
        }
    }

    private void LateUpdate() {
        if(accTime >= delay) {
            Act();
            accTime = 0.0f;
        }
    }


    void Act() {
        switch(behaviour) {
            case 0:
                //hide graph
                HideGraph();
                break;
            case 1:
                //calibrate
                EnterCalibrationMode();
                break;
            case 2:
                //start a simulation run
                StartSimulation();
                break;
            case 3:
                //start the training
                StartTraining();
                break;
            default:
                Debug.Log("undefined menu item behaviour");
                break;
        }
    }

    void Block() {
        blocked = true;
        accTime = 0.0f;
        if (loadCircle) {
            loadCircle.sprite = redCircle;
            loadCircle.fillAmount = 1.0f;
        }
    }

    void UnBlock() {
        blocked = false;
        accTime = 0.0f;
        if (loadCircle) {
            loadCircle.sprite = greenCircle;
            loadCircle.fillAmount = 1.0f;
        }
    }

    void HideGraph() {
        if(graph) {
            if (graphIsHidden) {
                //graph.transform.position -= new Vector3(100.0f,0.0f,0.0f);
                graph.GetComponent<CanvasGroup>().alpha = 1f;
                graphIsHidden = false;
                menuText.text = "Hide Graph";
            } else {
                //graph.transform.position += new Vector3(100.0f, 0.0f, 0.0f);
                graph.GetComponent<CanvasGroup>().alpha = 0f;
                graphIsHidden = true;
                menuText.text = "Show Graph";
            }
        } else {
            Debug.Log("Error, no graph found");
        }    
    }

    void EnterCalibrationMode() {
        Block();
        if (timer) {
            timer.EnterCalibrationMode();
        } else {
            Debug.Log("Error, no timer found");
        }
    }

    void StartSimulation() {
        Block();
        //simulating = true;
        if (timer) {
            timer.EnterSimulationMode();
        } else {
            Debug.Log("Error, no timer found");
        }
    }

    void StartTraining() {
        if (timer) {
            timer.EnterTrainingMode();
        } else {
            Debug.Log("Error, no timer found");
        }
    }
}
