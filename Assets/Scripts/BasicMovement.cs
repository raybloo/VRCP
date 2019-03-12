using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{

	// Public Fields
	public float speed = 1.0f;
	public float rotationSpeed = 100.0f;

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
	void Update()
	{
		// Get the horizontal and vertical axis.
		// By default they are mapped to the arrow keys.
		// The value is in the range -1 to 1
		float upDown = Input.GetAxis("Vertical") * speed;
		float leftRight = Input.GetAxis("Horizontal") * speed;

		// Make it move 10 meters per second instead of 10 meters per frame...
		upDown *= Time.deltaTime;
		leftRight *= Time.deltaTime;
		transform.Translate(leftRight, upDown, 0.0f);


	}

}
