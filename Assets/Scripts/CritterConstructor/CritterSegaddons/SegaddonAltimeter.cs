using UnityEngine;
using System.Collections;

public class SegaddonAltimeter {

    public int segmentID;
    public float[] altitude;
    public float[] fitnessAltitude;

    public SegaddonAltimeter() {
        altitude = new float[1];
        altitude[0] = 0f;
        fitnessAltitude = new float[1];
        fitnessAltitude[0] = 0f;
    }

    public SegaddonAltimeter(AddonAltimeter sourceNode) {
        altitude = new float[1];
        altitude[0] = 0f;
        fitnessAltitude = new float[1];
        fitnessAltitude[0] = 0f;
    }
}
