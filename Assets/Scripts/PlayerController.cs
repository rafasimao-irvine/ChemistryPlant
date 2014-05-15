using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed;
	public GUIText countText;
	public GUIText winText;
	public static int count;

	private GameObject pickedUpObj;

	void Start()
	{
		count = 0;
		countText.text = "Count: "+count.ToString();
		winText.text = "";

		pickedUpObj = null;
	}

	void FixedUpdate () 
	{
		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");

		//Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

		//rigidbody.AddForce(movement * speed * Time.deltaTime);

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		rigidbody.velocity = movement * speed * Time.deltaTime;
		

	}
	
	void Update() 
	{
		if(Input.GetKeyUp(KeyCode.Space)) {
			if(pickedUpObj != null) {
				pickedUpObj.SetActive(true);
				pickedUpObj = null;
			}
		}
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider.tag == "PickUp") {
			if(pickedUpObj == null) {
				pickedUpObj = collider.gameObject;
				pickedUpObj.SetActive(false);
			}
		}
	}

	/*
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "PickUp")
		{
			other.gameObject.SetActive(false);
			count++;
			SetCountText();
		}
	}
	*/

	void SetCountText() 
	{
		countText.text = "Count: "+count.ToString();
		if(count >= 15)
			winText.text = "YOU WIN!!!!";
	}


}
