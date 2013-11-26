using UnityEngine;
using System.Collections;

public class Crate : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay (Collider other) {
		if (other.tag == "Player" && other.GetComponent<BaseMinion>().type == MinionType.MEDIUM) {
			// shove the crate in the direction of the collision
			Vector3 prevPos = transform.position;
			Vector3 displace = transform.position - other.gameObject.transform.position;

			Debug.Log (displace.magnitude);// == PlayerPathfinding.TILE_SIZE);

			// if adjacent and we're not pushing into a wall
			if(displace.magnitude == PlayerPathfinding.TILE_SIZE && PlayerPathfinding._gameMap != null && PlayerPathfinding._gameMap.isClear(transform.position + displace)) {
				transform.position += displace;
				PlayerPathfinding._gameMap.moveWall(prevPos, transform.position);
			}

		}
	}

}
