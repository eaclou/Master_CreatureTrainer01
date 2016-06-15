using UnityEngine;
using System.Collections;

public class SegaddonNoiseMakerBasic {

    public int segmentID;
    public float[] volume;

    public SegaddonNoiseMakerBasic() {
        volume = new float[1];
        volume[0] = 0f;
    }

    public SegaddonNoiseMakerBasic(AddonNoiseMakerBasic sourceNode) {
        volume = new float[1];
        volume[0] = sourceNode.amplitude[0];
    }
}
