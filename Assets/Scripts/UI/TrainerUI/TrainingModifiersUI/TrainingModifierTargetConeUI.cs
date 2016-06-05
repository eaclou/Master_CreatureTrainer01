using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TrainingModifierTargetConeUI : MonoBehaviour {

    public Slider sliderDuration;
    public Text textDuration;
    public Slider sliderNumRounds;
    public Text textNumRounds;
    public Slider sliderBeginMinDistance;
    public Text textBeginMinDistance;
    public Slider sliderBeginMaxDistance;
    public Text textBeginMaxDistance;
    public Slider sliderEndMinDistance;
    public Text textEndMinDistance;
    public Slider sliderEndMaxDistance;
    public Text textEndMaxDistance;
    public Slider sliderBeginMinAngle;
    public Text textBeginMinAngle;
    public Slider sliderBeginMaxAngle;
    public Text textBeginMaxAngle;
    public Slider sliderEndMinAngle;
    public Text textEndMinAngle;
    public Slider sliderEndMaxAngle;
    public Text textEndMaxAngle;
    public Toggle toggleLiveForever;
    public Toggle toggleHorizontal;
    public Toggle toggleVertical;

    // Use this for initialization
    void Start () {
        sliderDuration.minValue = 2f;
        sliderDuration.maxValue = 500f;
        sliderDuration.value = 0f;  // DEFAULT
        textDuration.text = sliderDuration.value.ToString();

        sliderNumRounds.minValue = 1f;
        sliderNumRounds.maxValue = 10f;
        sliderNumRounds.value = 0f;  // DEFAULT
        textNumRounds.text = sliderNumRounds.value.ToString();

        sliderBeginMinDistance.minValue = 0f;
        sliderBeginMinDistance.maxValue = 50f;
        sliderBeginMinDistance.value = 0f;  // DEFAULT
        textBeginMinDistance.text = sliderBeginMinDistance.value.ToString();

        sliderBeginMaxDistance.minValue = 0f;
        sliderBeginMaxDistance.maxValue = 50f;
        sliderBeginMaxDistance.value = 0f;  // DEFAULT
        textBeginMaxDistance.text = sliderBeginMaxDistance.value.ToString();

        sliderEndMinDistance.minValue = 0f;
        sliderEndMinDistance.maxValue = 50f;
        sliderEndMinDistance.value = 0f;  // DEFAULT
        textEndMinDistance.text = sliderEndMinDistance.value.ToString();

        sliderEndMaxDistance.minValue = 0f;
        sliderEndMaxDistance.maxValue = 50f;
        sliderEndMaxDistance.value = 0f;  // DEFAULT
        textEndMaxDistance.text = sliderEndMaxDistance.value.ToString();

        sliderBeginMinAngle.minValue = 0f;
        sliderBeginMinAngle.maxValue = 180f;
        sliderBeginMinAngle.value = 0f;  // DEFAULT
        textBeginMinAngle.text = sliderBeginMinAngle.value.ToString();

        sliderBeginMaxAngle.minValue = 0f;
        sliderBeginMaxAngle.maxValue = 180f;
        sliderBeginMaxAngle.value = 0f;  // DEFAULT
        textBeginMaxAngle.text = sliderBeginMaxAngle.value.ToString();

        sliderEndMinAngle.minValue = 0f;
        sliderEndMinAngle.maxValue = 180f;
        sliderEndMinAngle.value = 0f;  // DEFAULT
        textEndMinAngle.text = sliderEndMinAngle.value.ToString();

        sliderEndMaxAngle.minValue = 0f;
        sliderEndMaxAngle.maxValue = 180f;
        sliderEndMaxAngle.value = 0f;  // DEFAULT
        textEndMaxAngle.text = sliderEndMaxAngle.value.ToString();

        toggleLiveForever.isOn = true;
        toggleHorizontal.isOn = true;
        toggleVertical.isOn = true;
    }

    public void SliderDuration(float val) {
        textDuration.text = val.ToString();
    }
    public void SliderNumRounds(float val) {
        textNumRounds.text = val.ToString();
    }
    public void SliderBeginMinDistance(float val) {
        textBeginMinDistance.text = val.ToString();
    }
    public void SliderBeginMaxDistance(float val) {
        textBeginMaxDistance.text = val.ToString();
    }
    public void SliderEndMinDistance(float val) {
        textEndMinDistance.text = val.ToString();
    }
    public void SliderEndMaxDistance(float val) {
        textEndMaxDistance.text = val.ToString();
    }
    public void SliderBeginMinAngle(float val) {
        textBeginMinAngle.text = val.ToString();
    }
    public void SliderBeginMaxAngle(float val) {
        textBeginMaxAngle.text = val.ToString();
    }
    public void SliderEndMinAngle(float val) {
        textEndMinAngle.text = val.ToString();
    }
    public void SliderEndMaxAngle(float val) {
        textEndMaxAngle.text = val.ToString();
    }
}
