using UnityEngine;
using System.Collections;

public class Molecule : MonoBehaviour {

	public string type;
	protected float range = 1.0f;
	protected float orientation = 0.12f;

	protected float moved = 0.0f;

	// Update is called once per frame
	void Update () {
		if (moved > range) {
			moved = 0;
			orientation = -orientation;
		
		} else moved += Time.deltaTime;

		transform.Translate(orientation * Vector3.up * Time.deltaTime);
	}
}
