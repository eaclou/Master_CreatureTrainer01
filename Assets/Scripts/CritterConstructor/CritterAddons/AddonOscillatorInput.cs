using UnityEngine;
using System.Collections;

public class AddonOscillatorInput {

    public int critterNodeID;
    public int innov;
    public float[] frequency;
    public float[] amplitude;
    public float[] offset;

    public AddonOscillatorInput() {
        Debug.Log("Constructor AddonOscillatorInput()");
        frequency = new float[1];
        frequency[0] = 1f;
        amplitude = new float[1];
        amplitude[0] = 1f;
        offset = new float[1];
        offset[0] = 0f;
    }

    public AddonOscillatorInput(int id, int inno) {
        Debug.Log("Constructor AddonOscillatorInput(" + id.ToString() + ")");
        critterNodeID = id;
        innov = inno;
        frequency = new float[1];
        frequency[0] = 1f;
        amplitude = new float[1];
        amplitude[0] = 1f;
        offset = new float[1];
        offset[0] = 0f;
    }
}
