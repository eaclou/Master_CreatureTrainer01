using UnityEngine;
using UnityEditor;
using System.Collections;

public static class CreatureEditor {

	private const float kEditorWindowTabHeight = 21.0f;
	private static Matrix4x4 _prevGuiMatrix;

	//public static CreatureData creatureData;  // the object that stores the genome for the current creature being edited

	// Quick access
	public static Vector2 mousePos;
	public static bool newNodeMode = false;
	
	// Static textures and styles
	public static Texture2D Background;
	public static Texture2D AALineTex;
	public static GUIStyle nodeBox;
	public static GUIStyle nodeButton;
	public static GUIStyle nodeLabel;
	public static GUIStyle nodeLabelBold;

	// Constants
	public const string editorPath = "Assets/Plugins/Creature_Node_Editor/";
	
	#region Setup
	
	//[NonSerialized]
	private static bool initiated = false;
	//[NonSerialized]
	public static bool InitiationError = false;
	
	public static void checkInit () 
	{
		if (!initiated && !InitiationError) 
		{			
			// Styles
			nodeBox = new GUIStyle (GUI.skin.box);
			nodeBox.normal.textColor = new Color (0.1f, 0.1f, 0.1f);			
			nodeButton = new GUIStyle (GUI.skin.button);			
			nodeLabel = new GUIStyle (GUI.skin.label);
			nodeLabel.normal.textColor = new Color (0.1f, 0.1f, 0.1f);
			
			nodeLabelBold = new GUIStyle (nodeLabel);
			nodeLabelBold.fontStyle = FontStyle.Bold;
			nodeLabelBold.wordWrap = false;
			
			initiated = true;
		}
		return;
	}
	
	#endregion

	// Used to bookend any OnGUI area where you want a zoomable area	
	public static Rect BeginZoomArea(float zoomScale, Rect screenCoordsArea) {
		GUI.EndGroup();  // end the internal Unity BeginGroup that starts at the beginning of drawing a window
		
		/*The next step is to set up correct clipping of the zoomed draw area by calling GUI.BeginGroup. This clip area is independent 
		 * of the editor window size but needs to match our zoomed in or zoomed out draw area. For example, if we were to zoom in by a 
		 * factor of 2 and our zoom area has a screen width and height of 200 by 200, we would need the clip area to be 100 by 100 pixels. 
		 * If we were to zoom out by half, i.e. by a factor of 0.5, we would need the clip area to be 400 by 400 pixels.*/
		
		//Rect clippedArea = screenCoordsArea.ScaleSizeBy(1f / zoomScale, screenCoordsArea.TopLeft());
		Rect clippedArea = RectExtensions.ScaleSizeBy(screenCoordsArea, 1f / zoomScale, screenCoordsArea.TopLeft());
		clippedArea.y += kEditorWindowTabHeight;
		GUI.BeginGroup (clippedArea);
		
		/* Note that we add kEditorWindowTabHeight, which has a value of 21, to the top edge of the clip area. That's to compensate 
		 * for the editor window tab at the top that displays the window name. Remember, we ended the group that Unity implicitly begins 
		 * that normally prevents us from rendering over it. So we need to account for that and that's why we add 21 pixels to the top edge 
		 * of the clip area.*/
		/* The final step is to change the GUI.matrix to do the scaling for us. To do that we need to create a composite matrix that first 
		 * translates the clip area's top-left corner to the origin, then does the scaling around the origin, and finally translates the zoomed 
		 * result back to where the clip area is supposed to be.*/
		
		_prevGuiMatrix = GUI.matrix;
		Matrix4x4 translation = Matrix4x4.TRS (clippedArea.TopLeft(), Quaternion.identity, Vector3.one);
		Matrix4x4 scale = Matrix4x4.Scale(new Vector3(zoomScale, zoomScale, 1.0f));
		GUI.matrix = translation * scale * translation.inverse * GUI.matrix;
		
		/*Note that for good style and to play nice we save off the old GUI.matrix and concatenate it with our scale matrix. However, 
		 * the code pretty much assumes that you haven't messed with GUI.matrix before calling EditorZoomArea.Begin.
		 * Finally, in EditorZoomArea.End we simply reset the GUI.matrix to what it was before, end the group for the clip area that we began, 
		 * and begin Unity's implicit group for the editor window again. Please check the full source code below for details.*/
		
		return clippedArea;
	}
	
	public static void EndZoomArea() {
		GUI.matrix = _prevGuiMatrix;
		GUI.EndGroup();
		GUI.BeginGroup(new Rect(0.0f, kEditorWindowTabHeight, Screen.width, Screen.height));	
	}
}
