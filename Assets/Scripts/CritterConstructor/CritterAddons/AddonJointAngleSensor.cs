using UnityEngine;
using System.Collections;

public class AddonJointAngleSensor {

    public int critterNodeID;
    public int innov;
    public float[] sensitivity;
    public bool[] measureVel;

    public AddonJointAngleSensor() {
        //Debug.Log("Constructor AddonJointAngleSensor()");
        sensitivity = new float[1];
        sensitivity[0] = 1f;
        measureVel = new bool[1];
        measureVel[0] = false;
    }

    public AddonJointAngleSensor(int id, int inno) {
        //Debug.Log("Constructor AddonJointAngleSensor(" + id.ToString() + ")");
        critterNodeID = id;
        innov = inno;
        sensitivity = new float[1];
        sensitivity[0] = 1f;
        measureVel = new bool[1];
        measureVel[0] = false;
    }
}
