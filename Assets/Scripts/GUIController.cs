using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIController : MonoBehaviour {

	public GUIText countText;
	public GUIText winText;

	public GUIText pressText;
	public bool isPlayerNearObj;

	private bool isEncyclopediaOn;

	public bool lockMessages;
	private bool isShowingMessage;
	private string showingMessage;
	private float stopShowingTime;
	private float stopShowingDelay = 2f;

	private PlayerController playerController;

	// Use this for initialization
	void Start () {
		//if (guiSkin!= null)
		//	GUI.skin = guiSkin;

		lockMessages = false;

		countText.text = "Tree phase: "+1;
		winText.text = "";
		
		isShowingMessage = false;
		showingMessage = "";

		isEncyclopediaOn = false;
		pressText.text = "";

		playerController = (PlayerController) GameObject.FindGameObjectWithTag("Player").
			GetComponent(typeof(PlayerController));
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp(KeyCode.H)) {
			isEncyclopediaOn = !isEncyclopediaOn;
		}else if(Input.GetKeyUp(KeyCode.E)){
			if(isShowingMessage){
				isShowingMessage = false;
				stopShowingTime = stopShowingDelay;
			}
		}

		if(stopShowingTime>0){
			stopShowingTime -= Time.deltaTime;
		}

		if(!isPlayerNearObj&&isShowingMessage)
			isShowingMessage = false;

		if(!lockMessages&&isPlayerNearObj&&!isShowingMessage)
			pressText.text = "Press E to interact";
		else
			pressText.text = "";

		isPlayerNearObj = false;
	}


	// Public Methods -----------------------------------------------------------------
	public void showMessage(string message)
	{
		isShowingMessage = true;
		showingMessage = message;
	}

	public bool isShowMessage()
	{
		if(stopShowingTime<1)
			return isShowingMessage;
		else
			return true;
	}

	public void setTreePhaseText(int treePhase, int treeWinPhase) 
	{
		countText.text = "Tree phase: "+treePhase.ToString();
		if (treePhase > treeWinPhase)
			winText.text = "YOU WIN!!!!!!!";
	}

	// GUI ----------------------------------------------------------------------------
	
	void OnGUI() 
	{
		showInventory();
		
		if (isEncyclopediaOn) {
			showEncyclopedia();
		}

		if(!lockMessages&&isShowingMessage) {
			GUI.Box(new Rect(500,160,280,30),showingMessage);
		}
	}
	
	void showInventory()
	{
		float inventoryX = 400.0f;
		float inventoryY = 520.0f;
		
		foreach(KeyValuePair<string, int> element in playerController.pickedUpMolecules)
		{
			showInventoryMolecules(inventoryX, inventoryY, element.Key, element.Value);
			inventoryX += 80.0f;
		}
		
		showInventoryElement();
	}
	
	void showInventoryElement()
	{
		if(playerController.element == -1)
			GUI.Box(new Rect(40,460,120,120),"");
		else
			GUI.Box(new Rect(40,460,120,120),playerController.possibleElements[playerController.element]);
	}
	
	void showInventoryMolecules(float x, float y, string name, int number)
	{
		GUI.Box(new Rect(x,y,70,70),name);
		GUI.Box(new Rect(x+40,y+40,30,30), number.ToString());
	}
	
	void showEncyclopedia()
	{
		Rect windowRect = GUI.Window (0, new Rect(Screen.width/6.0f,Screen.height/20.0f,900,470), 
		                              null, "Encyclopaedia");
		float elementsX = windowRect.x+40.0f;
		
		showEncyclopediaElement(elementsX, windowRect.y+30,
		                        "H2O","Water",
		                        "Naturally found as a liquid.\n" +
		                        "Also vital for all known life-forms and is the most widely used solvent.");
		showEncyclopediaElement(elementsX, windowRect.y+90,
		                        "CO2","Carbon Dioxide",
		                        "Naturally found as a gas.\n " +
		                        "Commonly released during the process of respiration by all known life-forms.\n" +
		                        "Also used by plants for photosynthesis as their main way of getting energy.");
		showEncyclopediaElement(elementsX, windowRect.y+150,
		                        "C6O12H6","Glucose",
		                        "Also known as D-glucose, dextrose and grape sugar.\n" +
		                        "It is an important carbohydrate, used as a secondary source of energy.\n" +
		                        "A main product of photosynthesis.");
		showEncyclopediaElement(elementsX, windowRect.y+210,
		                        "CON2H4","Urea",
		                        "Also known as carbamide.\n" +
		                        "Widely used in fertilizers as a common/convenient source of nitrogen.");
		showEncyclopediaElement(elementsX, windowRect.y+270,
		                        "K2SO4","Potassium sulfate",
		                        "Also known as arcanite.\n" +
		                        "A white crystalline salt, commonly used in fertilizers to provide potassium and sulfur.");
		showEncyclopediaElement(elementsX, windowRect.y+330,
		                        "O2","Oxygen",
		                        "Naturally found as a gas.\n" +
		                        "Released by plants during photosynthesis and used by organisms during respiration.");
		showEncyclopediaElement(elementsX, windowRect.y+390,
		                        "NH3","Ammonia",
		                        "Also known as azane.\n" +
		                        "A colorless gas with a pungent smell. A mid-strength base, can be used to neutralize acids.");
	}
	
	void showEncyclopediaElement(float x, float y, string element, string name, string description)
	{
		GUI.Box(new Rect(x,y,110,55),element);
		GUI.Box(new Rect(x+120,y,115,55),name);
		GUI.Box(new Rect(x+245,y,580,55),description);
	}
	
}
