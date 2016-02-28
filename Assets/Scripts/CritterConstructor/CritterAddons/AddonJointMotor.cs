using UnityEngine;
using System.Collections;

public class AddonJointMotor : CritterNodeAddonBase {

    public float[] motorForce;
    public float motorForceMin = 1f;
    public float motorForceMax = 1000f;


	public AddonJointMotor() {
        Debug.Log("Constructor AddonJointMotor()");
        motorForce = new float[1];
        motorForce[0] = 100f;
    }
}
