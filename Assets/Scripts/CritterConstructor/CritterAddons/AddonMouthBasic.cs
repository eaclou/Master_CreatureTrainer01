using UnityEngine;
using System.Collections;

public class AddonMouthBasic {

    public int critterNodeID;
    //public float[] sensitivity;

    public AddonMouthBasic() {
        Debug.Log("Constructor AddonMouthBasic()");
        //sensitivity = new float[1];
        //sensitivity[0] = 1f;
    }

    public AddonMouthBasic(int id) {
        Debug.Log("Constructor AddonMouthBasic(" + id.ToString() + ")");
        critterNodeID = id;
        //sensitivity = new float[1];
        //sensitivity[0] = 1f;
    }
}
