using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDeformerSpot : MonoBehaviour
{
    // Public fields
    public float force = 10f;
    public Vector3 spot = new Vector3(0.0f,0.0f,0.0f);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MeshDeformer deformer = GetComponent<MeshDeformer>();
        if (deformer) {
            deformer.AddDeformingForce(spot, force);
        }
    }
}
