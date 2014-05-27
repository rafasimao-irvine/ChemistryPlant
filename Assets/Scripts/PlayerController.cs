using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	public float speed;

	private Dictionary<string, int> pickedUpMolecules;

	private float minNeededDistance = 3.0f;

	// On gui:
	private bool isEncyclopediaOn;

	void Start()
	{
		isEncyclopediaOn = false;

		pickedUpMolecules = new Dictionary<string, int>();
		initPickedUpMoleculesValues();
	}

	void initPickedUpMoleculesValues()
	{
		pickedUpMolecules.Add("H",0);
		pickedUpMolecules.Add("O",0);
		pickedUpMolecules.Add("C",0);
		pickedUpMolecules.Add("N",0);
		pickedUpMolecules.Add("K",0);
		pickedUpMolecules.Add("S",0);
		pickedUpMolecules.Add("Mg",0);
	}

	void Update () 
	{
		if (Input.GetKeyUp(KeyCode.H)) {
			isEncyclopediaOn = !isEncyclopediaOn;
		}
	}

	void FixedUpdate () 
	{
		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input   .GetAxis("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		rigidbody.velocity = movement * speed * Time.deltaTime;		
		//rigidbody.AddForce(movement * speed * Time.deltaTime);

	}
	
	/*
	void Update() 
	{
		if(Input.GetKeyUp(KeyCode.Space)) {
			if(pickedUpObj != null) {
				pickedUpObj.SetActive(true);
				pickedUpObj.transform.position = transform.position;
				pickedUpObj.transform.position += pickedUpObj.transform.forward*1.5f;
				pickedUpObj = null;
			}
		}
	}*/

	void OnTriggerEnter(Collider collider)
	{
		if (collider.tag == "PickUp") {
			string moleculeType = ((Molecule) collider.gameObject.GetComponent(typeof(Molecule))).type;

			if (pickedUpMolecules.ContainsKey(moleculeType)) {
				pickedUpMolecules[moleculeType]++;
			}

			Destroy(collider.gameObject);
		}
	}


	// Public methods: =========================================

	public bool isPlayerNear(Vector3 otherPosition) 
	{
		float dist = Vector3.Distance(otherPosition, transform.position);
		if (dist < minNeededDistance)
			return true;

		return false;
	}

	

	public bool hasMolecules(string name, int number)
	{
		if (pickedUpMolecules.ContainsKey(name)) {
			if(pickedUpMolecules[name] >= number)
				return true;
		}
		return false;
	}

	public bool hasMolecules(Dictionary<string, int> elements)
	{
		bool result = true;
		foreach(KeyValuePair<string, int> element in elements) {
			if (!hasMolecules(element.Key,element.Value))
				result = false;
		}

		return result;
	}

	public void usePlayerMolecules(string name, int number)
	{
		if (pickedUpMolecules.ContainsKey(name)) {
			if(pickedUpMolecules[name] >= number)
				pickedUpMolecules[name] -= number;
		}
	}

	public void usePlayerMolecules(Dictionary<string, int> elements)
	{
		if (hasMolecules(elements)) {
			foreach(KeyValuePair<string, int> element in elements) {
				usePlayerMolecules(element.Key, element.Value);
			}
		}
	}

	// GUI ----------------------------------------------------------------------------

	void OnGUI() 
	{
		showInventory();

		if (isEncyclopediaOn) {
			showEncyclopedia();
		}
	}

	void showInventory()
	{
		float inventoryX = 360.0f;
		float inventoryY = 520.0f;

		foreach(KeyValuePair<string, int> element in pickedUpMolecules)
		{
			showInventoryElement(inventoryX, inventoryY, element.Key, element.Value);
			inventoryX += 80.0f;
		}
	}

	void showInventoryElement(float x, float y, string name, int number)
	{
		GUI.Box(new Rect(x,y,70,70),name);
		GUI.Box(new Rect(x+40,y+40,30,30), number.ToString());
	}

	void showEncyclopedia()
	{
		Rect windowRect = GUI.Window (0, new Rect(Screen.width/6.0f,Screen.height/13.0f,900,520), 
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
		                        "C55H72O5N4Mg","Chlorophyll A",
		                        "A chlorin pigment.\n" +
		                        "Commonly found in the chloroplasts of algae and plants.\n" +
		                        "Critical in photosynthesis, which allows plants to absorb light and convert it into energy.\n" +
		                        "Green in color.");
		showEncyclopediaElement(elementsX, windowRect.y+270,
		                        "CON2H4","Urea",
		                        "Also known as carbamide.\n" +
		                        "Widely used in fertilizers as a common/convenient source of nitrogen.");
		showEncyclopediaElement(elementsX, windowRect.y+330,
		                        "K2SO4","Potassium sulfate",
		                        "Also known as arcanite.\n" +
		                        "A white crystalline salt, commonly used in fertilizers to provide potassium and sulfur.");
		showEncyclopediaElement(elementsX, windowRect.y+390,
		                        "O2","Oxygen",
		                        "Naturally found as a gas.\n" +
		                        "Released by plants during photosynthesis and used by organisms during respiration.");
		showEncyclopediaElement(elementsX, windowRect.y+450,
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
