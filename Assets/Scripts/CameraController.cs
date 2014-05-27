﻿using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	private Transform player;

	public float smooth = 1.5f;         // The relative speed at which the camera will catch up.

	private Vector3 offset;             // The relative position of the camera from the player.
	private Vector3 newPos;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player").transform;

		offset = transform.position - player.position;
	}

	void FixedUpdate ()
	{
		// The standard position of the camera is the relative position of the camera from the player.
		Vector3 standardPos = player.position + offset;
		
		// The abovePos is directly above the player at the same distance as the standard position.
		Vector3 abovePos = player.position + Vector3.up * offset.magnitude;
		
		// An array of 5 points to check if the camera can see the player.
		Vector3[] checkPoints = new Vector3[5];
		
		// The first is the standard position of the camera.
		checkPoints[0] = standardPos;
		
		// The next three are 25%, 50% and 75% of the distance between the standard position and abovePos.
		checkPoints[1] = Vector3.Lerp(standardPos, abovePos, 0.25f);
		checkPoints[2] = Vector3.Lerp(standardPos, abovePos, 0.5f);
		checkPoints[3] = Vector3.Lerp(standardPos, abovePos, 0.75f);
		
		// The last is the abovePos.
		checkPoints[4] = abovePos;
		
		// Run through the check points...
		for(int i = 0; i < checkPoints.Length; i++)
		{
			// ... if the camera can see the player...
			if(ViewingPosCheck(checkPoints[i]))
				// ... break from the loop.
				break;
		}
		
		// Lerp the camera's position between it's current position and it's new position.
		transform.position = Vector3.Lerp(transform.position, newPos, smooth * Time.deltaTime);
		
		// Make sure the camera is looking at the player.
		SmoothLookAt();
	}
	
	
	bool ViewingPosCheck (Vector3 checkPos)
	{
		RaycastHit hit;
		
		// If a raycast from the check position to the player hits something...
		if(Physics.Raycast(checkPos, player.position - checkPos, out hit, offset.magnitude))
			// ... if it is not the player...
			if(hit.transform != player)
				// This position isn't appropriate.
				return false;
		
		// If we haven't hit anything or we've hit the player, this is an appropriate position.
		newPos = checkPos;
		return true;
	}
	
	// Update is called once per frame
	/*
	void FixedUpdate () {
		if(Input.GetKey(KeyCode.Q)) {
			
			Vector3 rotated = transform.position;
			
			float x = rotated.x - player.position.x;
			float z = rotated.z - player.position.z;
			float y = rotated.y;
			
			rotated.x = Mathf.Cos(20)*x - Mathf.Sin(20)*z;
			rotated.z = Mathf.Sin(20)*x + Mathf.Cos(20)*z;
			rotated.y = y;
			
			rotated.x += player.position.x;
			rotated.z += player.position.z;
			
			offset = rotated - player.position;
			
		}else if(Input.GetKey(KeyCode.E)) {
			Vector3 rotated = transform.position;
			
			float x = rotated.x - player.position.x;
			float z = rotated.z - player.position.z;
			float y = rotated.y;
			
			rotated.x = Mathf.Cos(-20)*x - Mathf.Sin(-20)*z;
			rotated.z = Mathf.Sin(-20)*x + Mathf.Cos(-20)*z;
			rotated.y = y;
			
			rotated.x += player.position.x;
			rotated.z += player.position.z;
			
			offset = rotated - player.position;
		}




		// The new position of the camera
		Vector3 newPos = player.position + offset;
		
		// The abovePos is directly above the player at the same distance as the standard position.
		Vector3 abovePos = player.position + Vector3.up * offset.magnitude;

		// Lerp the camera's position between it's current position and it's new position.
		transform.position = Vector3.Lerp(transform.position, newPos, smooth * Time.deltaTime);
		
		// Make sure the camera is looking at the player.
		SmoothLookAt();

	}
	*/

	void SmoothLookAt ()
	{
		// Create a vector from the camera towards the player.
		Vector3 relPlayerPosition = player.position - transform.position;
		
		// Create a rotation based on the relative position of the player being the forward vector.
		Quaternion lookAtRotation = Quaternion.LookRotation(relPlayerPosition, Vector3.up);
		
		// Lerp the camera's rotation between it's current rotation and the rotation that looks at the player.
		transform.rotation = Quaternion.Lerp(transform.rotation, lookAtRotation, smooth * Time.deltaTime);
	}
	
}
