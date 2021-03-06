using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(MeshFilter))]
public class LinearMeshDeformer : MonoBehaviour
{
	Mesh deformingMesh;
	MeshCollider meshCollider;
	Vector3[] originalVertices, displacedVertices;
	Vector3[] vertexDisplacement;
    public float d_width = 0.0001f;

	//Public fields
	public float magnitude = 0.2f;
    public float forceThreshold = 10f;
    public float defaultOffset = 0.1f;

	// Start is called before the first frame update
	void Start()
    {
		deformingMesh = GetComponent<SkinnedMeshRenderer>().sharedMesh;
        meshCollider = GetComponent<MeshCollider>();
		originalVertices = deformingMesh.vertices;
		displacedVertices = new Vector3[originalVertices.Length];
		for (int i = 0; i < originalVertices.Length; i++) {
			displacedVertices[i] = originalVertices[i];
		}
		vertexDisplacement = new Vector3[originalVertices.Length];
    }

    void Reset() 
    {
        deformingMesh.vertices = originalVertices;
    }

    private void OnApplicationQuit() {
        deformingMesh.vertices = originalVertices;
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
        float vertexForce = force * d_width / (d_width + pointToVertex.sqrMagnitude);
		float displacement = vertexForce * magnitude;
        vertexDisplacement[i] = pointToVertex.normalized * displacement;
	}

    void UpdateVertex(int i)
	{
        displacedVertices[i] = originalVertices[i] + vertexDisplacement[i];
	}
}
