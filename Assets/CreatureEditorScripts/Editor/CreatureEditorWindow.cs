using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class CreatureEditorWindow : EditorWindow {

	// TRY saving a reference to CreatureEditor or creatureData to prevent re-compile wipe.
	//public CreatureData creatureData;  // the object that stores the genome for the current creature being edited
	//private List<CreatureSegmentNode> nodeList = new List<CreatureSegmentNode>();

	public List<CreatureSegmentNode> workingNodeList = new List<CreatureSegmentNode>();
	public List<GameObject> segmentListGO;


	public static CreatureEditorWindow _editorWindow;
	/*public static CreatureEditorWindow editorWindow
	{
		get
		{
			AssureHasEditor ();
			return _editorWindow;
		}
	}
	// Method: 
	public static void AssureHasEditor () 
	{
		if (_editorWindow == null)
		{
			Init();
			_editorWindow.Repaint ();
		}
	}*/ 

	public string saveName = "Creature_01";
	  
	//public TestZoomWindow window;
	[MenuItem("Custom/CreatureEditor")]
	static void ShowEditor()
	{
		if (_editorWindow == null)
		{
			//Debug.Log ("CreatureEditorWindow Init()");
			_editorWindow = GetWindow<CreatureEditorWindow>("Creature Editor");
			_editorWindow.minSize = new Vector2(600.0f, 300.0f);
			_editorWindow.wantsMouseMove = true;
			_editorWindow.Show();
			FocusWindowIfItsOpen<CreatureEditorWindow>();
			//if(_editorWindow.creatureData == null) {
			//	_editorWindow.ResetCreatureData ();
			//}
		}
		else {
			//Debug.Log ("CreatureEditorWindow EXISTS!!!!!()");
		}
	}

	private const float kZoomMin = 0.1f;
	private const float kZoomMax = 10.0f;
	public bool gravityOn = false;

	private Rect _zoomArea = new Rect(0.0f, 0.0f, Screen.width, Screen.height);
	public float _zoom = 1.0f;
	private Vector2 _zoomCoordsOrigin = Vector2.zero;
	private float scrollSpeed = 1.175f;

	public int selectedNode = 0;
	public int topID = 0;

	// Settings
	public static int sideWindowWidth = 400;
	private Color bgColor = new Color(0.75f, 0.75f, 0.75f);
	private Color rootColor = new Color(1f, 0.8f, 0.75f);
	private Color segmentColor = new Color(0.95f, 0.75f, 0.9f);

	private Vector2 ConvertScreenCoordsToZoomCoords(Vector2 screenCoords)
	{
		return (screenCoords - _zoomArea.TopLeft()) / _zoom + _zoomCoordsOrigin;
	}

	public Rect sideWindowRect
	{
		get { return new Rect (position.width - sideWindowWidth, 0, sideWindowWidth, position.height); }
	}
	public Rect canvasWindowRect
	{
		get { return new Rect (0, 0, position.width - sideWindowWidth, position.height); }
	}
	
	private void DrawZoomArea()
	{
		// Within the zoom area all coordinates are relative to the top left corner of the zoom area
		// with the width and height being scaled versions of the original/unzoomed area's width and height.
		CreatureEditor.BeginZoomArea(_zoom, _zoomArea);
		DrawNodeLinks();
		DrawNodeWindows();		
		CreatureEditor.EndZoomArea();		
	}
	
	private void DrawNonZoomArea()
	{
		// Draw Side Window
		DrawSideWindow ();
	}

	private void DrawNodeLinks() {
		if(workingNodeList != null) {
			foreach(CreatureSegmentNode n in workingNodeList) {
				n.DrawCurves ();
			}
		}
	}

	private void DrawNodeWindows () {
		BeginWindows();
		if(workingNodeList != null) {

			//Debug.Log (creatureData.nodeList[0].id.ToString());
			for(int i = 0; i < workingNodeList.Count; i++) {
				workingNodeList[i].windowRect = GUILayout.Window (i, workingNodeList[i].windowRect, DrawNodeWindow, "Window Title [" + workingNodeList[i].id + "]");
			}

		}
		EndWindows();
	}

	public void DrawSideWindow () 
	{
		sideWindowWidth = Mathf.Min (600, Mathf.Max (300, (int)(position.width / 5)));
		GUI.color = bgColor;
		GUILayout.BeginArea (sideWindowRect, CreatureEditor.nodeBox);

		GUILayout.Label (new GUIContent ("Creature Node Editor (" + ")", "The currently opened canvas in the Node Editor"), CreatureEditor.nodeLabelBold);
		saveName = EditorGUILayout.TextField("Save Name:", saveName);
		if (GUILayout.Button (new GUIContent ("Save Creature", "Saves the current nodes as a new Creature Asset File in the Assets Folder"))) 
		{
			SaveCreatureData("Assets/Resources/" + saveName + ".asset");
			Debug.Log ("saveName = " + saveName);
		}
		if (GUILayout.Button (new GUIContent ("Load Creature", "Loads a Creature from a Canvas Asset File in the Assets Folder"), CreatureEditor.nodeButton)) 
		{
			LoadCreatureData();
		}
		if (GUILayout.Button (new GUIContent ("New Creature", "Creates a new Creature (remember to save the previous one to a referenced Creature Asset File at least once before! Else it'll be lost!)"), CreatureEditor.nodeButton)) 
		{
			ResetCreatureData ();  
		}
		if (GUILayout.Button (new GUIContent ("Rebuild Creature", ""), CreatureEditor.nodeButton)) 
		{
			RebuildCreature();
		}
		GUILayout.Label ("Zoom Scale:");
		_zoom = EditorGUILayout.Slider(_zoom, kZoomMin, kZoomMax);
		//_zoom = EditorGUI.Slider(new Rect(0.0f, 50.0f, 600.0f, 25.0f), _zoom, kZoomMin, kZoomMax);
		gravityOn = EditorGUILayout.Toggle ("Use Gravity", gravityOn);
		 
		GUILayout.EndArea ();
	}
	
	private void HandleEvents()
	{
		Event e = Event.current;
		CreatureEditor.mousePos = e.mousePosition;

		// Allow adjusting the zoom with the mouse wheel as well. In this case, use the mouse coordinates
		// as the zoom center instead of the top left corner of the zoom area. This is achieved by
		// maintaining an origin that is used as offset when drawing any GUI elements in the zoom area.
		if (e.type == EventType.ScrollWheel)
		{
			//Vector2 screenCoordsMousePos = Event.current.mousePosition;
			Vector2 delta = e.delta;
			//Vector2 zoomCoordsMousePos = ConvertScreenCoordsToZoomCoords(screenCoordsMousePos);
			//float zoomDelta = -delta.y / 150.0f;
			//float oldZoom = _zoom;
			//_zoom += zoomDelta;
			//_zoom = Mathf.Clamp(_zoom, kZoomMin, kZoomMax);
			//_zoomCoordsOrigin += (zoomCoordsMousePos - _zoomCoordsOrigin) - (oldZoom / _zoom) * (zoomCoordsMousePos - _zoomCoordsOrigin);
			if(delta.y >= 0 ) {
				_zoom /= scrollSpeed;
			}
			else {
				_zoom *= scrollSpeed;
			}
			e.Use();
		}
		
		// Allow moving the zoom area's origin by dragging with the middle mouse button or dragging
		// with the left mouse button with Alt pressed.
		if (e.type == EventType.MouseDrag &&
		    (e.button == 0 && e.modifiers == EventModifiers.Alt) ||
		    e.button == 2)
		{
			Vector2 delta = e.delta;

			delta /= _zoom; // compensate for zoom
			delta /= 2f;  // when delta was = 1x, windowRect was being moved by 2.... split in half to compensate
			//_zoomCoordsOrigin += delta;
			//windowRect.x += delta.x/2f;  // This function seems to be being called twice?
			//windowRect.y += delta.y/2f;  // 
			for(int i = 0; i < workingNodeList.Count; i++) {
				workingNodeList[i].windowRect.x += delta.x;
				workingNodeList[i].windowRect.y += delta.y;
			}
			
			e.Use();
		}

		if(e.type == EventType.MouseDown) {  // Mouse click

			//if(CreatureEditor.mousePos.x > Screen.width - sideWindowWidth) {
			//	Debug.Log ("MousePos: " + CreatureEditor.mousePos.x.ToString() + ", " + CreatureEditor.mousePos.x.ToString() + "... " + Screen.width.ToString());
			//}
			bool clickedOnWindow = false;
			//int selectIndex = -1;
			// Check if mouse cursor is on a window:
			for(int i = 0; i < workingNodeList.Count; i++) {
				if(workingNodeList[i].windowRect.Contains (CreatureEditor.mousePos / _zoom)) {
					selectedNode = i;
					clickedOnWindow = true;

					break;
				}
			}

			if(e.button == 0) { // left click

				if(CreatureEditor.newNodeMode) {  // if placing a new node

					CreateNewSegmentNode(selectedNode); // get parentID!!
					RebuildCreature();
					Repaint ();
					CreatureEditor.newNodeMode = false;
				}
				else {  // not placing a node -- first click:
					CreatureEditor.newNodeMode = false;

				}
			}
			else if(e.button == 1) {  // Right-Click
				CreatureEditor.newNodeMode = false;

				if(clickedOnWindow) { // clicked on Node window
					GenericMenu menu = new GenericMenu();
					menu.AddItem (new GUIContent("Add New Node"), false, ContextCallback, "addNode");					
					menu.AddSeparator ("");
					if(selectedNode != 0) {
						menu.AddItem (new GUIContent("Duplicate Node"), false, ContextCallback, "duplicateNode");					
						menu.AddSeparator ("");
						menu.AddItem (new GUIContent("Duplicate Branch"), false, ContextCallback, "duplicateBranch");					
						menu.AddSeparator ("");
						menu.AddItem (new GUIContent("Delete Node"), false, ContextCallback, "deleteNode");					
						menu.AddSeparator ("");
						menu.AddItem (new GUIContent("Delete Branch"), false, ContextCallback, "deleteBranch");
					}
					menu.ShowAsContext ();
					e.Use ();
				}
				else { // clicked on blank area
					//GenericMenu menu = new GenericMenu();				
					//menu.AddItem (new GUIContent("Reset To Root Node"), false, ContextCallback, "resetToRootNode");
					//menu.AddSeparator ("");
					//menu.AddItem (new GUIContent("Rebuild Creature"), false, ContextCallback, "rebuildCreature");				
					//menu.ShowAsContext ();
					//e.Use ();
				}
			}
		}

		Debug.Log ("SelectedNodeID: " + selectedNode.ToString());
	}

	void ContextCallback(object obj) {
		string callback = obj.ToString();
		
		if(callback.Equals ("addNode")) {
			// Delete this node
			AddNode(selectedNode);
		}
		else if(callback.Equals ("duplicateNode")) {
			DuplicateNode(selectedNode);
		}
		else if(callback.Equals ("duplicateBranch")) {
			DuplicateBranch(selectedNode);
		}
		else if(callback.Equals ("deleteNode")) {
			DeleteNode(selectedNode);
		}
		else if(callback.Equals ("deleteBranch")) {
			DeleteBranch(selectedNode);
		}
	}

	public void AddNode(int nodeID) {
		CreatureEditor.newNodeMode = true;
	}

	public void DuplicateNode(int nodeID) {

	}

	public void DuplicateBranch(int nodeID) {
		
	}

	public void DeleteNode(int nodeID) {
		int fosterParentID = workingNodeList[nodeID].parentID;
		List<int> orphanNodes = FindDirectChildrenOfNode(nodeID);
		//Debug.Log ("Node#: " + nodeID.ToString() + ", Number of Orphans: " + orphanNodes.Count.ToString() + ", fosterID: " + fosterParentID.ToString());

		for(int i = 0; i < orphanNodes.Count; i++) {
			//Debug.Log ("Node#: " + orphanNodes[i].ToString() + ", parentID: " + workingNodeList[orphanNodes[i]].parentID.ToString());
			workingNodeList[orphanNodes[i]].parentID = fosterParentID;
		}

		workingNodeList.RemoveAt (nodeID); // Might need to rebuild list to handle indexing mismatches

		// Fix List:
		for(int j = 0; j < workingNodeList.Count; j++) {  // list is one member shorter now:
			// Go through all nodes, fix id, parentID
			if(workingNodeList[j].parentID >= nodeID) {  // if this node has a parentID reference affected by the deleted node
				workingNodeList[j].parentID--;  // decrement parentID by one to match index and fill in deleted space
			}
			if(workingNodeList[j].id >= nodeID) {  
				workingNodeList[j].id--;  // decrement ID by one to match its spot in the List index.
			}
			Debug.Log ("WorkingNode# " + j.ToString() + ", ID: " + workingNodeList[j].id.ToString() + ", parentID: " + workingNodeList[j].parentID.ToString());
		}
	}

	public void DeleteBranch(int nodeID) {

	}

	private List<int> FindDirectChildrenOfNode(int nodeID) {
		List<int> childNodes = new List<int>();
		for(int i = 0; i < workingNodeList.Count; i++) {
			if(workingNodeList[i].parentID == nodeID) {
				int id = workingNodeList[i].id;
				//Debug.Log ("Orphan Node#: " + id.ToString());
				childNodes.Add (id);
			}
		}
		return childNodes;
	}

	private List<int> FindAllChildrenOfNode(int nodeID) {
		List<int> childNodes = new List<int>();

		return childNodes;
	}
	
	void DrawNodeWindow(int id) {
		workingNodeList[id].NodeGUI();
		GUI.DragWindow();		
	}
	
	public void OnGUI()
	{
		/*if (nodeList == null)
		{
			Debug.Log ("nodeList == null");
		}
		else {
			Debug.Log ("nodeList != null"); 
		}*/
		//Debug.Log ("nodeList.Count = " + nodeList.Count.ToString()); 
		CreatureEditor.checkInit ();
		CheckForCreatureData();
		ShowEditor();
		//AssureHasEditor ();  
		if (CreatureEditor.InitiationError) 
		{
			GUILayout.Label ("Initiation failed! Check console for more information!");
			return;
		}

		_zoomArea = new Rect(0f, 0f, Screen.width, Screen.height); // update _zoomArea if windowSize has been changed
		HandleEvents();		
		// The zoom area clipping is sometimes not fully confined to the passed in rectangle. At certain
		// zoom levels you will get a line of pixels rendered outside of the passed in area because of
		// floating point imprecision in the scaling. Therefore, it is recommended to draw the zoom
		// area first and then draw everything else so that there is no undesired overlap.
		DrawZoomArea();
		DrawNonZoomArea();

		if (GUI.changed) {
			RebuildCreature();
		}
	}

	private void CheckForCreatureData() {
		/*if(creatureData == null) {
			Debug.Log ("CreatureData is NULL!");
			ResetCreatureData ();
		}
		else {
			if(creatureData.nodeList == null) {
				Debug.Log ("CreatureData NODELIST is NULL!");
				creatureData.nodeList = new List<CreatureSegmentNode>();
			}
		}
		*/
	}

	public void ResetCreatureData () {
		// Delete all current nodes:
		//creatureData.nodeList = new List<CreatureSegmentNode>();
		workingNodeList = new List<CreatureSegmentNode>();
		topID = 0;
		//creatureData = ScriptableObject.CreateInstance<CreatureData>();

		// DO I NEED THIS???:
		//if(EditorPrefs.HasKey ("ObjectPath")) {
		//	string objectPath = EditorPrefs.GetString ("ObjectPath");
		//	creatureData = AssetDatabase.LoadAssetAtPath (objectPath, typeof(CreatureData)) as CreatureData;
		//}

		CreateRootSegmentNode();
	}

	public void SaveCreatureData(string fileName) {
		CreatureData creatureData = ScriptableObject.CreateInstance<CreatureData>(); // create ScriptableObject Asset
		// Copy values from workingData to creatureData asset:
		for(int i = 0; i < workingNodeList.Count; i++) {
			CreatureSegmentNode node = new CreatureSegmentNode(); // create new node to hold data
			node.CopyDataFromNode(workingNodeList[i]);
			//node.id = workingNodeList[i].id; // copy over values -- eventually make this a method!
			//node.parentID = workingNodeList[i].parentID;
			//node.size = workingNodeList[i].size;
			//node.windowRect = workingNodeList[i].windowRect;
			creatureData.nodeList.Add (node);
		}
		AssetDatabase.CreateAsset (creatureData, fileName); // create
		AssetDatabase.SaveAssets ();
		AssetDatabase.Refresh();
		EditorUtility.FocusProjectWindow ();
		Selection.activeObject = creatureData;

		//"Assets/NewCreatureData2.asset"
		//string currentAssetFileName = AssetDatabase.GetAssetPath(creatureData);
		//if(currentAssetFileName == "") {
			//Debug.Log ("creatureData not associated with an asset file");
			//AssetDatabase.CreateAsset (creatureData, fileName); // create
			// it's not correctly saving the individual Nodes -- check how to achieve that
			/*for(int i = 0; i < creatureData.nodeList.Count; i++) {
				CreatureSegmentNode node = creatureData.nodeList[i];
				AssetDatabase.AddObjectToAsset (node, creatureData);
			}*/
		//}
		//else {
			//Debug.Log ("creatureData is connected to an asset file: " + currentAssetFileName);
			//AssetDatabase.DeleteAsset(currentAssetFileName);
			//AssetDatabase.
			//AssetDatabase.CreateAsset (creatureData, fileName); // create
			//AssetDatabase.CopyAsset (currentAssetFileName, fileName); // THIS ISN"T WORKING?
		//}
		//if (System.IO.File.Exists(fileName)) // if file already exists:
		//AssetDatabase.SaveAssets ();
		//AssetDatabase.Refresh();
		//EditorUtility.FocusProjectWindow ();
		//Selection.activeObject = creatureData;
	}

	/* Stripping asset of subObjects:
	// Delete any old sub assets inside the prefab.
	Object[] assets = UnityEditor.AssetDatabase.LoadAllAssetsAtPath(AssetPath);
	for (int i = 0; i < assets.Length; i++)
	{
	    Object asset = assets[i];
	    if (UnityEditor.AssetDatabase.IsMainAsset(asset) || asset is GameObject || asset is Component) continue;
	    else Object.DestroyImmediate(asset, true);
	}
	*/

	public void LoadCreatureData() {
		CreatureData creatureData = ScriptableObject.CreateInstance<CreatureData>(); // create new container for file data
		string absPath = EditorUtility.OpenFilePanel ("Select Creature", "", "");
		if(absPath.StartsWith (Application.dataPath)) {
			string relPath = absPath.Substring (Application.dataPath.Length - "Assets".Length);
			//Debug.Log ("LoadCreatureData() relPath= " + relPath + " --- " + AssetDatabase.GetAssetPath(creatureData));
			creatureData = AssetDatabase.LoadAssetAtPath (relPath, typeof(CreatureData)) as CreatureData; // Funnel file data into a new ScriptableObject
			//if(creatureData) {
				//EditorPrefs.SetString ("ObjectPath", relPath);
				//Debug.Log ("End LoadCreatureData() - NodeCount= " + creatureData.nodeList.Count.ToString());
			//}
		}
		workingNodeList = new List<CreatureSegmentNode>(); // Clear current workingData
		// Copy data into workingData:
		for(int i = 0; i < creatureData.nodeList.Count; i++) {
			CreatureSegmentNode node = new CreatureSegmentNode(); // create new node to hold data
			node.CopyDataFromNode(creatureData.nodeList[i]);
			workingNodeList.Add (node);
		}

		/*string absPath = EditorUtility.OpenFilePanel ("Select Creature", "", "");
		if(absPath.StartsWith (Application.dataPath)) {
			string relPath = absPath.Substring (Application.dataPath.Length - "Assets".Length);
			Debug.Log ("LoadCreatureData() relPath= " + relPath + " --- " + AssetDatabase.GetAssetPath(creatureData));
			creatureData = AssetDatabase.LoadAssetAtPath (relPath, typeof(CreatureData)) as CreatureData;
			if(creatureData) {
				//EditorPrefs.SetString ("ObjectPath", relPath);
				Debug.Log ("End LoadCreatureData() - NodeCount= " + creatureData.nodeList.Count.ToString());
			}
		}*/
	}

	public void CreateRootSegmentNode() {
		// Create RootNode
		//CreatureSegmentNode node = CreateInstance<CreatureSegmentNode>();
		CreatureSegmentNode node = new CreatureSegmentNode();
		node.InitBase(new Vector2(200f / _zoom, 200f / _zoom));  // Added to main creatureData nodeList inside function
		node.id = workingNodeList.Count;
		workingNodeList.Add (node); 
	}

	/// <summary>
	/// Creates and opens a new empty node canvas
	/// </summary>
	public void CreateNewSegmentNode (int parentID) 
	{
		// New CreatureData

		//Debug.Log ("CreatureData DOESN'T exists!");
		//creatureData = CreateInstance<CreatureData> ();
		//creatureData.nodeList = new List<CreatureSegmentNode> ();  // See if cleanup is required on old List

		// Create RootNode
		//CreatureSegmentNode node = CreateInstance<CreatureSegmentNode>();
		CreatureSegmentNode node = new CreatureSegmentNode();
		//node.windowRect = new Rect (200, 200, 200, 100);
		node.InitBase(new Vector2(CreatureEditor.mousePos.x / _zoom, CreatureEditor.mousePos.y / _zoom));  // Added to main creatureData nodeList inside function
		node.id = workingNodeList.Count;
		//node.parentNode = creatureData.nodeList[parentID];
		node.parentID = parentID;
		workingNodeList.Add (node); 
	}

	public static void DrawNodeCurve(Rect start, Rect end) {
		Vector3 startPos = new Vector3(start.x + start.width/2, start.y + start.height/2, 0);
		Vector3 endPos = new Vector3(end.x + end.width/2, end.y + end.height/2, 0);
		Vector3 startTan = startPos + Vector3.right * 50;
		Vector3 endTan = endPos + Vector3.left * 50;
		Color shadowCol = new Color(0, 0, 0, 0.06f);
		for(int i = 0; i < 3; i++) // Draw a shadow, 3 iterations, each one slightly wider
			Handles.DrawBezier (startPos, endPos, startTan, endTan, shadowCol, null, (i + 1) * 5);
		Handles.DrawBezier (startPos, endPos, startTan, endTan, Color.black, null, 1);
	}

	#region RebuildCreature()
	void RebuildCreature() {
		// Delete all children of CreatureGO
		CreaturePreview creatureGO = FindObjectOfType<CreaturePreview>();
		segmentListGO = new List<GameObject>();
		//FindObject
		var children = new List<GameObject>();
		foreach(Transform child in creatureGO.gameObject.transform) children.Add (child.gameObject);
		children.ForEach (child => DestroyImmediate (child));
		
		// Create Creature:
		CreatureSegmentNode parentSegmentNode;
		for(int i = 0; i < workingNodeList.Count; i++) {  // iterate through every node
			if(i == 0) {  // If Root Node:
				//GameObject rootSegment = new GameObject("rootSegment");
				GameObject segmentGO = GameObject.CreatePrimitive(PrimitiveType.Cube);
				segmentListGO.Add (segmentGO);
				segmentListGO[0].name = "RootSegment" + i.ToString();
				segmentListGO[0].transform.parent = creatureGO.gameObject.transform;
				//segmentListGO[0].GetComponent<Collider>().enabled = false;
				segmentListGO[0].AddComponent<Rigidbody>();
				if(!gravityOn) {
					segmentListGO[0].GetComponent<Rigidbody>().useGravity = false;
				}
				workingNodeList[0].centerPos = new Vector3(0f, 0f, 0f); // set root node worldspace position
				segmentListGO[0].transform.localScale = new Vector3(workingNodeList[0].size.x, workingNodeList[0].size.y, workingNodeList[0].size.z);				

			}
			else {
				GameObject segmentGO = GameObject.CreatePrimitive(PrimitiveType.Cube);
				segmentListGO.Add (segmentGO);
				segmentListGO[i].name = "segmentGO" + i.ToString();
				segmentListGO[i].transform.parent = creatureGO.gameObject.transform;
				//segmentListGO[i].GetComponent<Collider>().enabled = false;
				segmentListGO[i].AddComponent<Rigidbody>();
				if(!gravityOn) {
					segmentListGO[i].GetComponent<Rigidbody>().useGravity = false;
				}

				workingNodeList[i].centerPos = GetBlockPosition(workingNodeList[workingNodeList[i].parentID].centerPos, workingNodeList[workingNodeList[i].parentID].size, workingNodeList[i].attachPointParent, workingNodeList[i].attachPointChild, workingNodeList[i].size, workingNodeList[i].parentAttachAxis);
				segmentListGO[i].transform.position = workingNodeList[i].centerPos;
				segmentListGO[i].transform.localScale = new Vector3(workingNodeList[i].size.x, workingNodeList[i].size.y, workingNodeList[i].size.z);

				ConfigurableJoint joint = segmentListGO[i].AddComponent<ConfigurableJoint>();
				joint.connectedBody = segmentListGO[workingNodeList[i].parentID].GetComponent<Rigidbody>();  // Set parent
				joint.anchor = GetJointAnchor(workingNodeList[i]);
				joint.connectedAnchor = GetJointConnectedAnchor(workingNodeList[i]); // <-- Might be Unnecessary
				ConfigureJointSettings(workingNodeList[i], ref joint);				
			}
		}
	}

	void ConfigureJointSettings(CreatureSegmentNode node, ref ConfigurableJoint joint) {
		if(node.jointPresetType == CreatureSegmentNode.JointPresetType.Fixed) { // Fixed Joint
			// Lock mobility:
			joint.xMotion = ConfigurableJointMotion.Locked;
			joint.yMotion = ConfigurableJointMotion.Locked;
			joint.zMotion = ConfigurableJointMotion.Locked;
			joint.angularXMotion = ConfigurableJointMotion.Locked;
			joint.angularYMotion = ConfigurableJointMotion.Locked;
			joint.angularZMotion = ConfigurableJointMotion.Locked;
		}
		else if(node.jointPresetType == CreatureSegmentNode.JointPresetType.HingeX) { // Uni-Axis Hinge Joint
			// Lock mobility:
			joint.xMotion = ConfigurableJointMotion.Locked;
			joint.yMotion = ConfigurableJointMotion.Locked;
			joint.zMotion = ConfigurableJointMotion.Locked;
			joint.angularXMotion = ConfigurableJointMotion.Limited;
			joint.angularYMotion = ConfigurableJointMotion.Locked;
			joint.angularZMotion = ConfigurableJointMotion.Locked;
			// Joint Limits:
			SoftJointLimit limitXMin = new SoftJointLimit();
			limitXMin.limit = node.jointLimitsMin.x;
			joint.lowAngularXLimit = limitXMin;
			SoftJointLimit limitXMax = new SoftJointLimit();
			limitXMax.limit = node.jointLimitsMax.x;
			joint.highAngularXLimit = limitXMax;
		}
		else if(node.jointPresetType == CreatureSegmentNode.JointPresetType.HingeY) { // Uni-Axis Hinge Joint
			// Lock mobility:
			joint.xMotion = ConfigurableJointMotion.Locked;
			joint.yMotion = ConfigurableJointMotion.Locked;
			joint.zMotion = ConfigurableJointMotion.Locked;
			joint.angularXMotion = ConfigurableJointMotion.Locked;
			joint.angularYMotion = ConfigurableJointMotion.Limited;
			joint.angularZMotion = ConfigurableJointMotion.Locked;
			// Joint Limits:
			SoftJointLimit limitY = new SoftJointLimit();
			limitY.limit = node.jointLimitsMax.y;
			joint.angularYLimit = limitY;
		}
		else if(node.jointPresetType == CreatureSegmentNode.JointPresetType.HingeZ) { // Uni-Axis Hinge Joint
			// Lock mobility:
			joint.xMotion = ConfigurableJointMotion.Locked;
			joint.yMotion = ConfigurableJointMotion.Locked;
			joint.zMotion = ConfigurableJointMotion.Locked;
			joint.angularXMotion = ConfigurableJointMotion.Locked;
			joint.angularYMotion = ConfigurableJointMotion.Locked;
			joint.angularZMotion = ConfigurableJointMotion.Limited;
			// Joint Limits:
			SoftJointLimit limitZ = new SoftJointLimit();
			limitZ.limit = node.jointLimitsMax.z;
			joint.angularZLimit = limitZ;
		}
	}

	Vector3 GetJointAnchor(CreatureSegmentNode node) {
		Vector3 retVec = new Vector3(0f, 0f, 0f);

		if(node.parentAttachAxis == CreatureSegmentNode.ParentAttachAxis.xNeg) {  // Neg X
			retVec.x = 0.5f;
			retVec.y = node.attachPointChild.y;
			retVec.z = node.attachPointChild.z;
		}
		else if(node.parentAttachAxis == CreatureSegmentNode.ParentAttachAxis.yNeg) {  // Neg Y
			retVec.x = node.attachPointChild.x;
			retVec.y = 0.5f;
			retVec.z = node.attachPointChild.z;
		}
		else if(node.parentAttachAxis == CreatureSegmentNode.ParentAttachAxis.zNeg) { // Neg Z
			retVec.x = node.attachPointChild.x;
			retVec.y = node.attachPointChild.y;
			retVec.z = 0.5f;
		}
		else if(node.parentAttachAxis == CreatureSegmentNode.ParentAttachAxis.xPos) {  // Pos X
			retVec.x = -0.5f;
			retVec.y = node.attachPointChild.y;
			retVec.z = node.attachPointChild.z;
		}
		else if(node.parentAttachAxis == CreatureSegmentNode.ParentAttachAxis.yPos) {  // Pos Y
			retVec.x = node.attachPointChild.x;
			retVec.y = -0.5f;
			retVec.z = node.attachPointChild.z;
		}
		else if(node.parentAttachAxis == CreatureSegmentNode.ParentAttachAxis.zPos) {  // Pos Z
			retVec.x = node.attachPointChild.x;
			retVec.y = node.attachPointChild.y;
			retVec.z = -0.5f;
		}

		return retVec;
	}

	Vector3 GetJointConnectedAnchor(CreatureSegmentNode node) {
		Vector3 retVec = new Vector3(0f, 0f, 0f);
		
		if(node.parentAttachAxis == CreatureSegmentNode.ParentAttachAxis.xNeg) {  // Neg X
			retVec.x = -0.5f;
			retVec.y = node.attachPointParent.y;
			retVec.z = node.attachPointParent.z;
		}
		else if(node.parentAttachAxis == CreatureSegmentNode.ParentAttachAxis.yNeg) {  // Neg Y
			retVec.x = node.attachPointParent.x;
			retVec.y = -0.5f;
			retVec.z = node.attachPointParent.z;
		}
		else if(node.parentAttachAxis == CreatureSegmentNode.ParentAttachAxis.zNeg) { // Neg Z
			retVec.x = node.attachPointParent.x;
			retVec.y = node.attachPointParent.y;
			retVec.z = -0.5f;
		}
		else if(node.parentAttachAxis == CreatureSegmentNode.ParentAttachAxis.xPos) {  // Pos X
			retVec.x = 0.5f;
			retVec.y = node.attachPointParent.y;
			retVec.z = node.attachPointParent.z;
		}
		else if(node.parentAttachAxis == CreatureSegmentNode.ParentAttachAxis.yPos) {  // Pos Y
			retVec.x = node.attachPointParent.x;
			retVec.y = 0.5f;
			retVec.z = node.attachPointParent.z;
		}
		else if(node.parentAttachAxis == CreatureSegmentNode.ParentAttachAxis.zPos) {  // Pos Z
			retVec.x = node.attachPointParent.x;
			retVec.y = node.attachPointParent.y;
			retVec.z = 0.5f;
		}
		
		return retVec;
	}
	
	Vector3 GetBlockPosition(Vector3 parentCenter, Vector3 parentSize, Vector3 attachParent, Vector3 attachChild, Vector3 ownSize, CreatureSegmentNode.ParentAttachAxis attachAxis) {
				
		float xOffset = parentCenter.x;
		float yOffset = parentCenter.y;
		float zOffset = parentCenter.z;


		if(attachAxis == CreatureSegmentNode.ParentAttachAxis.xNeg) {  // Neg X
			// Adjusting Offset For Connection Axis, each segment is centered on the other
			xOffset = parentCenter.x - ((parentSize.x / 2f) + (ownSize.x / 2f));
			// Adjust Offset due to Attach Point Parent  ( the location on the parent segment where the joint hinges ):
			yOffset += attachParent.y * parentSize.y;
			zOffset += attachParent.z * parentSize.z;
			// Adjust Offset due to own Attach Point  ( the location on this segment where the joint hinges ):
			yOffset -= attachChild.y * ownSize.y;
			zOffset -= attachChild.z * ownSize.z;
		}
		else if(attachAxis == CreatureSegmentNode.ParentAttachAxis.yNeg) {  // Neg Y
			yOffset = parentCenter.y - ((parentSize.y / 2f) + (ownSize.y / 2f));
			// Adjust Offset due to Attach Point Parent  ( the location on the parent segment where the joint hinges ):
			xOffset += attachParent.x * parentSize.x;
			zOffset += attachParent.z * parentSize.z;
			// Adjust Offset due to own Attach Point  ( the location on this segment where the joint hinges ):
			xOffset -= attachChild.x * ownSize.x;
			zOffset -= attachChild.z * ownSize.z;
		}
		else if(attachAxis == CreatureSegmentNode.ParentAttachAxis.zNeg) { // Neg Z
			zOffset = parentCenter.z - ((parentSize.z / 2f) + (ownSize.z / 2f));
			// Adjust Offset due to Attach Point Parent  ( the location on the parent segment where the joint hinges ):
			xOffset += attachParent.x * parentSize.x;
			yOffset += attachParent.y * parentSize.y;
			// Adjust Offset due to own Attach Point  ( the location on this segment where the joint hinges ):
			xOffset -= attachChild.x * ownSize.x;
			yOffset -= attachChild.y * ownSize.y;
		}
		else if(attachAxis == CreatureSegmentNode.ParentAttachAxis.xPos) {  // Pos X
			xOffset = parentCenter.x + ((parentSize.x / 2f) + (ownSize.x / 2f));
			// Adjust Offset due to Attach Point Parent  ( the location on the parent segment where the joint hinges ):
			yOffset += attachParent.y * parentSize.y;
			zOffset += attachParent.z * parentSize.z;
			// Adjust Offset due to own Attach Point  ( the location on this segment where the joint hinges ):
			yOffset -= attachChild.y * ownSize.y;
			zOffset -= attachChild.z * ownSize.z;
		}
		else if(attachAxis == CreatureSegmentNode.ParentAttachAxis.yPos) {  // Pos Y
			yOffset = parentCenter.y + ((parentSize.y / 2f) + (ownSize.y / 2f));
			// Adjust Offset due to Attach Point Parent  ( the location on the parent segment where the joint hinges ):
			xOffset += attachParent.x * parentSize.x;
			zOffset += attachParent.z * parentSize.z;
			// Adjust Offset due to own Attach Point  ( the location on this segment where the joint hinges ):
			xOffset -= attachChild.x * ownSize.x;
			zOffset -= attachChild.z * ownSize.z;
		}
		else if(attachAxis == CreatureSegmentNode.ParentAttachAxis.zPos) {  // Pos Z
			zOffset = parentCenter.z + ((parentSize.z / 2f) + (ownSize.z / 2f));
			// Adjust Offset due to Attach Point Parent  ( the location on the parent segment where the joint hinges ):
			xOffset += attachParent.x * parentSize.x;
			yOffset += attachParent.y * parentSize.y;
			// Adjust Offset due to own Attach Point  ( the location on this segment where the joint hinges ):
			xOffset -= attachChild.x * ownSize.x;
			yOffset -= attachChild.y * ownSize.y;
		}



		
		Vector3 retVec = new Vector3(xOffset, yOffset, zOffset);
		return retVec;
	}
	#endregion

	#region FROM UNITY LIVESTREAM SCRIPTING TUTORIAL:
	/*public InventoryItemList inventoryItemList;
	private int viewIndex = 1;

	[MenuItem]
	static void Init() {
		EditorWindow.GetWindow (typeof(InventoryItemEditor));
	}

	void OnEnable() {
		if(EditorPrefs.HasKey ("ObjectPath")) {
			string objectPath = EditorPrefs.GetString ("ObjectPath");
			inventoryItemList = AssetDatabase.LoadAssetAtPath (objectPath, typeof(InventoryItemList)) as InventoryItemList;
		}
	}*/

	/*void CreateNewItemList() {
		// no overwrite protection!
		// this should probably get a string from the user to create a new name and pass it...
		viewIndex = 1;
		inventoryItemList = CreateInventoryItemList.Create();
		if(inventoryItemList) {
			string relPath = AssetDatabase.GetAssetPath (inventoryItemList);
			EditorPrefs.SetString ("ObjectPath", relPath);
		}
	}*/

	/*void OpenItemList() {
		string absPath = EditorUtility.OpenFilePanel ("Select Inventory Item List", "", "");
		if(absPath.StartsWith (Application.dataPath)) {
			string relPath = absPath.Substring (Application.dataPath.Length - "Assets".Length);
			inventoryItemList = AssetDatabase.LoadAssetAtPath (relPath, typeof(InventoryItemList)) as InventoryItemList;
			if(inventoryItemList) {
				EditorPrefs.SetString ("ObjectPath", relPath);
			}
		}
	}*/

	/*void AddItem() {
		InventoryItem newItem = new InventoryItem();
		newItem.itemName = "New Item";
		inventoryItemList.itemList.Add(newItem);
		viewIndex = inventoryItemList.itemList.Count;
	}

	void DeleteItem(int index) {
		inventoryItemList.itemList.RemoveAt(index);
	}*/
	#endregion

}
