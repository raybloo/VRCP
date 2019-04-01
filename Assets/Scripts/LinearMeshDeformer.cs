using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class LinearMeshDeformer : MonoBehaviour
{
	Mesh deformingMesh;
	MeshCollider meshCollider;
	Vector3[] originalVertices, displacedVertices;
	Vector3[] vertexDisplacement;

	//Public fields
	public float magnitude = 0.2f;
    public float forceThreshold = 10f;
    public float defaultOffset = 0.1f;

	// Start is called before the first frame update
	void Start()
    {
		deformingMesh = GetComponent<MeshFilter>().mesh;
		meshCollider = GetComponent<MeshCollider>();
		originalVertices = deformingMesh.vertices;
		displacedVertices = new Vector3[originalVertices.Length];
		for (int i = 0; i < originalVertices.Length; i++) {
			displacedVertices[i] = originalVertices[i];
		}
		vertexDisplacement = new Vector3[originalVertices.Length];
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
        Vector3 pointToVertex = (Vector3.Scale(originalVertices[i],transform.lossyScale)) - (Quaternion.Inverse(transform.rotation) * (point - transform.position));
        pointToVertex = (originalVertices[i] - transform.InverseTransformPoint(point)) * transform.localScale.x;
        float vertexForce = force * 0.01f / (0.01f + pointToVertex.sqrMagnitude);
		float displacement = vertexForce * magnitude;
        vertexDisplacement[i] = pointToVertex.normalized * displacement;
	}

    void UpdateVertex(int i)
	{
        displacedVertices[i] = originalVertices[i] + vertexDisplacement[i];
	}
}
