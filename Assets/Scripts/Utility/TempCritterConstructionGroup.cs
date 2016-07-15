using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TempCritterConstructionGroup : MonoBehaviour {
    public static TempCritterConstructionGroup tempCritterConstructionGroup;

    void Awake() {
        //Debug.Log ("ArenaGroup AWAKE()");
        tempCritterConstructionGroup = this;

        //ViewOff();
        //this.enabled = false;
    }



    public void ViewOn() {
        this.gameObject.SetActive(true);
        this.enabled = true;
    }

    public void ViewOff() {
        this.gameObject.SetActive(false);
        this.enabled = false;
    }

    public void DeleteSegments() {
        // Delete existing GameObjects:
        var children = new List<GameObject>();
        foreach (Transform child in this.gameObject.transform) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));        
    }
}
