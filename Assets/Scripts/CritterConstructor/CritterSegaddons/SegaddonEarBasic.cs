using UnityEngine;
using System.Collections;

public class SegaddonEarBasic {

    public int segmentID;
    public float sensitivity;
    public float[] activation;

    public SegaddonEarBasic() {
        sensitivity = 0f;
        activation = new float[1];
        activation[0] = 0f;
    }

    public SegaddonEarBasic(AddonEarBasic sourceNode) {
        sensitivity = sourceNode.sensitivity[0];
        activation = new float[1];
        activation[0] = 0f;
    }
}
