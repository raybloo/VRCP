using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeshDeformerStatic : MonoBehaviour
{
    // Public fields
    public float rate = 0.0f;
    public float amplitude = 0.0f;
    public Vector3 point = new Vector3(0.0f,15.3f, 1.7f);
    public float deformation = 0.0f;
    public float deformationMax = 10f;
    public Text infoText;
    public DisplayInfo display;
    public TimerBehaviour timer;
    public RawImage indicator;
    public GraphDrawer graph;
    public GraphDrawer threshold;

    // Private fields
    private bool starting = false;
    private bool simulating = false;
    private bool training = true;
    private bool released = true;
    private float elapsed = 0.0f;
    private float interval = 0.0f;
    private float low = 0.0f;
    private float up = 0.0f;
    private float meanTime = 2.0f;
    private int avgOver = 5;
    private int streak = 0;
    private float amplitudeFactor = 0.75f;
    private float thresholdFactor = 0.4f;
    private float thresholdDelta = 0.2f;
    private LinkedList<float> timeQ;

    private float totalTime = 0.0f;
    private float totalAmplitude = 0.0f;
    private float maxElapsedTime = 0.0f;
    private int longestStreak = 0;
    private int totalPushes = 0;

    // Start is called before the first frame update
    void Start()
    {
        timeQ = new LinkedList<float>();
        rate = 0.0f;
        elapsed = 0.0f;
        amplitude = 0.0f;
        streak = 0;
        if(graph) {
            graph.UpdateThreshold(deformationMax * thresholdFactor * amplitudeFactor);
        }
    }

    // Update is called once per frame
    void Update()
    {
        LinearMeshDeformer deformer = GetComponent<LinearMeshDeformer>();
        if (deformer) {
            elapsed += Time.deltaTime;
            deformer.AddDeformingForce(transform.TransformPoint(point), deformation);
            if (released) {
                if (deformation > deformationMax * thresholdFactor) { //threshold exceeded, start of a new compression
                    released = false;
                    thresholdFactor -= thresholdDelta;
                    graph.UpdateThreshold(deformationMax * thresholdFactor * amplitudeFactor);
                    low = deformation;
                } else {
                    up = Mathf.Min(up, deformation);
                }
            } else {
                if (deformation > deformationMax * thresholdFactor) {
                    low = Mathf.Max(low, deformation);
                } else { //within threshold again, end of the compression
                    PushUp();
                    thresholdFactor += thresholdDelta;
                    graph.UpdateThreshold(deformationMax * thresholdFactor * amplitudeFactor);
                    up = deformation;
                }
            }


            /*
            if (deformation > deformationMax) {
                deformer.AddDeformingForce(transform.TransformPoint(point), deformationMax);
                if (released) {
                    released = false;
                    low = deformationMax;
                } else {
                    low = Mathf.Max(low, deformationMax);
                }
            } else if(deformation > 0.0f) {
                deformer.AddDeformingForce(transform.TransformPoint(point), deformation);
                if(deformation > deformationMax * thresholdFactor) {
                    if(released) {
                        released = false;
                        low = deformation;
                    } else {
                        low = Mathf.Max(low, deformation);
                    }
                } else {
                    if (!released) {
                        PushUp();
                        up = deformation;
                    } else {
                        up = Mathf.Min(up, deformation);
                    }
                }
            } else {
                deformer.AddDeformingForce(transform.TransformPoint(point), 0.0f);
                if (!released) {
                    PushUp();
                    up = 0.0f;
                } else {
                    up = 0.0f;
                }
            }*/
        }

        if (elapsed > meanTime) {
            rate = 0.0f;
            streak = 0;
        }
        if(simulating || training) {
            DisplayScore();
        }
    }

    private void PushUp() {
        //Debug.Log("One push since: " + elapsed.ToString());
        streak++;
        amplitude = low - up;
        if(elapsed > meanTime) {
            timeQ.Clear();
            rate = 0.0f;
            streak = 0;
        } else {
            timeQ.AddFirst(elapsed);
            ComputeRate();
        }
        if (simulating) {
            totalTime += elapsed;
            totalAmplitude += amplitude;
            maxElapsedTime = Mathf.Max(maxElapsedTime,elapsed);
            longestStreak = Mathf.Max(longestStreak, streak);
            totalPushes++;
        }
        if(starting) {
            display.StopDisplaying();
            timer.StartTimer(60.0f);
            starting = false;
            simulating = true;
            totalTime = 0.0f; //totaltime will be divided by the number of compression
            totalAmplitude = 0.0f;
            maxElapsedTime = 0.0f;
            longestStreak = 0;
            totalPushes = 0;
        }
        interval = elapsed;
        elapsed = 0.0f;
        released = true;
    }

    private void ComputeRate() {
        float acc = 0.0f;
        while (timeQ.Count > avgOver) {
            timeQ.RemoveLast();
        }
        foreach (float t in timeQ) {
            acc += t;
        }
        rate = 60.0f * ((float)timeQ.Count) / acc;        
    }

    public void StartOnNextPush() {
        display.Display("The timer will start on the first compression,\n Good Luck!");
        starting = true;
    }

    public void EndOfSimulation() {
        simulating = false;
        float rateAvg = (float) totalPushes * 60.0f / totalTime;
        float amplitudeAvg = totalAmplitude * amplitudeFactor / (float) totalPushes;
        display.Display("Well done, you managed "+totalPushes+" compressions at an average rate of "
            +rateAvg.ToString("###.") + " and an average amplitude of "+amplitudeAvg.ToString(".00")+". The longest interval mesured between two pushes was "+maxElapsedTime.ToString(".00") +
            ". Your longest streak without pause was "+longestStreak);
    }

    public void StartTraining() {
        training = true;
    }

    public void StopTraining() {
        training = false;
    }


    private void DisplayScore() {
        infoText.text = "Rate: " + Mathf.RoundToInt(rate).ToString() + " c/min\nAmplitude: " + (amplitude * amplitudeFactor).ToString("0.00") + " cm\nPushes in a row: " + streak.ToString() + "\nLast interval: " + interval.ToString("0.00") + " sec";
        if (rate < 75 || amplitude < 1.7 || rate > 160) {
            infoText.color = new Color(255, 0, 0);
        } else if (rate >= 100 && rate < 135) {
            infoText.color = new Color(0, 255, 0);
        } else {
            infoText.color = new Color(255, 255, 0);
        }
        indicator.rectTransform.localPosition = new Vector3(-0.375f+Mathf.Min((Mathf.Max(rate-60.0f,0.0f)/150.0f),0.75f),0.25f,0.0f);
        if(graph) {
            graph.UpdateGraph(deformation * amplitudeFactor, Time.deltaTime);
        }
    }
}
