using UnityEngine;
using System.Collections;

public class AddonMouthBasic {

    public int critterNodeID;
    public int innov;
    //public float[] sensitivity;

    public AddonMouthBasic() {
        Debug.Log("Constructor AddonMouthBasic()");
        //sensitivity = new float[1];
        //sensitivity[0] = 1f;
    }

    public AddonMouthBasic(int id, int inno) {
        Debug.Log("Constructor AddonMouthBasic(" + id.ToString() + ")");
        critterNodeID = id;
        innov = inno;
        //sensitivity = new float[1];
        //sensitivity[0] = 1f;
    }
}
