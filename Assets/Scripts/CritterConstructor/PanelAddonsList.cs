using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PanelAddonsList : MonoBehaviour {

    public PanelNodeAddons panelNodeAddons;
    public GameObject addonDisplayPrefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void RepopulateList(CritterNode sourceNode) {
        var children = new List<GameObject>();
        foreach (Transform child in this.gameObject.transform) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));

        for (int i = 0; i < sourceNode.addonsList.Count; i++) {
            Debug.Log("Addon# " + i.ToString() + ", " + sourceNode.addonsList[i].GetType().ToString());
            GameObject itemDisplayGO = (GameObject)Instantiate(addonDisplayPrefab);
            PanelAddonItemDisplay itemDisplay = itemDisplayGO.GetComponent<PanelAddonItemDisplay>();
            itemDisplay.panelAddonsList = this;
            itemDisplay.index = i;
            itemDisplay.sourceAddon = sourceNode.addonsList[i];
            itemDisplayGO.transform.SetParent(this.transform);

            //itemDisplay.textHeader.text = sourceNode.addonsList[i].GetType().ToString();
            itemDisplay.Prime();
        }
    }
}
