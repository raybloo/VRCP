using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorsoPushBehaviour : MonoBehaviour
{
    // Public fields
    public float callibFactor = 8.0f/0.5f;
    public float callibOffset = 0.5f;
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
                deformer.deformation = (callibOffset - transform.position.y) * callibFactor;
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
