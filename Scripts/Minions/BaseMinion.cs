using UnityEngine;
using System.Collections;

public enum MinionType {
	SLOW,
	MEDIUM,
	FAST
};

public class BaseMinion : MonoBehaviour{
	public MinionType type;
	public bool isActive;
	public float speed;
	public Vector2 pos;
	public string minionName;
	public float smooth;
	public Color minionColor;
	public Transform selectedGlow;
	
	public virtual void initMinion(){
//		Debug.Log("BaseMinion.initMinion() called!");
	}
	//Override this special ability
	public virtual void specialAbility(){
		
	}
	
	public virtual void selected(){
		
	}
	
	public virtual void unselected(){
	
	}
	
	void Start(){}
	void Update(){}
}