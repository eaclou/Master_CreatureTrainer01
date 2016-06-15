using UnityEngine;
using System.Collections;

public class testPhysicsStress : MonoBehaviour {

    public float timer = 0f;
    public float spawnTime = 20f;
    public GameObject chainPrefab;

	// Use this for initialization
	void Start () {
        Time.timeScale = 1f;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        timer += Time.fixedDeltaTime;

        if(timer >= spawnTime) {
            timer = 0f;

            //modifierDisplayGO = (GameObject)Instantiate(prefabModifierWideSearch);
            Instantiate(chainPrefab);
        }
	}
}
