using UnityEngine;
using System.Collections;

public class AddonSticky {

    public int critterNodeID;

    public AddonSticky() {
        Debug.Log("Constructor AddonSticky()");
    }

    public AddonSticky(int id) {
        Debug.Log("Constructor AddonSticky(" + id.ToString() + ")");
        critterNodeID = id;
    }
}
