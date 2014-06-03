using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	public float speed;
	public float turnSmoothing = 15f;   // A smoothing value for turning the player.
	//public float speedDampTime = 0.1f;  // The damping for the speed parameter

	private Animation anim;
	private bool moving= false;

	public Dictionary<string, int> pickedUpMolecules;

	public int element;
	public string[] possibleElements;

	private float minNeededDistance = 3.0f;

	private GUIController guiController;

	void Start()
	{
		anim = GetComponent<Animation>();
		anim["loop_run_funny"].speed=4.0f;

		element = -1;
		possibleElements = new string[]{"Water", "Carbon Dioxide", "Glucose", "Urea", "Potassium Sulfate", "Oxygen", "Ammonia"};

		pickedUpMolecules = new Dictionary<string, int>();
		initPickedUpMoleculesValues();

		guiController = GameObject.Find("GUI").GetComponentInChildren<GUIController>();
	}

	void initPickedUpMoleculesValues()
	{
		pickedUpMolecules.Add("H",0);
		pickedUpMolecules.Add("O",0);
		pickedUpMolecules.Add("C",0);
		pickedUpMolecules.Add("N",0);
		pickedUpMolecules.Add("K",0);
		pickedUpMolecules.Add("S",0);
	}
	
	void Update () 
	{
		if(Input.GetKeyUp(KeyCode.E)){
			anim.CrossFade("punch_hi_right");
		}
	}


	void FixedUpdate ()
	{
		// Cache the inputs.
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");

		MovementManagement(h, v);
	}
	
	void MovementManagement (float horizontal, float vertical)
	{
		// If there is some axis input...
		if(horizontal != 0f || vertical != 0f)
		{
			// ... set the players rotation and set the speed parameter to 5.5f.
			Rotating(horizontal, vertical);
			Vector3 movement = new Vector3(horizontal,0.0f,vertical);
			transform.position += movement*speed*Time.deltaTime;
			if(!moving){
				anim.CrossFade("loop_run_funny");
				moving = true;
			}
		}else{
			if(moving){
				anim.CrossFade("loop_idle");
				moving = false;
			}
		}
	}

	void Rotating (float horizontal, float vertical)
	{
		// Create a new vector of the horizontal and vertical inputs.
		Vector3 targetDirection = new Vector3(horizontal, 0f, vertical);
		
		// Create a rotation based on this new vector assuming that up is the global y axis.
		Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
		
		// Create a rotation that is an increment closer to the target rotation from the player's rotation.
		Quaternion newRotation = Quaternion.Lerp(rigidbody.rotation, targetRotation, turnSmoothing * Time.deltaTime);
		
		// Change the players rotation to this new rotation.
		rigidbody.MoveRotation(newRotation);
	}

	
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

	public string[] getPossibleElements()
	{
		return possibleElements;
	}


	public bool isPlayerNear(Vector3 otherPosition) 
	{
		float dist = Vector3.Distance(otherPosition, transform.position);
		if (dist < minNeededDistance) {
			guiController.isPlayerNearObj = true;
			return true;
		}

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
	
}
