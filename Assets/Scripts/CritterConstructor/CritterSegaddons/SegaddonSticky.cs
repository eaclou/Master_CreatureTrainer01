using UnityEngine;
using System.Collections;

public class SegaddonSticky {

    public int segmentID;
    public float[] stickiness;

    public SegaddonSticky() {
        stickiness = new float[1];
        stickiness[0] = 0f;
    }

    public SegaddonSticky(AddonSticky sourceNode) {
        stickiness = new float[1];
        stickiness[0] = 0f;
    }
}
