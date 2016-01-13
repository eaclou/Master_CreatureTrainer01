using UnityEngine;
using System.Collections;

public class TargetSphereDrift : MonoBehaviour {

	public float frequencyX = 0.01f;
	public float frequencyY = 0.021f;
	public float frequencyZ = 0.029f;
	public float amplitudeX = 5f;
	public float amplitudeY = 2f;
	public float amplitudeZ = 3f;
	public float offsetX = 20.5f;
	public float offsetY = -1f;
	public float offsetZ = 3f;

	public float speedModFrequency = 1f;


	void FixedUpdate () {
		Vector3 pos = new Vector3(0f, 0f, 0f);

		pos.x = Mathf.Sin (Time.fixedTime * frequencyX + offsetX) * amplitudeX;
		pos.y = Mathf.Sin (Time.fixedTime * frequencyY + offsetY) * amplitudeY;
		pos.z = Mathf.Sin (Time.fixedTime * frequencyZ + offsetZ) * amplitudeZ;

		this.gameObject.transform.position = pos;
	}
}
