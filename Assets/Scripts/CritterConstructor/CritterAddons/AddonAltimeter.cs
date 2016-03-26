using UnityEngine;
using System.Collections;

public class AddonAltimeter {

    public int critterNodeID;

    public AddonAltimeter() {
        Debug.Log("Constructor AddonAltimeter()");
    }

    public AddonAltimeter(int id) {
        Debug.Log("Constructor AddonAltimeter(" + id.ToString() + ")");
        critterNodeID = id;
    }
}
