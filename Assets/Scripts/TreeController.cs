using UnityEngine;
using System.Collections;

public class TreeController : MonoBehaviour {

	private PlayerController playerController;
	private GUIController guiController;

	public GameObject growEffect;

	public GameObject oxygen;
	public GameObject[] oxygens;
	private int maxOxygens = 1;
	private const int MAX_OXYGENS = 5;
	private const float SPAWN_DELAY = 15.0f;
	private float spawnTime = 0.0f;

	private int treeWinPhase = 3;
	private int treePhase = 1;

	private int neededElement;

	private string errorMessage = "The tree needs ";
	private string growMessage = "Congratulations! You just grown your three!!";

	// Use this for initialization
	void Start () {
		playerController = (PlayerController) GameObject.FindGameObjectWithTag("Player").
			GetComponent(typeof(PlayerController));
		guiController = GameObject.Find("GUI").GetComponentInChildren<GUIController>();

		oxygens = new GameObject[MAX_OXYGENS];

		neededElement = 0;

	}
	
	// Update is called once per frame
	void Update () {
		// Spawns Oxygen
		if(spawnTime > SPAWN_DELAY){
			spawnTime = 0.0f;
			bool spawn = false;
			for(int i=0;(i<maxOxygens)&&(!spawn);i++){
				if(oxygens[i]==null){
					Vector3 pos = new Vector3(transform.position.x+Random.Range(1,5),
					                          transform.position.y+0.5084908f,
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
				if(!guiController.isShowMessage()){
					if (playerCanFeedMe())
					{
						playerController.element = -1;
						grow();

						guiController.showMessage(growMessage);
					}else{
						guiController.showMessage(errorMessage+playerController.
						                          getPossibleElements()[neededElement]);
					}
				}
			}
		}
	}

	bool playerCanFeedMe()
	{
		if(playerController.element == neededElement) {
			if(neededElement==0)
				neededElement=2;
			else
				neededElement++;

			maxOxygens++;
			return true;
		}
		return false;
	}

	void grow(){
		treePhase++;
		transform.localScale = new Vector3(transform.localScale.x+0.2f,
		                                   transform.localScale.y+0.2f,
		                                   transform.localScale.z+0.2f);
		guiController.setTreePhaseText(treePhase, treeWinPhase);

		Instantiate(growEffect, transform.position, transform.rotation);
	}

	public int getTreePhase(){
		return treePhase;
	}

}
