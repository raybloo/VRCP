using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeshDeformerStatic : MonoBehaviour
{
    private bool released = true;
    private float pushes = 0.0f;
    private float rate = 0.0f;
    private float elapsed = 0.0f;
    private float amplitude = 0.0f;
    private float low = 0.0f;
    private float up = 0.0f;
    private float meanTime = 5.0f;
    // Public fields
    public Vector3 point = new Vector3(0.0f,15.3f, 1.7f);
    public float deformation = 0.0f;
    public float deformationMax = 10f;
    public Text infoText;
    // Start is called before the first frame update
    void Start()
    {
        pushes = 0.0f;
        rate = 0.0f;
        elapsed = 0.0f;
        amplitude = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        MeshDeformer deformer = GetComponent<MeshDeformer>();
        if (deformer) {
            elapsed += Time.deltaTime;
            if(elapsed > meanTime) {
                rate = 60.0f * pushes / elapsed;
                elapsed = 0.0f;
                pushes = 0.0f;
                Debug.Log("Rate: " + rate.ToString());
            }
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
                        amplitude = low - up;
                        Debug.Log("Amplitude: " + amplitude.ToString());
                        pushes += 1.0f;
                        released = true;
                        up = deformation;
                    } else {
                        up = Mathf.Min(up, deformation);
                    }
                }
            } else {
                if (!released) {
                    amplitude = low - up;
                    Debug.Log("Amplitude: " + amplitude.ToString());
                    pushes += 1.0f;
                    released = true;
                    up = 0.0f;
                } else {
                    up = 0.0f;
                }
            }
        }
        infoText.text = "Rate: "+rate.ToString()+"\n\nAmplitude: "+amplitude.ToString();
    }
}
