using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshDeformer : MonoBehaviour
{
	Mesh deformingMesh;
	Vector3[] originalVertices, displacedVertices;
	Vector3[] vertexVelocities;

	//Public fields
	public float springForce = 20f;
	public float damping = 5f;
    public float forceThreshold = 0.5f;

	// Start is called before the first frame update
	void Start()
    {
		deformingMesh = GetComponent<MeshFilter>().mesh;
		originalVertices = deformingMesh.vertices;
		displacedVertices = new Vector3[originalVertices.Length];
		for (int i = 0; i < originalVertices.Length; i++) {
			displacedVertices[i] = originalVertices[i];
		}
		vertexVelocities = new Vector3[originalVertices.Length];
    }

    // Update is called once per frame
    void Update()
    {
		for (int i = 0; i < displacedVertices.Length; i++)
		{
			UpdateVertex(i);
		}
		deformingMesh.vertices = displacedVertices;
		deformingMesh.RecalculateNormals();
	}


	public void AddDeformingForce(Vector3 point, float force)
	{
		Debug.DrawLine(Camera.main.transform.position, point);
        float maxForce = 0.0f;
        float currentForce = 0.0f;
        Vector3 maxVertex = new Vector3(0.0f,0.0f,0.0f);
        int maxi = 0;
		for (int i = 0; i < displacedVertices.Length; i++)
		{
			currentForce = AddForceToVertex(i, point, force);
            maxForce = currentForce > maxForce ? currentForce : maxForce;
            maxVertex = currentForce == maxForce ? displacedVertices[i] : maxVertex;
            maxi = currentForce == maxForce ? i : maxi;
		}
        Debug.Log(maxVertex.ToString());
        Debug.Log(maxi.ToString());
	}

    float AddForceToVertex(int i, Vector3 point, float force)
	{
		Vector3 pointToVertex = (Vector3.Scale(displacedVertices[i],transform.lossyScale)) - (Quaternion.Inverse(transform.rotation) * (point - transform.position));

        float attenuatedForce = force * 0.01f / (0.01f + pointToVertex.sqrMagnitude);
        if (pointToVertex.magnitude >= forceThreshold)  
        {
            attenuatedForce = 0.0f;
        }
		float velocity = attenuatedForce * Time.deltaTime;
		vertexVelocities[i] += pointToVertex.normalized * velocity;
        return force;
	}

	void UpdateVertex(int i)
	{
		Vector3 velocity = vertexVelocities[i];
		Vector3 displacement = displacedVertices[i] - originalVertices[i];
		velocity -= displacement * springForce * Time.deltaTime;
		velocity *= 1f - damping * Time.deltaTime;
		vertexVelocities[i] = velocity;
		displacedVertices[i] += velocity * Time.deltaTime;
	}
}
