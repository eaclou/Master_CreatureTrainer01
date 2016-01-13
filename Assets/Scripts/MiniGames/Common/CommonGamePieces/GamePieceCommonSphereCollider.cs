using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SphereCollider))]
public class GamePieceCommonSphereCollider : MonoBehaviour {

    public float resistanceForceMultiplier = 200f;
    public float frictionForceMultiplier = 200f;
    public float pushOnly = 1f;
    public float isColliding = 0.0f;

	void Awake() {
		this.GetComponent<SphereCollider>().isTrigger = true;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider otherCollider) {
		if(otherCollider.gameObject.layer == LayerMask.NameToLayer( "environment" )) {
            isColliding = 1.0f;
            Rigidbody rigidBod = this.GetComponent<Rigidbody>();
			Vector3 ownVel = rigidBod.GetPointVelocity(this.gameObject.transform.position);
			Vector3 resistanceForce = new Vector3(0f, 0f, 0f);
			resistanceForce.x = -1f * ownVel.x * frictionForceMultiplier;
			resistanceForce.y = -1f * ownVel.y * resistanceForceMultiplier;
			resistanceForce.z = -1f * ownVel.z * frictionForceMultiplier;
			rigidBod.AddForce (resistanceForce);
			//Debug.Log ("OnEnter: ownVel: " + ownVel.ToString() + ", resistanceForce: " + resistanceForce.ToString());
		}
	}

	void OnTriggerStay(Collider otherCollider) {
		if(otherCollider.gameObject.layer == LayerMask.NameToLayer( "environment" )) {
			Rigidbody rigidBod = this.GetComponent<Rigidbody>();
			Vector3 ownVel = rigidBod.GetPointVelocity(this.gameObject.transform.position);
			Vector3 resistanceForce = new Vector3(0f, 0f, 0f);
			resistanceForce.x = -1f * ownVel.x * frictionForceMultiplier;
			if(ownVel.y > 0f) { // if moving out of the ground
                if (pushOnly > 0.99999f) {                 
                    // if pushOnly, then y force stays at 0f;
                }
                else {
                    resistanceForce.y = -1f * ownVel.y * resistanceForceMultiplier * pushOnly;
                }
			}
            else
            {
                resistanceForce.y = -1f * ownVel.y * resistanceForceMultiplier;
            }
			resistanceForce.z = -1f * ownVel.z * frictionForceMultiplier;
			rigidBod.AddForce (resistanceForce);
			//Debug.Log ("OnStay: ownVel: " + ownVel.ToString() + ", resistanceForce: " + resistanceForce.ToString());
		}
	}

	void OnTriggerExit(Collider otherCollider) {
		if(otherCollider.gameObject.layer == LayerMask.NameToLayer( "environment" )) {
            //Debug.Log ("OnTriggerExit: " + this.ToString() + ", other: " + otherCollider.ToString());
            isColliding = 0.0f;
        }
	}

	void OnCollisionEnter(Collision collision) {
		//Debug.Log ("OnCollisionEnter: " + this.gameObject.ToString() + ", coll: " + collision.collider.gameObject.ToString());
	}
}
