using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TrainingModifierWideSearchUI : MonoBehaviour {

    public Slider sliderDuration;
    public Text textDuration;
    public Slider sliderSimilarityThreshold;
    public Text textSimilarityThreshold;
    public Slider sliderAdoptionRate;
    public Text textAdoptionRate;
    public Slider sliderLargeSpeciesPenalty;
    public Text textLargeSpeciesPenalty;
    public Toggle toggleLiveForever;
    public Toggle toggleDecayEffectOverDuration;


    // Use this for initialization
    void Start () {
        sliderDuration.minValue = 2f;
        sliderDuration.maxValue = 500f;
        sliderDuration.value = 0f;  // DEFAULT
        textDuration.text = sliderDuration.value.ToString();

        sliderSimilarityThreshold.minValue = 0f;
        sliderSimilarityThreshold.maxValue = 5f;
        sliderSimilarityThreshold.value = 0f;  // DEFAULT
        textSimilarityThreshold.text = sliderSimilarityThreshold.value.ToString();

        sliderAdoptionRate.minValue = 0f;
        sliderAdoptionRate.maxValue = 1f;
        sliderAdoptionRate.value = 0f;  // DEFAULT
        textAdoptionRate.text = sliderAdoptionRate.value.ToString();

        sliderLargeSpeciesPenalty.minValue = 0f;
        sliderLargeSpeciesPenalty.maxValue = 0.5f;
        sliderLargeSpeciesPenalty.value = 0f;  // DEFAULT
        textLargeSpeciesPenalty.text = sliderLargeSpeciesPenalty.value.ToString();
        
        toggleLiveForever.isOn = true;
        toggleDecayEffectOverDuration.isOn = true;
    }

    public void SliderDuration(float val) {
        textDuration.text = val.ToString();
    }
    public void SliderSimilarityThreshold(float val) {
        textSimilarityThreshold.text = val.ToString();
    }
    public void SliderAdoptionRate(float val) {
        textAdoptionRate.text = val.ToString();
    }
    public void SliderLargeSpeciesPenalty(float val) {
        textLargeSpeciesPenalty.text = val.ToString();
    }
}
