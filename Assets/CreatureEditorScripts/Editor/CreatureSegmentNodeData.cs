using UnityEngine;
using UnityEditor;
using System.Collections;


public class CreatureSegmentNodeData : ScriptableObject {

	public int id;
	public int parentID = -1;
	public Rect windowRect;

	public Vector3 size = new Vector3(0.25f, 0.25f, 0.25f);

	//[SerializeField]
	//public CreatureSegmentNode parentNode;

	//public CreatureSegmentNode Create (Vector2 pos) 
	//{
		//CreatureSegmentNode node = CreateInstance <CreatureSegmentNode> ();

		//node.windowRect = new Rect (pos.x, pos.y, 200, 100); // set GUI window position
		
		//return node;
	//} 

	public void OnEnable() {
		//size = new Vector3(1f, 1f, 1f);
		Debug.Log ("CreatureSegmentNode OnEnable()");
	}

	public void NodeGUI () 
	{
		if(parentID != -1) {
			GUILayout.Label ("In: " + parentID.ToString()); 
		}

		size = EditorGUILayout.Vector3Field("Size:", size);


		GUILayout.Space (10);
		if (GUILayout.Button (new GUIContent ("Add Segment"), CreatureEditor.nodeButton)) {
			CreatureEditor.newNodeMode = true;
			CreatureEditorWindow._editorWindow.selectedNode = id;
		}
	}

	public void DrawCurves() {
		if(parentID != -1) {
			//Rect rect = windowRect;
			//rect.x += connectedParentNodeRect.x;
			//rect.y += connectedParentNodeRect.y + connectedParentNodeRect.height/2;  // make sure this should be 2
			//rect.width = 1;
			//rect.height = 1;
			
			CreatureEditorWindow.DrawNodeCurve(CreatureEditorWindow._editorWindow.workingNodeList[parentID].windowRect, windowRect);
		}
	}
	
	/// <summary>
	/// Init this node. Has to be called when creating a child node
	/// </summary>
	public void InitBase (Vector2 pos) 
	{
		windowRect = new Rect (pos.x, pos.y, 200, 100); // set GUI window position
		//CreatureEditorWindow.creatureData.nodeList.Add (this); // Add this SegmentNode to the creatureData's node list

		
		// From Tutorial Code:
		/*#if UNITY_EDITOR
		if (name == "")
		{
			name = UnityEditor.ObjectNames.NicifyVariableName (GetID);
		}
		if (!String.IsNullOrEmpty(UnityEditor.AssetDatabase.GetAssetPath (NodeEditor.curNodeCanvas)))
		{
			UnityEditor.AssetDatabase.AddObjectToAsset (this, NodeEditor.curNodeCanvas);
			for (int inCnt = 0; inCnt < Inputs.Count; inCnt++) 
				UnityEditor.AssetDatabase.AddObjectToAsset (Inputs [inCnt], this);
			for (int outCnt = 0; outCnt < Outputs.Count; outCnt++) 
				UnityEditor.AssetDatabase.AddObjectToAsset (Outputs [outCnt], this);
			
			UnityEditor.AssetDatabase.ImportAsset (UnityEditor.AssetDatabase.GetAssetPath (NodeEditor.curNodeCanvas));
		}
		#endif
		*/
	}
	
	/// <summary>
	/// Deletes this Node from creatureData. Depends on that.
	/// </summary>
	public void Delete () 
	{
		
		// From Tutorial Code:
		/*
		NodeEditor.curNodeCanvas.nodes.Remove (this);
		for (int outCnt = 0; outCnt < Outputs.Count; outCnt++) 
		{
			NodeOutput output = Outputs [outCnt];
			for (int conCnt = 0; conCnt < output.connections.Count; conCnt++)
				output.connections [outCnt].connection = null;
			DestroyImmediate (output, true);
		}
		for (int inCnt = 0; inCnt < Inputs.Count; inCnt++) 
		{
			NodeInput input = Inputs [inCnt];
			if (input.connection != null)
				input.connection.connections.Remove (input);
			DestroyImmediate (input, true);
		}
		
		DestroyImmediate (this, true);
		
		#if UNITY_EDITOR
		if (!String.IsNullOrEmpty (UnityEditor.AssetDatabase.GetAssetPath (NodeEditor.curNodeCanvas))) 
		{
			UnityEditor.AssetDatabase.ImportAsset (UnityEditor.AssetDatabase.GetAssetPath (NodeEditor.curNodeCanvas));
		}
		#endif
		NodeEditorCallbacks.IssueOnDeleteNode (this);
		*/
	}
}
