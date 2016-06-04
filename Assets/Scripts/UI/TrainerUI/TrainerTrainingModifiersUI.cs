using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TrainerTrainingModifiersUI : MonoBehaviour {

    public GameObject panelVisibility;
    public GameObject panelActiveModifiers;
    public GameObject panelAddNewModifier;
    public GameObject panelActiveModifierList;

    public Button buttonNewModifierTab;
    public Button buttonActiveModifierTab;

    public Text textCurrentAddModifierType;
    public Button buttonPrevModifier;
    public Button buttonNextModifier;
    public Button buttonAddCurrentModifier;

    public GameObject prefabTrainingModifierRowUI;
    public GameObject prefabModifierLinkExplosion;
    public GameObject prefabModifierMutationBlast;
    public GameObject prefabModifierPruneBrain;
    public GameObject prefabModifierTargetCone;
    public GameObject prefabModifierTargetForward;
    public GameObject prefabModifierTargetOmni;
    public GameObject prefabModifierVariableTrialTimes;
    public GameObject prefabModifierWideSearch;
    
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
