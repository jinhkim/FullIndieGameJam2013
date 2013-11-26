using UnityEngine;
using System.Collections;

public class MinionSlowTest : MinionSlowBehaviour {

	public enum EntityType{
		PLAYER,
		ENEMY_CAMERA,
		ENEMY_TURRET
	};

	float deltaX = 0f, deltaZ = 0f, shiftSpeed = 0;
	EntityType unitType;
	Color color;	
	
	// Use this for initialization
	void Start () {
		base.initMinion();
		if(gameObject.name.Equals("MainCamera")){
			unitType = EntityType.ENEMY_CAMERA;
		}
		else if(gameObject.name.Equals("MinionSlow")){
			unitType = EntityType.PLAYER;
		}
		else {
			Debug.Log("MinionSlowTest.cs: No proper name found!");
		}
		shiftSpeed = speed * 2;
	}
	
	// Update is called once per frame
	void Update () {
		
		if(Input.anyKey){
			if(Input.GetKey(KeyCode.LeftShift)){
				if (Input.GetKey (KeyCode.W)) {
					deltaZ = shiftSpeed;
				}
				else if(Input.GetKey(KeyCode.S)){
					deltaZ = -shiftSpeed;
				}
				else {
					deltaZ = 0f;
				}
				
				if (Input.GetKey (KeyCode.D)) {
					deltaX = shiftSpeed;
				}
				else if(Input.GetKey(KeyCode.A)){
					deltaX = -shiftSpeed;
				}
				else{
					deltaX = 0f;
				}
			}
			else {
				if (Input.GetKey (KeyCode.W)) {
					deltaZ = speed;
				}
				else if(Input.GetKey(KeyCode.S)){
					deltaZ = -speed;
				}
				else {
					deltaZ = 0f;
				}
				
				if (Input.GetKey (KeyCode.D)) {
					deltaX = speed;
				}
				else if(Input.GetKey(KeyCode.A)){
					deltaX = -speed;
				}
				else{
					deltaX = 0f;
				}
			}
			
		}
		else {
			deltaX = deltaZ = 0f;
		}
		
		if(Input.GetMouseButtonDown(1)){
			if(unitType == EntityType.PLAYER)
				base.specialAbility();
		}
		
		this.pos.x += deltaX;
		this.pos.y += deltaZ;
		
		switch(unitType){
		case EntityType.ENEMY_CAMERA:{
			transform.Translate (new Vector3(deltaX,deltaZ,0f));
			break;
		}
		case EntityType.PLAYER:{
			transform.Translate (new Vector3(deltaX,0f,deltaZ));
			break;
		}
		}
	}
}
