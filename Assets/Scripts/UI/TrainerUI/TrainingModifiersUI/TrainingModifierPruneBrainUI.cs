using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TrainingModifierPruneBrainUI : MonoBehaviour {

    public Slider sliderDuration;
    public Text textDuration;
    public Slider sliderLargeBrainPenalty;
    public Text textLargeBrainPenalty;
    public Slider sliderRemoveLinkChance;
    public Text textRemoveLinkChance;
    public Slider sliderRemoveNodeChance;
    public Text textRemoveNodeChance;
    public Toggle toggleLiveForever;
    public Toggle toggleDecayEffectOverDuration;

	// Use this for initialization
	void Start () {
        sliderDuration.minValue = 2f;
        sliderDuration.maxValue = 500f;
        sliderDuration.value = 0f;  // DEFAULT
        textDuration.text = sliderDuration.value.ToString();

        sliderLargeBrainPenalty.minValue = 0f;
        sliderLargeBrainPenalty.maxValue = 0.5f;
        sliderLargeBrainPenalty.value = 0f;  // DEFAULT
        textLargeBrainPenalty.text = sliderLargeBrainPenalty.value.ToString();

        sliderRemoveLinkChance.minValue = 0f;
        sliderRemoveLinkChance.maxValue = 1f;
        sliderRemoveLinkChance.value = 0f;  // DEFAULT
        textRemoveLinkChance.text = sliderRemoveLinkChance.value.ToString();

        sliderRemoveNodeChance.minValue = 0f;
        sliderRemoveNodeChance.maxValue = 1f;
        sliderRemoveNodeChance.value = 0f;  // DEFAULT
        textRemoveNodeChance.text = sliderRemoveNodeChance.value.ToString();

        toggleLiveForever.isOn = true;

        toggleDecayEffectOverDuration.isOn = true;
    }

    public void SliderDuration(float val) {
        textDuration.text = val.ToString();
    }
    public void SliderLargeBrainPenalty(float val) {
        textLargeBrainPenalty.text = val.ToString();
    }
    public void SliderRemoveLinkChance(float val) {
        textRemoveLinkChance.text = val.ToString();
    }
    public void SliderRemoveNodeChance(float val) {
        textRemoveNodeChance.text = val.ToString();
    }
}
