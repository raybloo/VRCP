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
		for (int i = 0; i < displacedVertices.Length; i++)
		{
			AddForceToVertex(i, point, force);
		}
	}

	void AddForceToVertex(int i, Vector3 point, float force)
	{
		Vector3 pointToVertex = (Vector3.Scale(displacedVertices[i],transform.lossyScale)) - (Quaternion.Inverse(transform.rotation) * (point - transform.position));
		float attenuatedForce = force / (1f + pointToVertex.sqrMagnitude);
		float velocity = attenuatedForce * Time.deltaTime;
		vertexVelocities[i] += pointToVertex.normalized * velocity;
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
