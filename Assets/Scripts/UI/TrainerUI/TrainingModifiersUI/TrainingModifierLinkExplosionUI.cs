using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TrainingModifierLinkExplosionUI : MonoBehaviour {

    public Slider sliderLinksPerNode;
    public Text textLinksPerNode;
    public Slider sliderNodesPerLink;
    public Text textNodesPerLink;

	// Use this for initialization
	void Start () {
        sliderLinksPerNode.minValue = 0f;
        sliderLinksPerNode.maxValue = 1f;
        sliderLinksPerNode.value = 0f;  // DEFAULT
        textLinksPerNode.text = sliderLinksPerNode.value.ToString();

        sliderNodesPerLink.minValue = 0f;
        sliderNodesPerLink.maxValue = 1f;
        sliderNodesPerLink.value = 0f;  // DEFAULT
        textNodesPerLink.text = sliderNodesPerLink.value.ToString();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SliderLinksPerNode(float val) {

        textLinksPerNode.text = val.ToString();
    }

    public void SliderNodesPerLink(float val) {

        textNodesPerLink.text = val.ToString();
    }
}
