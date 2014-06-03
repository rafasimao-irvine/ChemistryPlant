using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

	private float speed = 1f;

	private float moveLimit = 5f;
	
	private TreeController tree;

	// Use this for initialization
	void Start () {
		tree = GameObject.Find("Tree").GetComponentInChildren<TreeController>();
	}
	
	// Update is called once per frame
	void Update () {
		if(tree.getTreePhase()>1){
			if(moveLimit > 1f){
				moveLimit -= speed*Time.deltaTime;
				Vector3 temp = transform.position;
				temp.y -= speed*Time.deltaTime;
				transform.position = temp;
			}
		}
	}
}
