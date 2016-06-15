using UnityEngine;
using System.Collections;

public class AddonNoiseMakerBasic {

    public int critterNodeID;
    public float[] amplitude;

    public AddonNoiseMakerBasic() {
        Debug.Log("Constructor AddonNoiseMakerBasic()");
        amplitude = new float[1];
        amplitude[0] = 1f;
    }

    public AddonNoiseMakerBasic(int id) {
        Debug.Log("Constructor AddonNoiseMakerBasic(" + id.ToString() + ")");
        critterNodeID = id;
        amplitude = new float[1];
        amplitude[0] = 1f;
    }
}
