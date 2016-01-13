using UnityEngine;
using UnityEditor;
using System.Collections;

[System.Serializable]
public class CreatureSegmentNode {

	public int id;
	public int parentID = -1;
	public Rect windowRect;
	public bool isEdit = false;

	public Vector3 size = new Vector3(0.25f, 0.25f, 0.25f);
	public Vector3 centerPos;	
	
	public Vector3 attachPointParent = new Vector3(0f, 0f, 0f);
	public Vector3 attachPointChild = new Vector3(0f, 0f, 0f);
	
	public ParentAttachAxis parentAttachAxis;
	public enum ParentAttachAxis {
		xPos,
		yPos,
		zPos,
		xNeg,
		yNeg,
		zNeg
	};
	
	public JointPresetType jointPresetType;
	public enum JointPresetType {
		Fixed,
		HingeX,
		HingeY,
		HingeZ,
		DualXY,
		DualYZ,
		DualXZ,
		Full
	};

	public NodeAddOns nodeAddOn1;
	public NodeAddOns nodeAddOn2;
	public enum NodeAddOns {
		None,
		ContactSensor,
		CompassSensor1D,
		CompassSensor3D,
		RangeSensor,
		JetEffector,
		GrabEffector
	};
	
	public Vector3 jointLimitsMin = new Vector3(-60f, -60f, -60f);
	public Vector3 jointLimitsMax = new Vector3(60f, 60f, 60f);
	public float jointSpeed = 100f;
	public float jointStrength = 1000f;

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

	public void CopyDataFromNode(CreatureSegmentNode sourceNode) {
		this.id = sourceNode.id; // copy over values -- eventually make this a method!
		this.parentID = sourceNode.parentID;
		this.size = sourceNode.size;
		this.windowRect = sourceNode.windowRect;
		this.centerPos = sourceNode.centerPos;
		this.attachPointParent = sourceNode.attachPointParent;
		this.attachPointChild = sourceNode.attachPointChild;
		this.parentAttachAxis = sourceNode.parentAttachAxis;
		this.jointPresetType = sourceNode.jointPresetType;
		this.jointLimitsMin = sourceNode.jointLimitsMin;
		this.jointLimitsMax = sourceNode.jointLimitsMax;
		this.jointSpeed = sourceNode.jointSpeed;
		this.jointStrength = sourceNode.jointStrength;
		this.nodeAddOn1 = sourceNode.nodeAddOn1;
		this.nodeAddOn2 = sourceNode.nodeAddOn2;
	}

	public void NodeGUI () 
	{
		if(parentID != -1) {
			GUILayout.Label ("In: " + parentID.ToString()); 
		}
		else {
			GUILayout.Label ("ROOT");
		}

		if(!isEdit) {
			if (GUILayout.Button (new GUIContent ("Expand For Edit"), CreatureEditor.nodeButton)) {
				ExpandGUI();
			}
		}
		else {
			if (GUILayout.Button (new GUIContent ("Compress"), CreatureEditor.nodeButton)) {
				CompressGUI();
			}
			size = EditorGUILayout.Vector3Field("Size:", size);
			jointPresetType = (JointPresetType) EditorGUILayout.EnumPopup ("Joint Preset Type", jointPresetType);
			parentAttachAxis = (ParentAttachAxis) EditorGUILayout.EnumPopup ("Parent Attach Axis", parentAttachAxis);
			
			attachPointParent = EditorGUILayout.Vector3Field ("Attach Point Parent", attachPointParent);
			attachPointChild = EditorGUILayout.Vector3Field ("Attach Point Child", attachPointChild);
			
			jointLimitsMin = EditorGUILayout.Vector3Field ("Joint Limit Min", jointLimitsMin);
			jointLimitsMax = EditorGUILayout.Vector3Field ("Joint Limit Max", jointLimitsMax);
			jointSpeed = EditorGUILayout.FloatField ("Joint Speed", jointSpeed);
			jointStrength = EditorGUILayout.FloatField ("Joint Strength", jointStrength);
			nodeAddOn1 = (NodeAddOns) EditorGUILayout.EnumPopup ("Node Add-On 1", nodeAddOn1);
			nodeAddOn2 = (NodeAddOns) EditorGUILayout.EnumPopup ("Node Add-On 2", nodeAddOn2);
			GUILayout.Space (15);
		}		
		//GUILayout.Space (15);
		/*if (GUILayout.Button (new GUIContent ("Add Segment"), CreatureEditor.nodeButton)) {
			CreatureEditor.newNodeMode = true;
			//CreatureEditorWindow._editorWindow.selectedNode = id;
		}*/
	}

	private void CompressGUI() {
		isEdit = false;
		//float oldHeight = windowRect.height;
		//float oldPosY = windowRect.y;
		windowRect.height = 100f; // compress windowHeight
		//windowRect.y = oldPosY - (windowRect.height / 2f - oldHeight / 2f);  // adjust position to keep centered
	}

	private void ExpandGUI() {
		isEdit = true;
	}

	public void DrawCurves() {
		if(parentID != -1) {
			//Rect rect = windowRect;
			//rect.x += connectedParentNodeRect.x;
			//rect.y += connectedParentNodeRect.y + connectedParentNodeRect.height/2;  // make sure this should be 2
			//rect.width = 1;
			//rect.height = 1;
			
			//CreatureEditorWindow.DrawNodeCurve(CreatureEditorWindow._editorWindow.workingNodeList[parentID].windowRect, windowRect);
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
