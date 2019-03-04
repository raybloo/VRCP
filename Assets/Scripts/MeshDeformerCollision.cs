using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDeformerCollision: MonoBehaviour
{
	List<Vector3> spots;

	// Public fields
	public float force = 10f;

    // Start is called before the first frame update
    void Start()
    {
		spots = new List<Vector3>();
    }

    // Update is called once per frame
    void Update()
    {
        MeshDeformer deformer = GetComponent<MeshDeformer>();
        if (deformer) {
			for(int i = 0; i < spots.Count; i++)
			{
				deformer.AddDeformingForce(spots[i], force);
			}
        }
    }

	private void OnCollisionEnter(Collision collision)
	{
		Debug.Log("Collision");
		spots.Clear();
		for (int i = 0; i < collision.contactCount; i++)
		{
			spots.Add(collision.GetContact(i).point);
		}
	}

	private void OnCollisionStay(Collision collision)
	{
		Debug.Log("Collision");
		spots.Clear();
		for (int i = 0; i < collision.contactCount; i++)
		{
			spots.Add(collision.GetContact(i).point);
		}
	}

	private void OnCollisionExit(Collision collision)
	{
		spots.Clear();
	}
}
