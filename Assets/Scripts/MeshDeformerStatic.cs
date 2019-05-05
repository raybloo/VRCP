using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeshDeformerStatic : MonoBehaviour
{
    private bool released = true;
    public float rate = 0.0f;
    private float elapsed = 0.0f;
    public float amplitude = 0.0f;
    private float low = 0.0f;
    private float up = 0.0f;
    private float meanTime = 5.0f;
    private int avgOver = 5;
    private int pushes = 0;
    private float amplitudeFactor = 1.5f;
    private LinkedList<float> timeQ;
    // Public fields
    public Vector3 point = new Vector3(0.0f,15.3f, 1.7f);
    public float deformation = 0.0f;
    public float deformationMax = 10f;
    public Text infoText;
    public RawImage indicator;
    public GraphDrawer graph;
    // Start is called before the first frame update
    void Start()
    {
        timeQ = new LinkedList<float>();
        rate = 0.0f;
        elapsed = 0.0f;
        amplitude = 0.0f;
        pushes = 0;
    }

    // Update is called once per frame
    void Update()
    {
        LinearMeshDeformer deformer = GetComponent<LinearMeshDeformer>();
        if (deformer) {
            elapsed += Time.deltaTime;
            if(deformation > deformationMax) {
                deformer.AddDeformingForce(transform.TransformPoint(point), deformationMax);
                if (released) {
                    released = false;
                    low = deformationMax;
                } else {
                    low = Mathf.Max(low, deformationMax);
                }
            } else if(deformation > 0.0f) {
                deformer.AddDeformingForce(transform.TransformPoint(point), deformation);
                if(deformation > deformationMax/4.0f) {
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
            }
        }
        if(elapsed > meanTime) {
            rate = 0.0f;
            pushes = 0;
        }
        DisplayScore();
    }

    private void PushUp() {
        //Debug.Log("One push since: " + elapsed.ToString());
        pushes++;
        amplitude = low - up;
        if(elapsed > meanTime) {
            timeQ.Clear();
            rate = 0.0f;
            pushes = 0;
        } else {
            timeQ.AddFirst(elapsed);
            ComputeRate();
        }
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

    private void DisplayScore() {
        infoText.text = "Rate: " + Mathf.RoundToInt(rate).ToString() + "\nPushes: " + pushes.ToString() + "\nAmplitude: " + (amplitude * amplitudeFactor).ToString();
        if (rate < 75 || amplitude < 1.7 || rate > 160) {
            infoText.color = new Color(255, 0, 0);
        } else if (rate >= 100 && rate < 135) {
            infoText.color = new Color(0, 255, 0);
        } else {
            infoText.color = new Color(255, 255, 0);
        }
        indicator.rectTransform.localPosition = new Vector3(-0.375f+Mathf.Min((Mathf.Max(rate-60.0f,0.0f)/150.0f),0.75f),0.25f,0.0f);
        if(graph) {
            graph.UpdateGraph(deformation * (-amplitudeFactor), Time.deltaTime);
        }
    }
}
