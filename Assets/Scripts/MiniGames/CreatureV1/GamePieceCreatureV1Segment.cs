using UnityEngine;
using System.Collections;


public class GamePieceCreatureV1Segment : GamePieceRigidBody {

    //public float resistanceForceMultiplier = 20f;
    //public float pushOnly = 1f;
    public float frictionForceMultiplier = 0.1f;
    public float bounceFactor = 1.0f;
    public float isColliding = 0.0f;

    void Awake()
    {
        //this.GetComponent<SphereCollider>().isTrigger = true;
    }

    void OnTriggerEnter(Collider otherCollider)
    {
        /*
        if (otherCollider.gameObject.layer == LayerMask.NameToLayer("environment"))
        {
            Debug.Log("OnTriggerEnter: RigidBodPos: (" + this.GetComponent<Rigidbody>().position.x.ToString() + ", " + this.GetComponent<Rigidbody>().position.y.ToString() + ", " + this.GetComponent<Rigidbody>().position.z.ToString() + ")");
            isColliding = 1.0f;
            Rigidbody rigidBod = this.GetComponent<Rigidbody>();
            Vector3 ownVel = rigidBod.GetPointVelocity(this.gameObject.transform.position);
            Vector3 resistanceForce = new Vector3(0f, 0f, 0f);
            resistanceForce.x = -1f * ownVel.x * frictionForceMultiplier;
            resistanceForce.y = -1f * ownVel.y * resistanceForceMultiplier;
            resistanceForce.z = -1f * ownVel.z * frictionForceMultiplier;
            //rigidBod.AddForce(resistanceForce);
            Debug.Log("OnTriggerEnter - AddForce(): RigidBodPos: (" + this.GetComponent<Rigidbody>().position.x.ToString() + ", " + this.GetComponent<Rigidbody>().position.y.ToString() + ", " + this.GetComponent<Rigidbody>().position.z.ToString() + ")");
            //Debug.Log ("OnEnter: ownVel: " + ownVel.ToString() + ", resistanceForce: " + resistanceForce.ToString());
            float penetrationDepth = otherCollider.gameObject.transform.position.y - rigidBod.gameObject.transform.position.y;
            rigidBod.MovePosition(new Vector3(rigidBod.position.x, otherCollider.gameObject.transform.position.y + rigidBod.gameObject.transform.localScale.x + penetrationDepth * bounceFactor, rigidBod.position.z));
            Debug.Log("OnTriggerEnter - MovePosition(): RigidBodPos: (" + this.GetComponent<Rigidbody>().position.x.ToString() + ", " + this.GetComponent<Rigidbody>().position.y.ToString() + ", " + this.GetComponent<Rigidbody>().position.z.ToString() + ")");
        }
        */
    }

    void OnTriggerStay(Collider otherCollider)
    {
        /*
        if (otherCollider.gameObject.layer == LayerMask.NameToLayer("environment"))
        {
            Debug.Log("OnTriggerStay: RigidBodPos: (" + this.GetComponent<Rigidbody>().position.x.ToString() + ", " + this.GetComponent<Rigidbody>().position.y.ToString() + ", " + this.GetComponent<Rigidbody>().position.z.ToString() + ")");
            Rigidbody rigidBod = this.GetComponent<Rigidbody>();
            Vector3 ownVel = rigidBod.GetPointVelocity(this.gameObject.transform.position);
            Vector3 resistanceForce = new Vector3(0f, 0f, 0f);
            resistanceForce.x = -1f * ownVel.x * frictionForceMultiplier;
            if (ownVel.y > 0f)
            { // if moving out of the ground
                if (pushOnly > 0.99999f)
                {
                    // if pushOnly, then y force stays at 0f;
                }
                else
                {
                    resistanceForce.y = -1f * ownVel.y * resistanceForceMultiplier * pushOnly;
                }
            }
            else
            {
                resistanceForce.y = -1f * ownVel.y * resistanceForceMultiplier;
            }
            resistanceForce.z = -1f * ownVel.z * frictionForceMultiplier;
            //rigidBod.AddForce(resistanceForce);
            Debug.Log("OnTriggerStay - AddForce(): RigidBodPos: (" + this.GetComponent<Rigidbody>().position.x.ToString() + ", " + this.GetComponent<Rigidbody>().position.y.ToString() + ", " + this.GetComponent<Rigidbody>().position.z.ToString() + ")");
            //Debug.Log ("OnStay: ownVel: " + ownVel.ToString() + ", resistanceForce: " + resistanceForce.ToString());
            //float penetrationDepth = otherCollider.gameObject.transform.position.y - rigidBod.gameObject.transform.position.y;
            rigidBod.MovePosition(new Vector3(rigidBod.position.x, otherCollider.gameObject.transform.position.y + rigidBod.gameObject.transform.localScale.x, rigidBod.position.z));
            Debug.Log("OnTriggerStay = MovePosition(): RigidBodPos: (" + this.GetComponent<Rigidbody>().position.x.ToString() + ", " + this.GetComponent<Rigidbody>().position.y.ToString() + ", " + this.GetComponent<Rigidbody>().position.z.ToString() + ")");
        }
        */
    }

    void OnTriggerExit(Collider otherCollider)
    {
        /*
        Debug.Log("OnTriggerExit: RigidBodPos: (" + this.GetComponent<Rigidbody>().position.x.ToString() + ", " + this.GetComponent<Rigidbody>().position.y.ToString() + ", " + this.GetComponent<Rigidbody>().position.z.ToString() + ")");
        if (otherCollider.gameObject.layer == LayerMask.NameToLayer("environment"))
        {
            //Debug.Log ("OnTriggerExit: " + this.ToString() + ", other: " + otherCollider.ToString());
            isColliding = 0.0f;
        }
        */
    }

    public override Mesh BuildMesh() {  // SIMPLE CUBE!
		MeshBuilder meshBuilder = new MeshBuilder();
        
		int m_HeightSegmentCount = 8;
		int m_RadialSegmentCount = 8;
		float m_Radius = 0.75f;
		float m_VerticalScale = 1f;
		Quaternion rotation = Quaternion.identity;
		Vector3 offset = new Vector3(0f, -0.75f, 0f);
		//the angle increment per height segment:
		float angleInc = Mathf.PI / m_HeightSegmentCount;
		
		//the vertical (scaled) radius of the sphere:
		float verticalRadius = m_Radius * m_VerticalScale;
		
		//build the rings:
		for (int i = 0; i <= m_HeightSegmentCount; i++)
		{
			Vector3 centrePos = Vector3.zero;
			
			//calculate a height offset and radius based on a vertical circle calculation:
			centrePos.y = -Mathf.Cos(angleInc * i);
			float radius = Mathf.Sin(angleInc * i);
			
			//calculate the slope of the shpere at this ring based on the height and radius:
			Vector2 slope = new Vector3(-centrePos.y / m_VerticalScale, radius);
			slope.Normalize();
			
			//multiply the unit height by the vertical radius, and then add the radius to the height to make this sphere originate from its base rather than its centre:
			centrePos.y = centrePos.y * verticalRadius + verticalRadius;
			
			//scale the radius by the one stored in the partData:
			radius *= m_Radius;
			
			//calculate the final position of the ring centre:
			Vector3 finalRingCentre = rotation * centrePos + offset;
			
			//V coordinate:
			float v = (float)i / m_HeightSegmentCount;
			
			//build the ring:
			BuildRing(meshBuilder, m_RadialSegmentCount, finalRingCentre, radius, v, i > 0, rotation, slope);
		}

		return meshBuilder.CreateMesh ();
	}
}
