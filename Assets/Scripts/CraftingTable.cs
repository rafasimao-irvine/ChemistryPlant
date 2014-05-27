using UnityEngine;
using System.Collections;

public class CraftingTable : MonoBehaviour {

	private bool isCraftingTableOn = false;

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
				isCraftingTableOn = !isCraftingTableOn;
			}
		}else{
			isCraftingTableOn = false;
		}
	}

	void OnGUI() {
		if(isCraftingTableOn) {
			Rect windowRect = GUI.Window (0, new Rect(Screen.width/6.0f,Screen.height/8.0f,850,450), 
			                         null, "Crafiting Table");
			GUI.Box(new Rect(windowRect.x+50,windowRect.y+50,150,350),"Elements");
			GUI.Box(new Rect(windowRect.x+250,windowRect.y+50,500,300),"Crafiting");
		}
	}
}
