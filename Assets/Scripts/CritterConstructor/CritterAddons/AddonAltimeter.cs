using UnityEngine;
using System.Collections;

public class AddonAltimeter {

    public int critterNodeID;
    public int innov;

    public AddonAltimeter() {
        Debug.Log("Constructor AddonAltimeter()");
    }

    public AddonAltimeter(int id, int inno) {
        Debug.Log("Constructor AddonAltimeter(" + id.ToString() + ")");
        critterNodeID = id;
        innov = inno;
    }
}
