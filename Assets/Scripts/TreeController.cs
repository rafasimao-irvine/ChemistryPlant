using UnityEngine;
using System.Collections;

public class TreeController : MonoBehaviour {

	private PlayerController playerController;

	public GUIText countText;
	public GUIText winText;

	public GameObject oxygen;
	public GameObject[] oxygens;
	private const int MAX_OXYGENS = 3;
	private const float SPAWN_DELAY = 15.0f;
	private float spawnTime = 0.0f;

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

		oxygens = new GameObject[MAX_OXYGENS];

		neededElement = 0;

		countText.text = "Tree phase: "+treePhase.ToString();
		winText.text = "";

		isShowingMessage = false;
	}
	
	// Update is called once per frame
	void Update () {
		// Spawns Oxygen
		if(spawnTime > SPAWN_DELAY){
			spawnTime = 0.0f;
			bool spawn = false;
			for(int i=0;(i<MAX_OXYGENS)&&(!spawn);i++){
				if(oxygens[i]==null){
					Vector3 pos = new Vector3(transform.position.x+Random.Range(1,5),
				    	    	              transform.position.y,
				        	                  transform.position.z+Random.Range(1,5));
					oxygens[i] = (GameObject)Instantiate(oxygen, pos, transform.rotation);
					((Molecule)oxygens[i].GetComponent(typeof(Molecule))).type="O";
					spawn=true;
				}
			}
		}else{
			spawnTime += Time.deltaTime;
			//Debug.Log(spawnTime);
		}

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
			if(neededElement==0)
				neededElement=2;
			else
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
