using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Playground_Controller : MonoBehaviour {

	public static Playground_Controller playgroundControllerStatic;

	public float physicsTimeScale = 1f;
	public Vector3 physicsGravity = new Vector3(0f, 0f, 0f);
	public float gravityAmplitude = 0f;
	public float physicsViscosity = 100f;

	public GameObject targetObject;
	public Playground_Critter5B critter;
	public GameObject mainCamera;
	public Playground_UI_Display display;
	//public GameObject targetDirLine;

	private float pitchDot = 0f;
	private float yawDot = 0f;
	private float headingZ = 0f;

	private float throttle = 0f;
	private float accelTime = 1.05f;
	private bool throttleEngaged = false;
	private float elapsedThrottleTime = 0f;
	private float elapsedStoppedTime = 0f;

	private float speedMultiplier = 15f;
	private float sprintDotThreshold = 0.85f;
	private float trackingDotThreshold = 0.5f;
	private float turningSpeedThreshold = 0.2f;

	void Awake() {
		turningSpeedThreshold = turningSpeedThreshold / speedMultiplier;
		playgroundControllerStatic = this;
		this.gameObject.SetActive(true);
		this.enabled = true;

	}

	// Use this for initialization
	void Start () {
	
	}

	void FixedUpdate () {
		Time.timeScale = physicsTimeScale;

		physicsGravity.x = Mathf.Sin (Time.fixedTime * 0.5f + 0f) * gravityAmplitude;
		physicsGravity.y = Mathf.Cos (Time.fixedTime * 0.27f + 30f) * 0.6f * gravityAmplitude - 0.1f;
		physicsGravity.z = Mathf.Sin (Time.fixedTime * 1.2f + 10f) * gravityAmplitude;
		Physics.gravity = physicsGravity;

		Playground_Camera camera = mainCamera.GetComponent<Playground_Camera>();
		camera.focusPosition = critter.CritterCOM;

		Vector3 targetPos = (camera.AimDirection.normalized * 11f) + camera.focusPosition;
		targetObject.gameObject.transform.position = targetPos;

		headingZ = Vector3.Dot(critter.critterTargetVector.normalized, critter.critterVelocity.normalized);
		pitchDot = Vector3.Dot(mainCamera.gameObject.transform.up, critter.critterVelocity.normalized);
		yawDot = Vector3.Dot(mainCamera.gameObject.transform.right, critter.critterVelocity.normalized);

		UpdateThrottle();

		//critter.jointMotorForce = critter.jointMotorForceMax * throttle;
		critter.jointMotorSpeed = critter.jointMotorSpeedMax * throttle;
		//targetDirLine.gameObject.transform.position = camera.focusPosition;
		//targetDirLine.gameObject.transform.rotation = Quaternion.Euler (camera.AimDirection.normalized);

		SetBehavior();
	}

	// Update is called once per frame
	void Update () {
		/*if (Input.GetKeyDown(KeyCode.Alpha1)) {
			SetCritterBehavior(1);
		}
		if (Input.GetKeyDown(KeyCode.Alpha2)) {
			SetCritterBehavior(2);
		}
		if (Input.GetKeyDown(KeyCode.Alpha3)) {
			SetCritterBehavior(3);
		}
		if (Input.GetKeyDown(KeyCode.Alpha4)) {
			SetCritterBehavior(4);
		}*/

		if (Input.GetKeyDown(KeyCode.W)) {
			Debug.Log ("Pressed W DOWN");
			throttleEngaged = true;
			elapsedThrottleTime = 0f;
			elapsedStoppedTime = 0f;
		}
		if (Input.GetKeyUp(KeyCode.W)) {
			Debug.Log ("Pressed W UP");
			throttleEngaged = false;
			elapsedThrottleTime = 0f;
			elapsedStoppedTime = 0f;
		}

		UpdateUI ();
	}

	public void SetBehavior() {
		int newBehavior = critter.currentBehavior; // current behavior

		if(!throttleEngaged && elapsedStoppedTime >= accelTime) {
			newBehavior = 3;
		}
		if(throttleEngaged) {
			if(critter.critterVelocity.magnitude < turningSpeedThreshold || headingZ < trackingDotThreshold) {  // if barely moving or moving wrong direction
				newBehavior = 4;  // TURN towards target
			}
			if(headingZ >= trackingDotThreshold && headingZ < sprintDotThreshold) {
				newBehavior = 1;
			}
			if(headingZ >= sprintDotThreshold) {
				newBehavior = 2;  // SPRINT!
			}
		}

		critter.currentBehavior = newBehavior;
	}

	public void UpdateThrottle() {


		if(throttleEngaged) {
			throttle = Mathf.Lerp (0f, 1f, elapsedThrottleTime / accelTime);
			elapsedThrottleTime += Time.fixedDeltaTime;
		}
		else {
			throttle = Mathf.Lerp (throttle, 0f, elapsedStoppedTime / accelTime);
			elapsedStoppedTime += Time.fixedDeltaTime;
		}
	}

	public void SetCritterBehavior(int id) {
		critter.currentBehavior = id;
	}

	private void UpdateUI() {
		string behaviorName = "";
		if(critter.currentBehavior == 1) {
			behaviorName = "Tracking";
		}
		else if(critter.currentBehavior == 2) {
			behaviorName = "Sprinting";
		}
		else if(critter.currentBehavior == 3) {
			behaviorName = "Stopped";
		}
		else if(critter.currentBehavior == 4) {
			behaviorName = "Turning";
		}
		display.textCurBehavior.text = "Current Behavior: " + behaviorName;

		float speed = critter.critterVelocity.magnitude * 15f;
		display.textSpeedValue.text = speed.ToString();
		display.sliderSpeed.value = speed;
		
		display.sliderPitch.value = pitchDot * 0.5f + 0.5f;
		display.sliderYaw.value = yawDot * 0.5f + 0.5f;
		ColorBlock pitchCB = display.sliderPitch.colors;
		ColorBlock yawCB = display.sliderYaw.colors;
		pitchCB.disabledColor = Color.Lerp (Color.red, Color.green, headingZ * 0.5f + 0.5f);
		yawCB.disabledColor = Color.Lerp (Color.red, Color.green, headingZ * 0.5f + 0.5f);
		display.sliderPitch.colors = pitchCB;
		display.sliderYaw.colors = yawCB;

		display.sliderThrottle.value = throttle;
		display.textThrottleValue.text = throttle.ToString();
		//display.textCurBehavior.text = "pitchDot: " + pitchDot.ToString() + ", yawDot: " + yawDot.ToString();
	}
}
