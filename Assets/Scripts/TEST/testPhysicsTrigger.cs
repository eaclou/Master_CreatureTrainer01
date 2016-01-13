using UnityEngine;
using System.Collections;

public class testPhysicsTrigger : MonoBehaviour {

    public testPhysicsExecOrder testPhysicsExecOrderScript;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate()
    {
        Debug.Log(this.gameObject.name.ToString() + " FixedUpdate(" + testPhysicsExecOrderScript.timeSteps.ToString() +
            ") Start RigidBodPos: ( " + this.GetComponent<Rigidbody>().position.x.ToString() + ", " + this.GetComponent<Rigidbody>().position.y.ToString() + ", " + this.GetComponent<Rigidbody>().position.z.ToString() + " )" + 
            " RigidBodVel: ( " + this.GetComponent<Rigidbody>().velocity.x.ToString() + ", " + this.GetComponent<Rigidbody>().velocity.y.ToString() + ", " + this.GetComponent<Rigidbody>().velocity.z.ToString() + " )");
        
        RaycastHit hit;
        Ray downRay = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(downRay, out hit, 10f))
        {
            //print(this.gameObject.name.ToString() + " FixedUpdate(" + testPhysicsExecOrderScript.timeSteps.ToString() + ") There is something " + hit.distance.ToString() + " units below the segment!");
        }

        //this.GetComponent<Rigidbody>().AddForce(Vector3.right);
        //this.GetComponent<Rigidbody>().MovePosition(Vector3.up * 0.2f);
        float penetrationY = this.GetComponent<Rigidbody>().position.y - (this.transform.localScale.y / 2f) - -0.6f;
        float distX = Mathf.Abs(this.GetComponent<Rigidbody>().position.x);
        print(penetrationY.ToString());
        if (penetrationY < 0f && distX < 0.5f)
        {
            Vector3 newPos = new Vector3(this.GetComponent<Rigidbody>().position.x, 0f, this.GetComponent<Rigidbody>().position.z);
            newPos.y = -0.6f - penetrationY + (this.transform.localScale.y / 2f);
            float bounceFactor = 0.5f;
            Vector3 newVel = new Vector3(this.GetComponent<Rigidbody>().velocity.x, -this.GetComponent<Rigidbody>().velocity.y * bounceFactor, this.GetComponent<Rigidbody>().velocity.z);
            print(newPos.ToString());
            this.GetComponent<Rigidbody>().MovePosition(newPos);
            this.GetComponent<Rigidbody>().velocity = newVel;
        }
                
        if (Physics.Raycast(downRay, out hit, 10f))
        {
            //print(this.gameObject.name.ToString() + " FixedUpdate PostMove(" + testPhysicsExecOrderScript.timeSteps.ToString() + ") There is something " + hit.distance.ToString() + " units below the segment!");
        }

        Debug.Log(this.gameObject.name.ToString() + " FixedUpdate(" + testPhysicsExecOrderScript.timeSteps.ToString() +
            ") End RigidBodPos: ( " + this.GetComponent<Rigidbody>().position.x.ToString() + ", " + this.GetComponent<Rigidbody>().position.y.ToString() + ", " + this.GetComponent<Rigidbody>().position.z.ToString() + " )" +
            " RigidBodVel: ( " + this.GetComponent<Rigidbody>().velocity.x.ToString() + ", " + this.GetComponent<Rigidbody>().velocity.y.ToString() + ", " + this.GetComponent<Rigidbody>().velocity.z.ToString() + " )");
    }

    void OnTriggerEnter(Collider otherCollider)
    {
        //Debug.Log("OnTriggerEnter() " + testPhysicsExecOrderScript.timeSteps.ToString());
        //Debug.Log("Segment RigidBodPos OnTriggerEnter(): ( " + this.GetComponent<Rigidbody>().position.x.ToString() + ", " + this.GetComponent<Rigidbody>().position.y.ToString() + ", " + this.GetComponent<Rigidbody>().position.z.ToString() + " )");
    }

    void OnTriggerStay(Collider otherCollider)
    {
        /*
        Debug.Log(this.gameObject.name.ToString() + " OnTriggerStay triggered by: " + otherCollider.gameObject.name);
        Debug.Log(this.gameObject.name.ToString() + " OnTriggerStay(" + testPhysicsExecOrderScript.timeSteps.ToString() +
            ") Start RigidBodPos: ( " + this.GetComponent<Rigidbody>().position.x.ToString() + ", " + this.GetComponent<Rigidbody>().position.y.ToString() + ", " + this.GetComponent<Rigidbody>().position.z.ToString() + " )" +
            " RigidBodVel: ( " + this.GetComponent<Rigidbody>().velocity.x.ToString() + ", " + this.GetComponent<Rigidbody>().velocity.y.ToString() + ", " + this.GetComponent<Rigidbody>().velocity.z.ToString() + " )");

        RaycastHit hit;
        Ray downRay = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(downRay, out hit, 10f))
        {
            print(this.gameObject.name.ToString() + " OnTriggerStay(" + testPhysicsExecOrderScript.timeSteps.ToString() + ") There is something " + hit.distance.ToString() + " units below the segment!");
        }

        //this.GetComponent<Rigidbody>().AddForce(Vector3.forward);
        //this.GetComponent<Rigidbody>().MovePosition(Vector3.up * 0.5f);

        if (Physics.Raycast(downRay, out hit, 10f))
        {
            print(this.gameObject.name.ToString() + " OnTriggerStay PostMove(" + testPhysicsExecOrderScript.timeSteps.ToString() + ") There is something " + hit.distance.ToString() + " units below the segment!");
        }

        Debug.Log(this.gameObject.name.ToString() + " OnTriggerStay(" + testPhysicsExecOrderScript.timeSteps.ToString() +
            ") End RigidBodPos: ( " + this.GetComponent<Rigidbody>().position.x.ToString() + ", " + this.GetComponent<Rigidbody>().position.y.ToString() + ", " + this.GetComponent<Rigidbody>().position.z.ToString() + " )" +
            " RigidBodVel: ( " + this.GetComponent<Rigidbody>().velocity.x.ToString() + ", " + this.GetComponent<Rigidbody>().velocity.y.ToString() + ", " + this.GetComponent<Rigidbody>().velocity.z.ToString() + " )");
        */
    }

    void OnTriggerExit(Collider otherCollider)
    {
        //Debug.Log("OnTriggerExit()" + testPhysicsExecOrderScript.timeSteps.ToString());
    }
}
