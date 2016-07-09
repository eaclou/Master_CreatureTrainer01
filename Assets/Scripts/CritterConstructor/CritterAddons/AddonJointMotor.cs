using UnityEngine;
using System.Collections;

public class AddonJointMotor {

    public int critterNodeID;
    public int innov;
    public float[] motorForce;
    public float[] motorSpeed;

	public AddonJointMotor() {
        //Debug.Log("Constructor AddonJointMotor()");
        motorForce = new float[1];
        motorForce[0] = 100f;
        motorSpeed = new float[1];
        motorSpeed[0] = 100f;
    }

    public AddonJointMotor(int id, int inno) {
        //Debug.Log("Constructor AddonJointMotor(" + id.ToString() + ")");
        critterNodeID = id;
        innov = inno;
        motorForce = new float[1];
        motorForce[0] = 100f;
        motorSpeed = new float[1];
        motorSpeed[0] = 100f;
    }

    public AddonJointMotor CloneThisAddon() {
        AddonJointMotor clonedAddon = new AddonJointMotor(this.critterNodeID, this.innov);
        clonedAddon.motorForce[0] = this.motorForce[0];
        clonedAddon.motorSpeed[0] = this.motorSpeed[0];
        return clonedAddon;
    }
}
