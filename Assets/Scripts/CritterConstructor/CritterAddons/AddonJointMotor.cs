using UnityEngine;
using System.Collections;

public class AddonJointMotor {

    public int critterNodeID;
    public float[] motorForce;
    public float[] motorSpeed;

	public AddonJointMotor() {
        //Debug.Log("Constructor AddonJointMotor()");
        motorForce = new float[1];
        motorForce[0] = 100f;
        motorSpeed = new float[1];
        motorSpeed[0] = 100f;
    }

    public AddonJointMotor(int id) {
        //Debug.Log("Constructor AddonJointMotor(" + id.ToString() + ")");
        critterNodeID = id;
        motorForce = new float[1];
        motorForce[0] = 100f;
        motorSpeed = new float[1];
        motorSpeed[0] = 100f;
    }
}
