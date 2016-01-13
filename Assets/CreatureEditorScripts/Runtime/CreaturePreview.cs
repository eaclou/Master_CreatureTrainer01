using UnityEngine;
using System.Collections;

public class CreaturePreview : MonoBehaviour {

	//public CreatureGenome creatureGenome;
	//public Creature currentCreature;
	//private GameObject creatureGO;
	
	//public bool genomeExists = false;
	//public bool creatureExists = false;
	
	void Awake() {
		Debug.Log ("CreatureConstructor Awake()");
	}
	
	public void ResetGenomeToRoot() {
		Debug.Log ("CreatureConstructor ResetGenomeToRoot()");
		//if(genomeExists) {
		//	DeleteCreatureInstances();
		//	DeleteCurrentGenome();
		//}
		//creatureGenome = new CreatureGenome();
		//genomeExists = true;
		
		//if(!creatureExists) { 
		//	CreateBlankCreature();
		//	creatureExists = true;
		//}
		
		//UpdateCreatureFromGenome();
	}
	
	
	public void DeleteCreatureInstances() {
		
	}
	
	public void DeleteCurrentGenome() {
		
	}
	
	public void UpdateCreatureFromGenome() {
		//currentCreature.UpdateFromGenome(creatureGenome);
	}
	
	public void CreateBlankCreature() {
		//creatureGO = new GameObject("creatureGO");
		//currentCreature = creatureGO.AddComponent<Creature>();
	}
}
