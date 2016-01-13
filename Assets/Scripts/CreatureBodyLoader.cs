using UnityEngine;
using UnityEditor;
using System.Collections;

public class CreatureBodyLoader {

	public static CreatureBodyLoader loader;

	public static CreatureBodyGenome LoadBodyGenome(string filepath) {
		// Create a Scriptable object to load the data from file:
		CreatureData creatureData = ScriptableObject.CreateInstance<CreatureData>(); // create new container for file data
		// Resources.Load:
		//filepath = "Creature_01";
		//Material mat = Resources.Load ("resourceMat") as Material;
		creatureData = (CreatureData)Resources.Load(filepath) as CreatureData;
		//creatureData = AssetDatabase.LoadAssetAtPath (filepath, typeof(CreatureData)) as CreatureData; // Funnel file data into a new ScriptableObject
		Debug.Log ("CreatureBodyLoader, Filepath: " + filepath.ToString());
		if(creatureData != null) {
			Debug.Log ("CreatureBodyLoader: " + creatureData.nodeList.Count.ToString());
		}
		else {
			Debug.Log ("CreatureBodyLoader: NULL" );
		}

		// Create a BodyGenome:
		CreatureBodyGenome bodyGenome = new CreatureBodyGenome();
		// Copy data from file into agent's body genome:
		for(int i = 0; i < creatureData.nodeList.Count; i++) { // for all nodes in fileData:
			CreatureBodySegmentGenome newBodySegmentGenome = new CreatureBodySegmentGenome();
			newBodySegmentGenome.CopySettingsFromGraphNode(creatureData.nodeList[i]);
			bodyGenome.creatureBodySegmentGenomeList.Add (newBodySegmentGenome);
			//CreatureSegmentNode node = new CreatureSegmentNode(); // create new node to hold data
			//node.CopyDataFromNode(creatureData.nodeList[i]);
			//workingNodeList.Add (node);
		}
		Debug.Log ("CreatureBodyLoader, bodyGenome #nodes: " + bodyGenome.creatureBodySegmentGenomeList.Count.ToString());

		return bodyGenome;
	}
}
