using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDeformerStatic : MonoBehaviour
{
    // Public fields
    public Vector3 point = new Vector3(0.0f,15.3f, 1.7f);
    public float deformation = 10f;
    public float deformationMax = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MeshDeformer deformer = GetComponent<MeshDeformer>();
        if (deformer) {
            if(deformation > deformationMax) {
                deformer.AddDeformingForce(transform.TransformPoint(point), deformationMax);
            } else if(deformation > 0.0f) {
                deformer.AddDeformingForce(transform.TransformPoint(point), deformation);
            }
        }
    }
}
