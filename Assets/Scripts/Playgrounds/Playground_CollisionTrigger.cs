using UnityEngine;
using System.Collections;

public class Playground_CollisionTrigger : MonoBehaviour {

	void OnTriggerEnter(Collider other)
	{
		Debug.Log(other.ToString());
	}
}
