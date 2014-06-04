using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CraftingTable : MonoBehaviour {

	public GameObject smokeEffect;

	private bool isCraftingTableOn = false;
	private bool isOverridingElement = false;
	private string overridingMessage = "You are carrying an element!\n" +
		"Are you sure you want to craft a new element?\n" +
		"This will make you lose the element you have!!";

	private PlayerController playerController;
	private GUIController guiController;

	private int selectionGridInt;
	private string[] selectionStrings;

	// Use this for initialization
	void Start () {
		playerController = (PlayerController) GameObject.FindGameObjectWithTag("Player").
			GetComponent(typeof(PlayerController));
		guiController = GameObject.Find("GUI").GetComponentInChildren<GUIController>();

		selectionGridInt = 0;
		selectionStrings = new string[]{"H2O", "CO2", "C6O12H6", "CON2H4", "K2SO4", "O2", "NH3"};
	}
	
	// Update is called once per frame
	void Update () {
		if (playerController.isPlayerNear(transform.position)) {
			if (Input.GetKeyUp(KeyCode.E)) {
				isCraftingTableOn = !isCraftingTableOn;
			}
		}else{
			isCraftingTableOn = false;
			isOverridingElement = false;
		}
		guiController.lockMessages = isCraftingTableOn;
	}

	void OnGUI() {

		if(isOverridingElement) {
			float x = Screen.width/2.0f;
			float y = Screen.height/2.0f;
			GUI.Box(new Rect(x-180,y-80,360,70),overridingMessage);
			if (GUI.Button (new Rect (x-60,y, 50, 50), "Craft")) {
				Dictionary<string, int> molecules = getMoleculesWithStr(
					selectionStrings[selectionGridInt].ToCharArray());
				createNewElement(molecules);
				
				isOverridingElement =false;
				isCraftingTableOn = false;
			}
			if (GUI.Button (new Rect (x+10, y, 50, 50), "Cancel")) {
				isOverridingElement =false;
				isCraftingTableOn = false;
			}

		}else if(isCraftingTableOn) {
			Rect windowRect = new Rect(Screen.width/5.0f,Screen.height/8.0f,650,400);
			selectionGridInt = GUI.SelectionGrid (new Rect (windowRect.x+200, windowRect.y+80, 250, 200), 
			                                      selectionGridInt, selectionStrings, 2);
			// Craft Button
			if (GUI.Button (new Rect (windowRect.x+480, windowRect.y+120, 80, 80), "Craft")) {
				Dictionary<string, int> molecules = getMoleculesWithStr(
					selectionStrings[selectionGridInt].ToCharArray());
				if (playerController.hasMolecules(molecules)) {
					if (playerController.element == -1)
						createNewElement(molecules);
					else{
						isOverridingElement = true;
					}
				}
			}

			//GUI.Window (0, windowRect, null, "Crafiting Table");
		}
	}
	
	void createNewElement(Dictionary<string, int> molecules){
		playerController.usePlayerMolecules(molecules);
		playerController.element = selectionGridInt;

		//Instantiate the smoke effect
		Vector3 pos = new Vector3(transform.position.x+0.7f,
		                          transform.position.y+3.9f,
		                          transform.position.z-0.8f);
		Instantiate(smokeEffect, pos, transform.rotation);
	}
	
	Dictionary<string, int> getMoleculesWithStr(char[] components)
	{
		Dictionary<string, int> molecules = new Dictionary<string, int>();

		string lastKey = "";
		string lastNumber = "";
		for (int i = 0; i < components.Length; i++) {

			if (char.IsLetter(components[i])) {
				lastKey = components[i].ToString();
				if( ((i+1) >= components.Length) || (char.IsLetter(components[i+1])) ){
					molecules.Add(lastKey, 1);
					//Debug.Log(lastKey+" : "+1);
				}
			}else if(char.IsNumber(components[i])) {
				lastNumber = components[i].ToString();
				while ( ((i+1) < components.Length) && (char.IsNumber(components[i+1])) ) {
					i++;
					lastNumber = lastNumber.Insert(lastNumber.Length,components[i].ToString());
				}
				molecules.Add(lastKey, int.Parse(lastNumber));
				//Debug.Log(lastKey+" : "+lastNumber);
			}
		}

		return molecules;

	}
}
