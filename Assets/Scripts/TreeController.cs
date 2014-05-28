using UnityEngine;
using System.Collections;

public class TreeController : MonoBehaviour {

	private PlayerController playerController;

	public GUIText countText;
	public GUIText winText;

	public GameObject oxygen;

	private int treeWinPhase = 3;
	private int treePhase = 1;

	private int neededElement;

	private string errorMessage = "The tree needs ";
	private string growMessage = "Congratulations! You just grown your three!!";
	private bool isShowingMessage;
	private string showingMessage;

	// Use this for initialization
	void Start () {
		playerController = (PlayerController) GameObject.FindGameObjectWithTag("Player").
			GetComponent(typeof(PlayerController));

		neededElement = 0;

		countText.text = "Tree phase: "+treePhase.ToString();
		winText.text = "";

		isShowingMessage = false;
	}
	
	// Update is called once per frame
	void Update () {
		// Spawns Oxygen

		// Verify player interaction
		if (playerController.isPlayerNear(transform.position)) {
			if (Input.GetKeyUp(KeyCode.E)) {
				if(!isShowingMessage){
					if (playerCanFeedMe())
					{
						playerController.element = -1;
						grow();

						showingMessage = growMessage;
					}else{
						showingMessage = errorMessage+playerController.getPossibleElements()[neededElement];
					}
				}
				isShowingMessage = !isShowingMessage;
			}
		}else{
			isShowingMessage = false;
		}
	}

	bool playerCanFeedMe()
	{
		if(playerController.element == neededElement) {
			neededElement++;
			return true;
		}
		return false;
	}

	void grow(){
		treePhase++;
		transform.localScale = new Vector3(transform.localScale.x,transform.localScale.y+3,transform.localScale.z);
		setTreePhaseText();
	}

	void setTreePhaseText() 
	{
		countText.text = "Tree phase: "+treePhase.ToString();
		if (treePhase > treeWinPhase)
			winText.text = "YOU WIN!!!!!!!";
	}


	void OnGUI()
	{
		if(isShowingMessage) {
			GUI.Box(new Rect(500,160,280,30),showingMessage);
		}
	}

}
