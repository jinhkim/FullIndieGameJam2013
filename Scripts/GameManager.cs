using UnityEngine;
using System.Collections;

//this script defines the classes and global vars for the game

public class GameManager : MonoBehaviour {

	BaseMinion[] minions;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this);
		
		minions = (BaseMinion[])GameObject.FindObjectsOfType(typeof(BaseMinion));
		if(minions == null)
			Debug.Log("GameManager.Start(): Failed to get all BaseMinion types into an array");
	}
	//green slow
	//purple mid
	//blue fast
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)){
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			
			if(Physics.Raycast(ray, out hit)){
				BaseMinion minion = hit.collider.gameObject.GetComponent<BaseMinion>();
				if(minion != null){
					for(int i = 0; i < minions.Length; i++){
						if(minions[i].minionName.Equals(minion.minionName)){
							minions[i].isActive = true;
//							Debug.Log (minions[i].minionName + " has been chosen!!");
							minions[i].selected();
						}
						else
							minions[i].isActive = false;
					}
				}
			}
		}
	}

	//public class Minion {}
}
