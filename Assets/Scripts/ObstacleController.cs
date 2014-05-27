using UnityEngine;
using System.Collections;

public class ObstacleController : MonoBehaviour {

	private PlayerController playerController;

	// Use this for initialization
	void Start () {
		playerController = (PlayerController) GameObject.FindGameObjectWithTag("Player").
			GetComponent(typeof(PlayerController));
	}
	
	// Update is called once per frame
	void Update () {
		if (playerController.isPlayerNear(transform.position)) {
			if (Input.GetKeyUp(KeyCode.E)) {
				if (playerCanDefeatMe())
				{
					//Die action
				}
			}
		}
	}

	bool playerCanDefeatMe()
	{
		return true;
	}


}
