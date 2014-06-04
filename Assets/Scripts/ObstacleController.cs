using UnityEngine;
using System.Collections;

public class ObstacleController : MonoBehaviour {

	private PlayerController playerController;
	private GUIController guiController;

	public int neededElement;

	public GameObject defeatedEffect;

	private string errorMessage = "You dont have the right element!";

	// Use this for initialization
	void Start () {
		playerController = (PlayerController) GameObject.FindGameObjectWithTag("Player").
			GetComponent(typeof(PlayerController));
		guiController = GameObject.Find("GUI").GetComponentInChildren<GUIController>();
	}
	
	// Update is called once per frame
	void Update () {
		if (playerController.isPlayerNear(transform.position)) {
			if (Input.GetKeyUp(KeyCode.E)) {
				if(!guiController.isShowMessage()){
					if (playerCanDefeatMe())
					{
						playerController.element = -1;
						Instantiate(defeatedEffect, transform.position, transform.rotation);
						Destroy(gameObject);
					}else{
						guiController.showMessage(errorMessage);
					}
				}
			}
		}
	}

	bool playerCanDefeatMe()
	{
		if(playerController.element == neededElement) {
			return true;
		}
		return false;
	}


}
