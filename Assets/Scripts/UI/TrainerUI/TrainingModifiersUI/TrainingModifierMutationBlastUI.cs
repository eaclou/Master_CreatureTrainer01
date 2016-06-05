using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TrainingModifierMutationBlastUI : MonoBehaviour {

    public Slider sliderDuration;
    public Text textDuration;
    public Toggle toggleLiveForever;

	// Use this for initialization
	void Start () {
        sliderDuration.minValue = 2f;
        sliderDuration.maxValue = 500f;
        sliderDuration.value = 0f;  // DEFAULT
        textDuration.text = sliderDuration.value.ToString();

        toggleLiveForever.isOn = true;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SliderDuration(float val) {

        textDuration.text = val.ToString();
    }
}
