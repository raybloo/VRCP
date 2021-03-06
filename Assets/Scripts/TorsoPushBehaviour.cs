﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorsoPushBehaviour : MonoBehaviour
{
    public float callibFactor = 20.0f/0.5f;
    public float callibOffset = 0.4f;
    public bool active = true;
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (active) {
            MeshDeformerStatic deformer = target.GetComponent<MeshDeformerStatic>();
            if(deformer) {
                deformer.deformation = Mathf.Min(Mathf.Max(callibOffset - transform.position.y,0.0f) * callibFactor, deformer.deformationMax);
            }
        } 
    }


    public void ResetDeformation() {
        MeshDeformerStatic deformer = target.GetComponent<MeshDeformerStatic>();
        if (deformer) {
            deformer.deformation = 0.0f;
        }
    }

}
