using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TrainingModifierTargetOmniUI : MonoBehaviour {

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
    public Toggle toggleLiveForever;
    public Toggle toggleForward;
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
        
        toggleLiveForever.isOn = true;
        toggleForward.isOn = true;
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
}
