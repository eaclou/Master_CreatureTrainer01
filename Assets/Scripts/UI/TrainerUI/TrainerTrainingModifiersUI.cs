using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TrainerTrainingModifiersUI : MonoBehaviour {

    public TrainerModuleUI trainerModuleScript;

    public GameObject panelVisibility;
    public GameObject panelActiveModifiers;
    public GameObject panelAddNewModifier;
    public Transform panelActiveModifierList;
    public Transform panelModifierDisplay;

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

    public GameObject currentModifierPrefabGO;

    public bool panelActive = true;
    public bool activeModifierPanelOn = true;
    public bool addNewModifierPanelOn = false;

    public TrainingModifier.TrainingModifierType currentModifierType = TrainingModifier.TrainingModifierType.LinkExplosion;

    //public List<TrainingModifier> activeTrainingModifiersList;

    public void InitializePanelWithTrainerData() {
        UpdateUIWithCurrentData();
    }

    public void UpdateUIElementStates() {
        
        // Changing Button Displays !!
        if (panelActive) {
            panelVisibility.SetActive(true);
        }
        else {
            panelVisibility.SetActive(false);
        }

        // Inputs / Outputs / Options toggles
        if (activeModifierPanelOn) {
            panelActiveModifiers.SetActive(true);
            panelAddNewModifier.SetActive(false);
        }
        if (addNewModifierPanelOn) {
            panelActiveModifiers.SetActive(false);
            panelAddNewModifier.SetActive(true);
        }        
    }

    public void RepopulateActiveModifiersList() {
        // looks at list of currently active TrainingModifiers and creates a UI panel with basic info about them

        foreach (Transform child in panelActiveModifierList) {  // clear current stuff
            GameObject.Destroy(child.gameObject);
        }

        // grab a reference to the Trainer's active Modifiers list
        List<TrainingModifier> activeTrainingModifiersList = trainerModuleScript.gameController.masterTrainer.trainingModifierManager.activeTrainingModifierList;

        int modifierDisplayIndex = 0;
        if(activeTrainingModifiersList != null) {
            for (int i = 0; i < activeTrainingModifiersList.Count; i++) {

                GameObject itemDisplayGO = (GameObject)Instantiate(prefabTrainingModifierRowUI);
                TrainerTrainingModifierRowUI itemDisplay = itemDisplayGO.GetComponent<TrainerTrainingModifierRowUI>();
                itemDisplay.trainerTrainingModifiersUI = this;
                itemDisplay.index = modifierDisplayIndex;
                modifierDisplayIndex++;
                itemDisplay.trainingModifierType = activeTrainingModifiersList[i].modifierType;
                itemDisplay.textHeader.text = itemDisplay.trainingModifierType.ToString();
                itemDisplay.textInfo.text = "Start: " + activeTrainingModifiersList[i].startGen.ToString() + ", Duration: " + activeTrainingModifiersList[i].duration.ToString();
                itemDisplayGO.transform.SetParent(panelActiveModifierList);
                /*itemDisplay.panelAddonsList = this;
                itemDisplay.index = addonDisplayIndex;
                addonDisplayIndex++;
                itemDisplay.sourceAddonType = CritterNodeAddonBase.CritterNodeAddonTypes.PhysicalAttributes;
                itemDisplay.sourceAddonIndex = physicalAttributesIndex;
                itemDisplayGO.transform.SetParent(this.transform);
                itemDisplay.Prime(sourceGenome);
                */
            }
        }
    }

    public void UpdateAddNewModifiersPanel() {
        textCurrentAddModifierType.text = currentModifierType.ToString();
        foreach (Transform child in panelModifierDisplay) {  // clear current stuff
            GameObject.Destroy(child.gameObject);
        }
        GameObject modifierDisplayGO;
        switch (currentModifierType) {
            case TrainingModifier.TrainingModifierType.LinkExplosion:                
                modifierDisplayGO = (GameObject)Instantiate(prefabModifierLinkExplosion);
                modifierDisplayGO.transform.SetParent(panelModifierDisplay);                
                // Initialize values in the Awake() function of the created prefab's script?
                break;

            case TrainingModifier.TrainingModifierType.MutationBlast:
                modifierDisplayGO = (GameObject)Instantiate(prefabModifierMutationBlast);
                modifierDisplayGO.transform.SetParent(panelModifierDisplay);
                break;

            case TrainingModifier.TrainingModifierType.PruneBrain:
                modifierDisplayGO = (GameObject)Instantiate(prefabModifierPruneBrain);
                modifierDisplayGO.transform.SetParent(panelModifierDisplay);
                break;

            case TrainingModifier.TrainingModifierType.TargetCone:
                modifierDisplayGO = (GameObject)Instantiate(prefabModifierTargetCone);
                modifierDisplayGO.transform.SetParent(panelModifierDisplay);
                break;

            case TrainingModifier.TrainingModifierType.TargetForward:
                modifierDisplayGO = (GameObject)Instantiate(prefabModifierTargetForward);
                modifierDisplayGO.transform.SetParent(panelModifierDisplay);
                break;

            case TrainingModifier.TrainingModifierType.TargetOmni:
                modifierDisplayGO = (GameObject)Instantiate(prefabModifierTargetOmni);
                modifierDisplayGO.transform.SetParent(panelModifierDisplay);
                break;

            case TrainingModifier.TrainingModifierType.VariableTrialTimes:
                modifierDisplayGO = (GameObject)Instantiate(prefabModifierVariableTrialTimes);
                modifierDisplayGO.transform.SetParent(panelModifierDisplay);
                break;

            case TrainingModifier.TrainingModifierType.WideSearch:
                modifierDisplayGO = (GameObject)Instantiate(prefabModifierWideSearch);
                modifierDisplayGO.transform.SetParent(panelModifierDisplay);
                break;

            default:
                Debug.Log("Modifier type not found!!! SWITCH DEFAULT CASE");
                modifierDisplayGO = (GameObject)Instantiate(prefabModifierWideSearch);
                break;
        }
        currentModifierPrefabGO = modifierDisplayGO;
    }

    public void UpdateUIWithCurrentData() {
        if(addNewModifierPanelOn) {
            UpdateAddNewModifiersPanel();
        }
        if(activeModifierPanelOn) {
            RepopulateActiveModifiersList();
        }
        
        //CheckActivationCriteria();
        UpdateUIElementStates();
    }

    public void RemoveModifier(int index) {
        trainerModuleScript.gameController.masterTrainer.trainingModifierManager.activeTrainingModifierList.RemoveAt(index);
        UpdateUIWithCurrentData();
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ClickActiveModifierPanel() {
        activeModifierPanelOn = true;
        addNewModifierPanelOn = false;
        UpdateUIWithCurrentData();
    }

    public void ClickAddNewModifierPanel() {
        activeModifierPanelOn = false;
        addNewModifierPanelOn = true;
        UpdateUIWithCurrentData();
    }

    public void ClickAddCurrentModifier() {

        TrainingModifier newModifier = new TrainingModifier();
        newModifier.modifierType = currentModifierType;
        newModifier.startGen = trainerModuleScript.gameController.masterTrainer.PlayingCurGeneration;
        
        switch (currentModifierType) {
            case TrainingModifier.TrainingModifierType.LinkExplosion:
                TrainingModifierLinkExplosionUI linkExplosionSettings = currentModifierPrefabGO.GetComponent<TrainingModifierLinkExplosionUI>();
                newModifier.linksPerNode = linkExplosionSettings.sliderLinksPerNode.value;
                newModifier.nodesPerLink = linkExplosionSettings.sliderNodesPerLink.value;
                break;

            case TrainingModifier.TrainingModifierType.MutationBlast:
                TrainingModifierMutationBlastUI mutationBlastSettings = currentModifierPrefabGO.GetComponent<TrainingModifierMutationBlastUI>();
                newModifier.duration = (int)mutationBlastSettings.sliderDuration.value;
                newModifier.liveForever = mutationBlastSettings.toggleLiveForever.isOn;
                break;

            case TrainingModifier.TrainingModifierType.PruneBrain:
                TrainingModifierPruneBrainUI pruneBrainSettings = currentModifierPrefabGO.GetComponent<TrainingModifierPruneBrainUI>();
                newModifier.duration = (int)pruneBrainSettings.sliderDuration.value;
                newModifier.largeBrainPenalty = pruneBrainSettings.sliderLargeBrainPenalty.value;
                newModifier.removeLinkChance = pruneBrainSettings.sliderRemoveLinkChance.value;
                newModifier.removeNodeChance = pruneBrainSettings.sliderRemoveNodeChance.value;
                newModifier.liveForever = pruneBrainSettings.toggleLiveForever.isOn;
                newModifier.decayEffectOverDuration = pruneBrainSettings.toggleDecayEffectOverDuration.isOn;
                break;

            case TrainingModifier.TrainingModifierType.TargetCone:
                TrainingModifierTargetConeUI targetConeSettings = currentModifierPrefabGO.GetComponent<TrainingModifierTargetConeUI>();
                newModifier.duration = (int)targetConeSettings.sliderDuration.value;
                newModifier.numRounds = (int)targetConeSettings.sliderNumRounds.value;
                newModifier.beginMinDistance = targetConeSettings.sliderBeginMinDistance.value;
                newModifier.beginMaxDistance = targetConeSettings.sliderBeginMaxDistance.value;
                newModifier.endMinDistance = targetConeSettings.sliderEndMinDistance.value;
                newModifier.endMaxDistance = targetConeSettings.sliderEndMaxDistance.value;
                newModifier.beginMinAngle = targetConeSettings.sliderBeginMinAngle.value;
                newModifier.beginMaxAngle = targetConeSettings.sliderBeginMaxAngle.value;
                newModifier.endMinAngle = targetConeSettings.sliderEndMinAngle.value;
                newModifier.endMaxAngle = targetConeSettings.sliderEndMaxAngle.value;
                newModifier.liveForever = targetConeSettings.toggleLiveForever.isOn;
                newModifier.horizontal = targetConeSettings.toggleHorizontal.isOn;
                newModifier.vertical = targetConeSettings.toggleVertical.isOn;
                break;

            case TrainingModifier.TrainingModifierType.TargetForward:
                TrainingModifierTargetForwardUI targetForwardSettings = currentModifierPrefabGO.GetComponent<TrainingModifierTargetForwardUI>();
                newModifier.duration = (int)targetForwardSettings.sliderDuration.value;
                newModifier.numRounds = (int)targetForwardSettings.sliderNumRounds.value;
                newModifier.beginMinDistance = targetForwardSettings.sliderBeginMinDistance.value;
                newModifier.beginMaxDistance = targetForwardSettings.sliderBeginMaxDistance.value;
                newModifier.endMinDistance = targetForwardSettings.sliderEndMinDistance.value;
                newModifier.endMaxDistance = targetForwardSettings.sliderEndMaxDistance.value;
                newModifier.liveForever = targetForwardSettings.toggleLiveForever.isOn;
                break;

            case TrainingModifier.TrainingModifierType.TargetOmni:
                TrainingModifierTargetOmniUI targetOmniSettings = currentModifierPrefabGO.GetComponent<TrainingModifierTargetOmniUI>();
                newModifier.duration = (int)targetOmniSettings.sliderDuration.value;
                newModifier.numRounds = (int)targetOmniSettings.sliderNumRounds.value;
                newModifier.beginMinDistance = targetOmniSettings.sliderBeginMinDistance.value;
                newModifier.beginMaxDistance = targetOmniSettings.sliderBeginMaxDistance.value;
                newModifier.endMinDistance = targetOmniSettings.sliderEndMinDistance.value;
                newModifier.endMaxDistance = targetOmniSettings.sliderEndMaxDistance.value;
                newModifier.liveForever = targetOmniSettings.toggleLiveForever.isOn;
                newModifier.forward = targetOmniSettings.toggleForward.isOn;
                newModifier.horizontal = targetOmniSettings.toggleHorizontal.isOn;
                newModifier.vertical = targetOmniSettings.toggleVertical.isOn;
                break;

            case TrainingModifier.TrainingModifierType.VariableTrialTimes:
                TrainingModifierVariableTrialTimesUI trialTimesSettings = currentModifierPrefabGO.GetComponent<TrainingModifierVariableTrialTimesUI>();
                newModifier.duration = (int)trialTimesSettings.sliderDuration.value;
                newModifier.beginMinTime = trialTimesSettings.sliderBeginMinTime.value;
                newModifier.beginMaxTime = trialTimesSettings.sliderBeginMaxTime.value;
                newModifier.endMinTime = trialTimesSettings.sliderEndMinTime.value;
                newModifier.endMaxTime = trialTimesSettings.sliderEndMaxTime.value;
                newModifier.liveForever = trialTimesSettings.toggleLiveForever.isOn;
                break;

            case TrainingModifier.TrainingModifierType.WideSearch:
                TrainingModifierWideSearchUI wideSearchSettings = currentModifierPrefabGO.GetComponent<TrainingModifierWideSearchUI>();
                newModifier.duration = (int)wideSearchSettings.sliderDuration.value;
                newModifier.speciesSimilarityThreshold = wideSearchSettings.sliderSimilarityThreshold.value;
                newModifier.adoptionRate = wideSearchSettings.sliderAdoptionRate.value;
                newModifier.largeSpeciesPenalty = wideSearchSettings.sliderLargeSpeciesPenalty.value;
                newModifier.liveForever = wideSearchSettings.toggleLiveForever.isOn;
                newModifier.decayEffectOverDuration = wideSearchSettings.toggleDecayEffectOverDuration.isOn;
                break;

            default:
                Debug.Log("Modifier type not found!!! SWITCH DEFAULT CASE");
                break;
        }

        trainerModuleScript.gameController.masterTrainer.trainingModifierManager.activeTrainingModifierList.Add(newModifier);
        Debug.Log("new Modifier! " + newModifier.modifierType.ToString() + newModifier.startGen.ToString() + ", #: " + trainerModuleScript.gameController.masterTrainer.trainingModifierManager.activeTrainingModifierList.Count.ToString());
    }

    public void ClickPrevModifier() {
        int numPages = System.Enum.GetNames(typeof(TrainingModifier.TrainingModifierType)).Length;
        int curPage = (int)currentModifierType;
        int newPage = curPage - 1;
        if (newPage < 0) {
            newPage = numPages - 1;
        }
        currentModifierType = (TrainingModifier.TrainingModifierType)newPage;
        UpdateUIWithCurrentData();
    }
    public void ClickNextModifier() {
        int numPages = System.Enum.GetNames(typeof(TrainingModifier.TrainingModifierType)).Length;
        int curPage = (int)currentModifierType;
        int newPage = curPage + 1;
        if (newPage >= numPages) {
            newPage = 0;
        }
        currentModifierType = (TrainingModifier.TrainingModifierType)newPage;
        UpdateUIWithCurrentData();
    }

}
