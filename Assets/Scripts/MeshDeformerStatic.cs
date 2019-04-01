using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeshDeformerStatic : MonoBehaviour
{
    private bool released = true;
    private float rate = 0.0f;
    private float elapsed = 0.0f;
    private float amplitude = 0.0f;
    private float low = 0.0f;
    private float up = 0.0f;
    private float meanTime = 5.0f;
    private LinkedList<float> timeQ;
    // Public fields
    public Vector3 point = new Vector3(0.0f,15.3f, 1.7f);
    public float deformation = 0.0f;
    public float deformationMax = 10f;
    public Text infoText;
    // Start is called before the first frame update
    void Start()
    {
        timeQ = new LinkedList<float>();
        rate = 0.0f;
        elapsed = 0.0f;
        amplitude = 0.0f;
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
        if(elapsed >= meanTime) {
            rate = 0.0f;
            timeQ.Clear();
        } else {
            ComputeRate();
        }
        
        infoText.text = "Rate: "+ Mathf.RoundToInt(rate).ToString()+"\n\nAmplitude: "+amplitude.ToString();
    }

    private void PushUp() {
        Debug.Log("One push since: " + elapsed.ToString());
        amplitude = low - up;
        if(timeQ.Count == 0) {
            timeQ.AddLast(1.0f);
        } else {
            timeQ.AddFirst(elapsed);
        }
        elapsed = 0.0f;
        released = true;
    }

    private void ComputeRate() {
        float acc = elapsed;
        float count = 0.0f;
        foreach(float t in timeQ) {
            if(acc < meanTime) {
                acc += t;
                count++;
            }
        }
        if (acc >= meanTime) {
            timeQ.RemoveLast();
            rate = 60.0f * count / meanTime;
        } else {
            rate = 60.0f * count / acc;
        }
    }
}
