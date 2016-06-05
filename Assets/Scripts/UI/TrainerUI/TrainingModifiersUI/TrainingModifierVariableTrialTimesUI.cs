using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TrainingModifierVariableTrialTimesUI : MonoBehaviour {

    public Slider sliderDuration;
    public Text textDuration;
    public Slider sliderBeginMinTime;
    public Text textBeginMinTime;
    public Slider sliderBeginMaxTime;
    public Text textBeginMaxTime;
    public Slider sliderEndMinTime;
    public Text textEndMinTime;
    public Slider sliderEndMaxTime;
    public Text textEndMaxTime;
    public Toggle toggleLiveForever;

    // Use this for initialization
    void Start () {
        sliderDuration.minValue = 2f;
        sliderDuration.maxValue = 500f;
        sliderDuration.value = 0f;  // DEFAULT
        textDuration.text = sliderDuration.value.ToString();

        sliderBeginMinTime.minValue = 0f;
        sliderBeginMinTime.maxValue = 1000f;
        sliderBeginMinTime.value = 0f;  // DEFAULT
        textBeginMinTime.text = sliderBeginMinTime.value.ToString();

        sliderBeginMaxTime.minValue = 0f;
        sliderBeginMaxTime.maxValue = 1000f;
        sliderBeginMaxTime.value = 0f;  // DEFAULT
        textBeginMaxTime.text = sliderBeginMaxTime.value.ToString();

        sliderEndMinTime.minValue = 0f;
        sliderEndMinTime.maxValue = 1000f;
        sliderEndMinTime.value = 0f;  // DEFAULT
        textEndMinTime.text = sliderEndMinTime.value.ToString();

        sliderEndMaxTime.minValue = 0f;
        sliderEndMaxTime.maxValue = 1000f;
        sliderEndMaxTime.value = 0f;  // DEFAULT
        textEndMaxTime.text = sliderEndMaxTime.value.ToString();

        toggleLiveForever.isOn = true;
    }

    public void SliderDuration(float val) {
        textDuration.text = val.ToString();
    }
    public void SliderBeginMinTime(float val) {
        textBeginMinTime.text = val.ToString();
    }
    public void SliderBeginMaxTime(float val) {
        textBeginMaxTime.text = val.ToString();
    }
    public void SliderEndMinTime(float val) {
        textEndMinTime.text = val.ToString();
    }
    public void SliderEndMaxTime(float val) {
        textEndMaxTime.text = val.ToString();
    }
}
