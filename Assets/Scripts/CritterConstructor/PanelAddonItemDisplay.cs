using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PanelAddonItemDisplay : MonoBehaviour {

    public PanelAddonsList panelAddonsList;

    public Text textHeader;
    public Button buttonRemove;
    public GameObject panelAttributeList;

    public int index;

	// Use this for initialization
	void Start () {
        // go to hell VS
        Debug.Log("FUCK YOU LET ME SAVE");
	}

	// Update is called once per frame
	void Update () {
	
	}

    public void ClickRemoveAddon() {
        panelAddonsList.panelNodeAddons.critterEditorState.RemoveAddon(index);
    }
}
