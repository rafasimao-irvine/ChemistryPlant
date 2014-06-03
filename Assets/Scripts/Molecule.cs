using UnityEngine;
using System.Collections;

public class Molecule : MonoBehaviour {

	public string type;
	protected float range = 1.0f;
	protected float orientation = 0.12f;

	protected float moved = 0.0f;

	void Start() {
		if(type != null){
			if(type=="H")
				gameObject.renderer.material.color = Color.white;
			else if(type=="O")
				gameObject.renderer.material.color = Color.blue;
			else if(type=="C")
				gameObject.renderer.material.color = Color.black;
			else if(type=="N")
				gameObject.renderer.material.color = Color.magenta;
			else if(type=="K")
				gameObject.renderer.material.color = Color.green;
			else if(type=="S")
				gameObject.renderer.material.color = Color.red;
		}
	}

	// Update is called once per frame
	void Update () {
		if (moved > range) {
			moved = 0;
			orientation = -orientation;

		} else moved += Time.deltaTime;

		transform.Translate(orientation * Vector3.up * Time.deltaTime);
	}
}
