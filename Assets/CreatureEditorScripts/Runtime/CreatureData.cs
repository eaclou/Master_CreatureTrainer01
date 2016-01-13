using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;


public class CreatureData : ScriptableObject {

	public float testFloat = 0f;
	public List<CreatureSegmentNode> nodeList;

	public void OnEnable() {
		Debug.Log ("CreatureData OnEnable()");
		//hideFlags = HideFlags.HideAndDontSave; // treats as a root object to get around Unity Garbage Collection?
		if(nodeList == null) {
			nodeList = new List<CreatureSegmentNode>();
		}
	}

	//public void CreateNewSegment(int parentID, int newID) {
		// Create RootNode
		//CreatureSegmentNode newSegment = CreateInstance<CreatureSegmentNode>();
		//newSegment.InitBase(CreatureEditor.mousePos / CreatureEditorWindow.editorWindow._zoom);  // Added to main creatureData nodeList inside function
		//newSegment.id = newID;
	//}
}
