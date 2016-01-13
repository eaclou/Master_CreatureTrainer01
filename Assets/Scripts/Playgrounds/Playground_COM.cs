using UnityEngine;
using System.Collections;

public class Playground_COM : MonoBehaviour {

	public Playground_Critter5B critter;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 com_pos = critter.CritterCOM;
		this.gameObject.transform.position = com_pos;
	}
}
