﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CraftingTable : MonoBehaviour {

	private bool isCraftingTableOn = false;

	private PlayerController playerController;

	private int selectionGridInt;
	private string[] selectionStrings;

	// Use this for initialization
	void Start () {
		playerController = (PlayerController) GameObject.FindGameObjectWithTag("Player").
			GetComponent(typeof(PlayerController));

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
		}
	}

	void OnGUI() {
		if(isCraftingTableOn) {
			Rect windowRect = new Rect(Screen.width/5.0f,Screen.height/8.0f,650,400);
			selectionGridInt = GUI.SelectionGrid (new Rect (windowRect.x+200, windowRect.y+80, 250, 200), 
			                                      selectionGridInt, selectionStrings, 2);
			if (GUI.Button (new Rect (windowRect.x+480, windowRect.y+120, 80, 80), "Craft")) {
				Dictionary<string, int> molecules = getMoleculesWithStr(
					selectionStrings[selectionGridInt].ToCharArray());
				if (playerController.hasMolecules(molecules)) {
					playerController.usePlayerMolecules(molecules);
					playerController.element = selectionGridInt;
				}
			}

			//GUI.Window (0, windowRect, null, "Crafiting Table");
		}
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
