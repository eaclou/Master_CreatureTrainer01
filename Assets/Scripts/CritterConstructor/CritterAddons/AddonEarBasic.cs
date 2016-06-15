using UnityEngine;
using System.Collections;

public class AddonEarBasic {

    public int critterNodeID;
    public float[] sensitivity;

    public AddonEarBasic() {
        Debug.Log("Constructor AddonEarBasic()");
        sensitivity = new float[1];
        sensitivity[0] = 1f;
    }

    public AddonEarBasic(int id) {
        Debug.Log("Constructor AddonEarBasic(" + id.ToString() + ")");
        critterNodeID = id;
        sensitivity = new float[1];
        sensitivity[0] = 1f;
    }
}
