using UnityEngine;
using System.Collections;

public class CritterEditorInputManager : MonoBehaviour {

    public CritterConstructorManager critterConstructorManager;
    public CritterConstructorCameraController critterConstructorCameraController;
    public CritterEditorUI critterEditorUI;
    public CritterEditorState critterEditorState;
    
    public bool mouseLeftClickDown = false;
    public bool mouseMiddleClickDown = false;
    public bool mouseRightClickDown = false;
    public bool mouseLeftClickUp = false;
    public bool mouseMiddleClickUp = false;
    public bool mouseRightClickUp = false;
    public bool keyAltDown = false;
    public bool keyAltUp = false;
    public bool keyFDown = false;
    public bool keyQDown = false;
    public bool keyWDown = false;
    public bool keyRDown = false;

    public void InitKeyPressBools() {
        mouseLeftClickDown = false;
        mouseMiddleClickDown = false;
        mouseRightClickDown = false;
        mouseLeftClickUp = false;
        mouseMiddleClickUp = false;
        mouseRightClickUp = false;
        keyAltDown = false;
        keyAltUp = false;
        keyFDown = false;
        keyQDown = false;
        keyWDown = false;
        keyRDown = false;
    }

    public void CheckInputs() {
        InitKeyPressBools();  // reset key and mouse press bools to false
        // Update Mouse Cursor Information!!
        critterEditorState.SetMouseCursorVelocity(new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")));
        critterEditorState.SetMouseCursorPosition(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        //  Is the alt-key currently ON?
        if (Input.GetKeyDown("left alt") || Input.GetKeyDown("right alt")) {
            keyAltDown = true;
            //critterEditorState.UpdateStateAltDown();
        }
        if (Input.GetKeyUp("left alt") || Input.GetKeyUp("right alt")) {
            keyAltUp = true;
            //critterEditorState.UpdateStateAltUp();
        }        
        if (Input.GetMouseButtonDown(0)) {
            mouseLeftClickDown = true;
            //critterEditorState.UpdateStateLeftClickDown();
        }
        else if (Input.GetMouseButtonDown(2)) {
            mouseMiddleClickDown = true;
            //critterEditorState.UpdateStateMiddleClickDown();
        }        
        else if (Input.GetMouseButtonDown(1)) {
            mouseRightClickDown = true;
            
            //critterEditorState.UpdateStateRightClickDown();
        }
        if (Input.GetMouseButtonUp(0)) {
            mouseLeftClickUp = true;
            //critterEditorState.UpdateStateLeftClickUp();
        }
        else if (Input.GetMouseButtonUp(2)) {
            mouseMiddleClickUp = true;
            //critterEditorState.UpdateStateMiddleClickUp();
        }
        else if (Input.GetMouseButtonUp(1)) {
            mouseRightClickUp = true;
            //critterEditorState.UpdateStateRightClickUp();
        }
        
        if (Input.GetKeyDown("f")) {
            keyFDown = true;
        }
        if (Input.GetKeyDown("q")) {
            keyQDown = true;
        }
        if (Input.GetKeyDown("w")) {
            keyWDown = true;
        }
        if (Input.GetKeyDown("r")) {
            keyRDown = true;
            Debug.Log("key r down");
        }

        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(-1)) {  // is the cursor over any unity UI element?
            critterEditorState.mouseOverUI = true;
        }
        else {
            critterEditorState.mouseOverUI = false;
        }        

        critterEditorState.UpdateStatesFromInput(this);  // Send keypress information to EditorState to figure out what to do with it
    }
}
