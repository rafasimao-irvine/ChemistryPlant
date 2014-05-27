using UnityEngine;
using System.Collections;

public class TreeController : MonoBehaviour {

	private PlayerController playerController;

	public GUIText countText;
	public GUIText winText;

	private int treeWinPhase = 3;
	private int treePhase = 1;

	// Use this for initialization
	void Start () {
		playerController = (PlayerController) GameObject.FindGameObjectWithTag("Player").
			GetComponent(typeof(PlayerController));

		countText.text = "Tree phase: "+treePhase.ToString();
		winText.text = "";
	}
	
	// Update is called once per frame
	void Update () {
		if (playerController.isPlayerNear(transform.position)) {
			if (Input.GetKeyUp(KeyCode.E)) {
				if (playerCanFeedMe())
				{
					//playerController.usePlayerMolecules();
					grow();
				}
			}
		}
	}

	bool playerCanFeedMe()
	{
		return true;
	}

	void grow(){
		treePhase++;
		setTreePhaseText();
	}

	void setTreePhaseText() 
	{
		countText.text = "Tree phase: "+treePhase.ToString();
		if (treePhase > treeWinPhase)
			winText.text = "YOU WIN!!!!!!!";
	}
}
