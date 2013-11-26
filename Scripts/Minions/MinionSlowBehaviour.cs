using UnityEngine;
using System.Collections;

//name: Stinky
//ability: fart cloud that blocks vision cones (from cameras and drones)
//it does this by either disabling nearby cameras, or intersecting the 
//coneVision mid-way

public class MinionSlowBehaviour : BaseMinion {
	
	public Transform cloud;
	Color fartColor;
	

	// Use this for initialization
	void Start () {
	}
	
	public override void initMinion(){
//		Debug.Log("MinionSlowBehaviour.initMinion Called!");
		minionName = "Stinky";
		speed = 0.05f;
		type = MinionType.SLOW;
		isActive = false;
		pos = new Vector2 (transform.position.x, transform.position.z);
		fartColor = new Color(0.5f, 1.0f, 0.5f, 1f);
		smooth = 3f;
		minionColor = Color.green;
		gameObject.renderer.material.color = minionColor;
	}
	
//	void lerpColors(ColorLerp lerp){
////		Debug.Log("Intersected!!");
//		Mesh mesh = GetComponent<MeshFilter>().mesh;
//		Vector3[] verts = mesh.vertices;
//		Color[] colors = new Color[verts.Length];
//		for(int i = 0; i < verts.Length; i++){
//			color = Color.Lerp(lerp.from, lerp.to, verts[i].x);
//		}
//		mesh.colors = colors;
//	}
//	
//	void changeMaterialColor(Color color){
//		Renderer renderer = GetComponent<Renderer>();
//		renderer.material.color = color;
//	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public override void specialAbility(){
		//inherited special ability
		
		cloud.GetComponent<Renderer>().material.color = fartColor;
		Instantiate (cloud, new Vector3(pos.x, 0, pos.y), new Quaternion(0,0,0,1));
	}
	
	public override void selected(){
//		selectedGlow.GetComponent<Renderer>().material.color = Color.white;
//		Instantiate(selectedGlow, new Vector3(pos.x, 0, pos.y), new Quaternion(0,0,0,1));
		gameObject.renderer.material.color = new Color(0.7f, 1f, 0.7f);
	}
	
	public override void unselected(){
		gameObject.renderer.material.color = minionColor;
	}
}