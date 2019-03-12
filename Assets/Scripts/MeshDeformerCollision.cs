using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDeformerCollision: MonoBehaviour
{
	List<Vector3> spots;

	// Public fields
	public float force = 10f;
	public float offset = 10f;

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
				deformer.AddDeformingForceWithNormalOffset(spots[i], force);
			}
        }
    }

	private void OnCollisionEnter(Collision collision)
	{
		spots.Clear();
		Vector3 spot;
		for (int i = 0; i < 1; i++) //collision.contactCount
		{
            spot = collision.GetContact(i).point;// + (collision.GetContact(i).normal * offset);
			spots.Add(spot);
			//Debug.Log("Point: " + spot.ToString());
			//Debug.Log("Normal: " + (collision.GetContact(i).normal * offset).ToString());
		}
	}

	private void OnCollisionStay(Collision collision)
	{
		spots.Clear();
		Vector3 spot;
		for (int i = 0; i < 1; i++)//collision.contactCount
		{
            spot = collision.GetContact(i).point;// + (collision.GetContact(i).normal * offset);
			spots.Add(spot);
			//Debug.Log("Point: " + spot.ToString());
			//Debug.Log("Normal: " + (collision.GetContact(i).normal * offset).ToString());
		}
	}

	private void OnCollisionExit(Collision collision)
	{
		spots.Clear();
	}
}
