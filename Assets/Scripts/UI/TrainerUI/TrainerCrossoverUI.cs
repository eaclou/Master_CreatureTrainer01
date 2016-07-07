using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TrainerCrossoverUI : MonoBehaviour {

	public bool debugFunctionCalls = true;

	public TrainerModuleUI trainerModuleScript;
    public GameObject panelVisibility;
    
    public GameObject panelMutationPanel;
    public GameObject panelCrossoverPanel;
    public GameObject panelSpeciesPanel;
    public GameObject panelBodyPanel;

    public Button buttonMutationTab;
    public Button buttonCrossoverTab;
    public Button buttonSpeciesTab;
    public Button buttonBodyTab;

    // MUTATION PANEL!!!!:
    public Slider sliderMasterMutationRate;  // chance that an existing link will have a mutation
	public Text textMasterMutationRate;
	public Slider sliderMaximumWeight;  // multiplier on the randomly-chosen weight from a guassian distribution
	public Text textMaximumWeight;
	public Slider sliderMutationDriftScale;  // the maximum amount that a mutation can deviate from its pre-mutation value
	public Text textMutationDriftScale;
	public Slider sliderRemoveLinkChance;  // chance that an existing link will be deleted
	public Text textRemoveLinkChance;
	public Slider sliderAddLinkChance;  // chance that a brand new link will be created
	public Text textAddLinkChance;
    public Slider sliderRemoveNodeChance;   // chance that an exisitng node will be deleted, and its connections re-routed
    public Text textRemoveNodeChance;
    public Slider sliderAddNodeChance;  //  chance to split an existing link and insert a brand new neuron
    public Text textAddNodeChance;
    public Slider sliderFunctionChance;   //  chance that a neuron's activation function will be randomly changed
	public Text textFunctionChance;
    public Slider sliderLargeBrainPenalty;   // fitness penalty for brains with more nodes and connections
    public Text textLargeBrainPenalty;
    public Slider sliderNewLinkMutateBonus;   //  multiplier on the chance of weight-mutation for newly-created links (based on inno#)
    public Text textNewLinkMutateBonus;
    public Slider sliderNewLinkBonusDuration;  // the number of generations before the mutation bonus fades away completely -- lerp
    public Text textNewLinkBonusDuration;
    public Slider sliderExistingNetworkBias;   // the chance that when a new link is created, it is attached to a node that already has at least one connection 
    public Text textExistingNetworkBias;
    public Slider sliderExistingFromNodeBias;   // the chance that ExistingNetworkBias attaches to an incoming or outgoing link -- 0 = connects to existing input Node, 1 = connects to existing output node,  0.5 = randomly selects
    public Text textExistingFromNodeBias;

    // CROSSOVER PANEL!!!!!:
    public Slider sliderCrossoverRandomLinkChance;  // the chance that a connection gene will be chosen from a random parent rather than from the more-fit parent
    public Text textCrossoverRandomLinkChance;

    // SPECIES PANEL!!!!:
    public Slider sliderSimilarityThreshold;  // the similarity score below which an offspring matches an existing species template genome
    public Text textSimilarityThreshold;
    public Slider sliderExcessLinkWeight;   // how much excess link gene's count towards the similarityThreshold score
    public Text textExcessLinkWeight;
    public Slider sliderDisjointLinkWeight;   // how much disjoint link gene's count towards the similarityThreshold score
    public Text textDisjointLinkWeight;
    public Slider sliderLinkWeightWeight;   // how much link gene's connection weights count towards the similarityThreshold score
    public Text textLinkWeightWeight;
    public Slider sliderNormalizeExcess;   // how much the excessLinkGene score is adjusted by the total # of links in the genome
    public Text textNormalizeExcess;
    public Slider sliderNormalizeDisjoint;   // how much the disjointLinkGene score is adjusted by the total # of links in the genome
    public Text textNormalizeDisjoint;
    public Slider sliderNormalizeLinkWeights;   // how much the link weights similarity score is adjusted by the total # of links in the genome
    public Text textNormalizeLinkWeights;
    public Slider sliderAdoptionRate;    // the chance that a newly created offspring will check what species it belongs to -- otherwise it joins its parent's species automatically
    public Text textAdoptionRate;
    public Slider sliderSpeciesSizePenalty;   //  the penalty for a species containing a large number of members -- this is to encourage diversity by protecting smaller species
    public Text textSpeciesSizePenalty;
    public Slider sliderInterspeciesMatingRate;   //  the chance that an individual will be bred with a member of another species rather than its own
    public Text textInterspeciesMatingRate;

    // BODY PANEL!!!!:
    public Slider sliderMaxAttributeValueChange;  //
    public Text textMaxAttributeValueChange;
    public Slider sliderNewSegmentChance;  //
    public Text textNewSegmentChance;
    public Slider sliderRemoveSegmentChance;  //
    public Text textRemoveSegmentChance;
    public Slider sliderSegmentProportionChance;  //
    public Text textSegmentProportionChance;
    public Slider sliderSegmentAttachSettingsChance;  //
    public Text textSegmentAttachSettingsChance;
    public Slider sliderJointSettingsChance;  //
    public Text textJointSettingsChance;
    public Slider sliderNewAddonChance;  //
    public Text textNewAddonChance;
    public Slider sliderRemoveAddonChance;  //
    public Text textRemoveAddonChance;
    public Slider sliderAddonSettingsChance;  //
    public Text textAddonSettingsChance;
    public Slider sliderRecursionChance;  //
    public Text textRecursionChance;
    public Slider sliderSymmetryChance;  //
    public Text textSymmetryChance;

    // COMMON SETTINGS!:
    public Slider sliderSurvivalRate;
	public Text textSurvivalRate;
	public Toggle toggleSurvivalByRanking;
	public Toggle toggleSurvivalRandom;
	public Toggle toggleSurvivalByFitLottery;

	public Slider sliderBreedingRate;
	public Text textBreedingRate;
	public Toggle toggleBreedingByRanking;
	public Toggle toggleBreedingRandom;
	public Toggle toggleBreedingByFitLottery;

    public Toggle toggleMutation;
    public Toggle toggleCrossover;
    public Toggle toggleSpeciation;

    public Button buttonApply;
	public Button buttonCancel;
	public Image bgImage;

	private Player playerRef;
	public CrossoverManager pendingCrossoverManager;

	// UI Settings:
	
	private bool panelActive = false;  // requires valid population
    public bool mutationPanelOn = true;
    public bool crossoverPanelOn = false;
    public bool speciesPanelOn = false;
    public bool bodyPanelOn = false;

    public bool valuesChanged = false;
	public bool applyPressed = false;

	public void InitializePanelWithTrainerData() {
		DebugBot.DebugFunctionCall("TCrossoverUI; InitializePanelWithTrainerData(); ", debugFunctionCalls);
		//CheckActivationCriteria();


		// set population references:
		Trainer trainer = trainerModuleScript.gameController.masterTrainer;
		int curPlayer = trainer.CurPlayer;
		playerRef = trainer.PlayerList[curPlayer-1]; // get current population instance
		if(pendingCrossoverManager == null) {
			pendingCrossoverManager = new CrossoverManager();
		}
		pendingCrossoverManager.CopyFromSourceCrossoverManager(playerRef.masterCupid);

		// SET values from trainer data:
        // MUTATION TAB!!!!
		sliderMasterMutationRate.minValue = 0f; // set up slider bounds
		sliderMasterMutationRate.maxValue = 1f;
		sliderMaximumWeight.minValue = 0f; // set up slider bounds
		sliderMaximumWeight.maxValue = 5f;
		sliderMutationDriftScale.minValue = 0f; // set up slider bounds
		sliderMutationDriftScale.maxValue = 1f;
		sliderRemoveLinkChance.minValue = 0f; // set up slider bounds
		sliderRemoveLinkChance.maxValue = 1f;
		sliderAddLinkChance.minValue = 0f; // set up slider bounds
		sliderAddLinkChance.maxValue = 1f;
        sliderRemoveNodeChance.minValue = 0f; // set up slider bounds
        sliderRemoveNodeChance.maxValue = 1f;
        sliderAddNodeChance.minValue = 0f; // set up slider bounds
        sliderAddNodeChance.maxValue = 1f;
        sliderFunctionChance.minValue = 0f; // set up slider bounds
		sliderFunctionChance.maxValue = 1f;
        sliderLargeBrainPenalty.minValue = 0f; // set up slider bounds
        sliderLargeBrainPenalty.maxValue = 0.5f;
        sliderNewLinkMutateBonus.minValue = 1f;
        sliderNewLinkMutateBonus.maxValue = 10f;
        sliderNewLinkBonusDuration.minValue = 5;
        sliderNewLinkBonusDuration.maxValue = 50;
        sliderExistingNetworkBias.minValue = 0f;
        sliderExistingNetworkBias.maxValue = 1f;
        sliderExistingFromNodeBias.minValue = 0f;
        sliderExistingFromNodeBias.maxValue = 1f;
        
        // CROSSOVER TAB!!!
        sliderCrossoverRandomLinkChance.minValue = 0f;
        sliderCrossoverRandomLinkChance.maxValue = 1f;
        
        // SPECIES TAB!!!!!
        sliderSimilarityThreshold.minValue = 0f; // set up slider bounds
        sliderSimilarityThreshold.maxValue = 10f;
        sliderExcessLinkWeight.minValue = 0f;
        sliderExcessLinkWeight.maxValue = 1f;
        sliderDisjointLinkWeight.minValue = 0f;
        sliderDisjointLinkWeight.maxValue = 1f;
        sliderLinkWeightWeight.minValue = 0f;
        sliderLinkWeightWeight.maxValue = 1f;
        sliderNormalizeExcess.minValue = 0f;
        sliderNormalizeExcess.maxValue = 1f;
        sliderNormalizeDisjoint.minValue = 0f;
        sliderNormalizeDisjoint.maxValue = 1f;
        sliderNormalizeLinkWeights.minValue = 0f;
        sliderNormalizeLinkWeights.maxValue = 1f;
        sliderAdoptionRate.minValue = 0f; // set up slider bounds
        sliderAdoptionRate.maxValue = 1f;
        sliderSpeciesSizePenalty.minValue = 0f; // set up slider bounds
        sliderSpeciesSizePenalty.maxValue = 0.5f;
        sliderInterspeciesMatingRate.minValue = 0f; // set up slider bounds
        sliderInterspeciesMatingRate.maxValue = 1f;

        // BODY TAB!!!!
        sliderMaxAttributeValueChange.minValue = 1f;  // multiplier
        sliderMaxAttributeValueChange.maxValue = 5f;
        sliderNewSegmentChance.minValue = 0f;
        sliderNewSegmentChance.maxValue = 0.2f;
        sliderRemoveSegmentChance.minValue = 0f;
        sliderRemoveSegmentChance.maxValue = 0.2f;
        sliderSegmentProportionChance.minValue = 0f;
        sliderSegmentProportionChance.maxValue = 0.2f;
        sliderSegmentAttachSettingsChance.minValue = 0f;
        sliderSegmentAttachSettingsChance.maxValue = 0.2f;
        sliderJointSettingsChance.minValue = 0f;
        sliderJointSettingsChance.maxValue = 0.2f;
        sliderNewAddonChance.minValue = 0f;
        sliderNewAddonChance.maxValue = 0.2f;
        sliderRemoveAddonChance.minValue = 0f;
        sliderRemoveAddonChance.maxValue = 0.2f;
        sliderAddonSettingsChance.minValue = 0f;
        sliderAddonSettingsChance.maxValue = 0.2f;
        sliderRecursionChance.minValue = 0f;
        sliderSymmetryChance.maxValue = 0.2f;

        // COMMON!!!!
        sliderSurvivalRate.minValue = 0f; // set up slider bounds
		sliderSurvivalRate.maxValue = 1f;
		sliderBreedingRate.minValue = 0f; // set up slider bounds
		sliderBreedingRate.maxValue = 1f;
        // Need to add the Toggles here ?!?????????????????????????????????????????

		
		valuesChanged = false;
		applyPressed = false;
		
		UpdateUIWithCurrentData();
	}
	
	public void SetTrainerDataFromUIApply() {
		DebugBot.DebugFunctionCall("TCrossoverUI; SetTrainerDataFromUIApply(); ", debugFunctionCalls);
		//populationRef.SetMaxPopulationSize(pendingMaxPopulationSize);  // Set new max population Size
		playerRef.masterCupid.CopyFromSourceCrossoverManager(pendingCrossoverManager);
		InitializePanelWithTrainerData();
	}
	
	public void CheckActivationCriteria() {  // checks which buttons/elements should be active/inactive based on the current data
		DebugBot.DebugFunctionCall("TCrossoverUI; CheckActivationCriteria(); ", debugFunctionCalls);
		Trainer trainer = trainerModuleScript.gameController.masterTrainer;
		int curPlayer = trainer.CurPlayer;
		panelActive = false;
		if(trainer.PlayerList != null) {
			if(trainer.PlayerList[curPlayer-1].masterPopulation != null) {
				if(trainer.PlayerList[curPlayer-1].masterPopulation.isFunctional) {
					panelActive = true;
				}
			}
		}
	}
	
	public void UpdateUIElementStates() {
		DebugBot.DebugFunctionCall("TCrossoverUI; UpdateUIElementStates(); ", debugFunctionCalls);
		// Changing Button Displays !!
		
		// Active Population Options:
		if(panelActive) {
			panelVisibility.SetActive (true);
            // Sub-Panel toggles
            if (mutationPanelOn) {
                buttonMutationTab.interactable = false;
                buttonCrossoverTab.interactable = true;
                buttonSpeciesTab.interactable = true;
                buttonBodyTab.interactable = true;
                panelMutationPanel.SetActive(true);
                panelCrossoverPanel.SetActive(false);
                panelSpeciesPanel.SetActive(false);
                panelBodyPanel.SetActive(false);
            }
            if (crossoverPanelOn) {
                buttonMutationTab.interactable = true;
                buttonCrossoverTab.interactable = false;
                buttonSpeciesTab.interactable = true;
                buttonBodyTab.interactable = true;
                panelMutationPanel.SetActive(false);
                panelCrossoverPanel.SetActive(true);
                panelSpeciesPanel.SetActive(false);
                panelBodyPanel.SetActive(false);
            }
            if (speciesPanelOn) {
                buttonMutationTab.interactable = true;
                buttonCrossoverTab.interactable = true;
                buttonSpeciesTab.interactable = false;
                buttonBodyTab.interactable = true;
                panelMutationPanel.SetActive(false);
                panelCrossoverPanel.SetActive(false);
                panelSpeciesPanel.SetActive(true);
                panelBodyPanel.SetActive(false);
            }
            if (bodyPanelOn) {
                buttonMutationTab.interactable = true;
                buttonCrossoverTab.interactable = true;
                buttonSpeciesTab.interactable = true;
                buttonBodyTab.interactable = false;
                panelMutationPanel.SetActive(false);
                panelCrossoverPanel.SetActive(false);
                panelSpeciesPanel.SetActive(false);
                panelBodyPanel.SetActive(true);
            }
        }
		else {
            panelVisibility.SetActive (false);
		}
		
		if(valuesChanged) {
			buttonApply.interactable = true;
			buttonCancel.interactable = true;
		}
		else {
			buttonApply.interactable = false;
			buttonCancel.interactable = false;
		}
		if(applyPressed) {
			bgImage.color = new Color(0.99f, 0.75f, 0.6f);
		}
		else {
			bgImage.color = trainerModuleScript.defaultBGColor;
		}
	}
	
	public void UpdateUIWithCurrentData() {
		DebugBot.DebugFunctionCall("TCrossoverUI; UpdateUIWithCurrentData(); ", debugFunctionCalls);

        toggleBreedingByFitLottery.isOn = pendingCrossoverManager.breedingByRaffle;
        toggleBreedingByRanking.isOn = pendingCrossoverManager.breedingByRank;
        toggleBreedingRandom.isOn = pendingCrossoverManager.breedingStochastic;
        toggleCrossover.isOn = pendingCrossoverManager.useCrossover;
        toggleMutation.isOn = pendingCrossoverManager.useMutation;
        toggleSpeciation.isOn = pendingCrossoverManager.useSpeciation;
        toggleSurvivalByFitLottery.isOn = pendingCrossoverManager.survivalByRaffle;
        toggleSurvivalByRanking.isOn = pendingCrossoverManager.survivalByRank;
        toggleSurvivalRandom.isOn = pendingCrossoverManager.survivalStochastic;

        // MUTATION TAB!!!!
        sliderMasterMutationRate.value = pendingCrossoverManager.masterMutationRate;
		textMasterMutationRate.text = pendingCrossoverManager.masterMutationRate.ToString();
		sliderMaximumWeight.value = pendingCrossoverManager.maximumWeightMagnitude;
		textMaximumWeight.text = pendingCrossoverManager.maximumWeightMagnitude.ToString();
		sliderMutationDriftScale.value = pendingCrossoverManager.mutationDriftScale;
		textMutationDriftScale.text = pendingCrossoverManager.mutationDriftScale.ToString();
		sliderRemoveLinkChance.value = pendingCrossoverManager.mutationRemoveLinkChance;
		textRemoveLinkChance.text = pendingCrossoverManager.mutationRemoveLinkChance.ToString();
		sliderAddLinkChance.value = pendingCrossoverManager.mutationAddLinkChance;
		textAddLinkChance.text = pendingCrossoverManager.mutationAddLinkChance.ToString();
        sliderRemoveNodeChance.value = pendingCrossoverManager.mutationRemoveNodeChance;
        textRemoveNodeChance.text = pendingCrossoverManager.mutationRemoveNodeChance.ToString();
        sliderAddNodeChance.value = pendingCrossoverManager.mutationAddNodeChance;
        textAddNodeChance.text = pendingCrossoverManager.mutationAddNodeChance.ToString();
        sliderFunctionChance.value = pendingCrossoverManager.mutationActivationFunctionChance;
		textFunctionChance.text = pendingCrossoverManager.mutationActivationFunctionChance.ToString();
        sliderLargeBrainPenalty.value = pendingCrossoverManager.largeBrainPenalty;
        textLargeBrainPenalty.text = pendingCrossoverManager.largeBrainPenalty.ToString();
        sliderNewLinkMutateBonus.value = pendingCrossoverManager.newLinkMutateBonus;
        textNewLinkMutateBonus.text = pendingCrossoverManager.newLinkMutateBonus.ToString();
        sliderNewLinkBonusDuration.value = pendingCrossoverManager.newLinkBonusDuration;
        textNewLinkBonusDuration.text = pendingCrossoverManager.newLinkBonusDuration.ToString();
        sliderExistingNetworkBias.value = pendingCrossoverManager.existingNetworkBias;
        textExistingNetworkBias.text = pendingCrossoverManager.existingNetworkBias.ToString();
        sliderExistingFromNodeBias.value = pendingCrossoverManager.existingFromNodeBias;
        textExistingFromNodeBias.text = pendingCrossoverManager.existingFromNodeBias.ToString();
        // CROSSOVER TAB!!!
        sliderCrossoverRandomLinkChance.value = pendingCrossoverManager.crossoverRandomLinkChance;
        textCrossoverRandomLinkChance.text = pendingCrossoverManager.crossoverRandomLinkChance.ToString();
        // SPECIES TAB!!!!!
        sliderSimilarityThreshold.value = pendingCrossoverManager.speciesSimilarityThreshold;
        textSimilarityThreshold.text = pendingCrossoverManager.speciesSimilarityThreshold.ToString();
        sliderExcessLinkWeight.value = pendingCrossoverManager.excessLinkWeight;
        textExcessLinkWeight.text = pendingCrossoverManager.excessLinkWeight.ToString();
        sliderDisjointLinkWeight.value = pendingCrossoverManager.disjointLinkWeight;
        textDisjointLinkWeight.text = pendingCrossoverManager.disjointLinkWeight.ToString();
        sliderLinkWeightWeight.value = pendingCrossoverManager.linkWeightWeight;
        textLinkWeightWeight.text = pendingCrossoverManager.linkWeightWeight.ToString();
        sliderNormalizeExcess.value = pendingCrossoverManager.normalizeExcess;
        textNormalizeExcess.text = pendingCrossoverManager.normalizeExcess.ToString();
        sliderNormalizeDisjoint.value = pendingCrossoverManager.normalizeDisjoint;
        textNormalizeDisjoint.text = pendingCrossoverManager.normalizeDisjoint.ToString();
        sliderNormalizeLinkWeights.value = pendingCrossoverManager.normalizeLinkWeight;
        textNormalizeLinkWeights.text = pendingCrossoverManager.normalizeLinkWeight.ToString();
        sliderAdoptionRate.value = pendingCrossoverManager.adoptionRate;
        textAdoptionRate.text = pendingCrossoverManager.adoptionRate.ToString();
        sliderSpeciesSizePenalty.value = pendingCrossoverManager.largeSpeciesPenalty;
        textSpeciesSizePenalty.text = pendingCrossoverManager.largeSpeciesPenalty.ToString();
        sliderInterspeciesMatingRate.value = pendingCrossoverManager.interspeciesBreedingRate;
        textInterspeciesMatingRate.text = pendingCrossoverManager.interspeciesBreedingRate.ToString();

        // BODY TAB!!!!
        sliderMaxAttributeValueChange.value = pendingCrossoverManager.maxAttributeValueChange;
        textMaxAttributeValueChange.text = pendingCrossoverManager.maxAttributeValueChange.ToString();
        sliderNewSegmentChance.value = pendingCrossoverManager.newSegmentChance;
        textNewSegmentChance.text = pendingCrossoverManager.newSegmentChance.ToString();
        sliderRemoveSegmentChance.value = pendingCrossoverManager.removeSegmentChance;
        textRemoveSegmentChance.text = pendingCrossoverManager.removeSegmentChance.ToString();
        sliderSegmentProportionChance.value = pendingCrossoverManager.segmentProportionChance;
        textSegmentProportionChance.text = pendingCrossoverManager.segmentProportionChance.ToString();
        sliderSegmentAttachSettingsChance.value = pendingCrossoverManager.segmentAttachSettingsChance;
        textSegmentAttachSettingsChance.text = pendingCrossoverManager.segmentAttachSettingsChance.ToString();
        sliderJointSettingsChance.value = pendingCrossoverManager.jointSettingsChance;
        textJointSettingsChance.text = pendingCrossoverManager.jointSettingsChance.ToString();
        sliderNewAddonChance.value = pendingCrossoverManager.newAddonChance;
        textNewAddonChance.text = pendingCrossoverManager.newAddonChance.ToString();
        sliderRemoveAddonChance.value = pendingCrossoverManager.removeAddonChance;
        textRemoveAddonChance.text = pendingCrossoverManager.removeAddonChance.ToString();
        sliderAddonSettingsChance.value = pendingCrossoverManager.addonSettingsChance;
        textAddonSettingsChance.text = pendingCrossoverManager.addonSettingsChance.ToString();
        sliderRecursionChance.value = pendingCrossoverManager.recursionChance;
        textRecursionChance.text = pendingCrossoverManager.recursionChance.ToString();
        sliderSymmetryChance.value = pendingCrossoverManager.symmetryChance;
        textSymmetryChance.text = pendingCrossoverManager.symmetryChance.ToString();

        // COMMON!!!
		sliderSurvivalRate.value = pendingCrossoverManager.survivalRate; 
		textSurvivalRate.text = pendingCrossoverManager.survivalRate.ToString(); 
		sliderBreedingRate.value = pendingCrossoverManager.breedingRate; 
		textBreedingRate.text = pendingCrossoverManager.breedingRate.ToString(); 

		CheckActivationCriteria();
		UpdateUIElementStates();
	}

    #region UI FUNCTIONS

    #region ui functions MUTATION TAB!!
    public void SliderMasterMutationRate(float sliderValue) { // On Slider Value Changed
		DebugBot.DebugFunctionCall("TCrossoverUI; SliderMasterMutationRate(); ", debugFunctionCalls);
		pendingCrossoverManager.masterMutationRate = sliderValue;
		float dataMasterMutationRate = playerRef.masterCupid.masterMutationRate;
		if(pendingCrossoverManager.masterMutationRate != dataMasterMutationRate) {
			valuesChanged = true;
		}
		else {
			valuesChanged = false;
		}
		UpdateUIWithCurrentData();  // Will update text display of PENDING numPlayers value (NOT the applied value!)
	}
	public void SliderMaximumWeight(float sliderValue) { // On Slider Value Changed
		DebugBot.DebugFunctionCall("TCrossoverUI; SliderMaximumWeight(); ", debugFunctionCalls);
		pendingCrossoverManager.maximumWeightMagnitude = sliderValue;
		float dataWeightMagnitude = playerRef.masterCupid.maximumWeightMagnitude;
		if(pendingCrossoverManager.maximumWeightMagnitude != dataWeightMagnitude) {
			valuesChanged = true;
		}
		else {
			valuesChanged = false;
		}
		UpdateUIWithCurrentData();  // Will update text display of PENDING numPlayers value (NOT the applied value!)
	}
	public void SliderMutationDriftScale(float sliderValue) { // On Slider Value Changed
		DebugBot.DebugFunctionCall("TCrossoverUI; SliderMutationDriftScale(); ", debugFunctionCalls);
		pendingCrossoverManager.mutationDriftScale = sliderValue;
		float dataMutationDriftScale = playerRef.masterCupid.mutationDriftScale;
		if(pendingCrossoverManager.mutationDriftScale != dataMutationDriftScale) {
			valuesChanged = true;
		}
		else {
			valuesChanged = false;
		}
		UpdateUIWithCurrentData();  // Will update text display of PENDING numPlayers value (NOT the applied value!)
	}
	public void SliderRemoveLinkChance(float sliderValue) { // On Slider Value Changed
		DebugBot.DebugFunctionCall("TCrossoverUI; SliderRemoveLinkChance(); ", debugFunctionCalls);
		pendingCrossoverManager.mutationRemoveLinkChance = sliderValue;
		float dataRemoveLinkChance = playerRef.masterCupid.mutationRemoveLinkChance;
		if(pendingCrossoverManager.mutationRemoveLinkChance != dataRemoveLinkChance) {
			valuesChanged = true;
		}
		else {
			valuesChanged = false;
		}
		UpdateUIWithCurrentData();  // Will update text display of PENDING numPlayers value (NOT the applied value!)
	}
	public void SliderAddLinkChance(float sliderValue) { // On Slider Value Changed
		DebugBot.DebugFunctionCall("TCrossoverUI; SliderAddLinkChance(); ", debugFunctionCalls);
		pendingCrossoverManager.mutationAddLinkChance = sliderValue;
		float dataAddLinkChance = playerRef.masterCupid.mutationAddLinkChance;
		if(pendingCrossoverManager.mutationAddLinkChance != dataAddLinkChance) {
			valuesChanged = true;
		}
		else {
			valuesChanged = false;
		}
		UpdateUIWithCurrentData();  // Will update text display of PENDING numPlayers value (NOT the applied value!)
	}
    public void SliderRemoveNodeChance(float sliderValue) { // On Slider Value Changed
        DebugBot.DebugFunctionCall("TCrossoverUI; SliderRemoveNodeChance(); ", debugFunctionCalls);
        pendingCrossoverManager.mutationRemoveNodeChance = sliderValue;
        float dataRemoveNodeChance = playerRef.masterCupid.mutationRemoveNodeChance;
        if (pendingCrossoverManager.mutationRemoveNodeChance != dataRemoveNodeChance) {
            valuesChanged = true;
        }
        else {
            valuesChanged = false;
        }
        UpdateUIWithCurrentData();  // Will update text display of PENDING numPlayers value (NOT the applied value!)
    }
    public void SliderAddNodeChance(float sliderValue) { // On Slider Value Changed
        DebugBot.DebugFunctionCall("TCrossoverUI; SliderAddNodeChance(); ", debugFunctionCalls);
        pendingCrossoverManager.mutationAddNodeChance = sliderValue;
        float dataAddNodeChance = playerRef.masterCupid.mutationAddNodeChance;
        if (pendingCrossoverManager.mutationAddNodeChance != dataAddNodeChance) {
            valuesChanged = true;
        }
        else {
            valuesChanged = false;
        }
        UpdateUIWithCurrentData();  // Will update text display of PENDING numPlayers value (NOT the applied value!)
    }
    public void SliderFunctionChance(float sliderValue) { // On Slider Value Changed
		DebugBot.DebugFunctionCall("TCrossoverUI; SliderFunctionChance(); ", debugFunctionCalls);
		pendingCrossoverManager.mutationActivationFunctionChance = sliderValue;
		float dataFunctionChance = playerRef.masterCupid.mutationActivationFunctionChance;
		if(pendingCrossoverManager.mutationActivationFunctionChance != dataFunctionChance) {
			valuesChanged = true;
		}
		else {
			valuesChanged = false;
		}
		UpdateUIWithCurrentData();  // Will update text display of PENDING numPlayers value (NOT the applied value!)
	}
    public void SliderLargeBrainPenalty(float sliderValue) { // On Slider Value Changed
        DebugBot.DebugFunctionCall("TCrossoverUI; SliderLargeBrainPenalty(); ", debugFunctionCalls);
        pendingCrossoverManager.largeBrainPenalty = sliderValue;
        float dataLargeBrainPenalty = playerRef.masterCupid.largeBrainPenalty;
        if (pendingCrossoverManager.largeBrainPenalty != dataLargeBrainPenalty) {
            valuesChanged = true;
        }
        else {
            valuesChanged = false;
        }
        UpdateUIWithCurrentData();  // Will update text display of PENDING numPlayers value (NOT the applied value!)
    }
    public void SliderNewLinkMutateBonus(float sliderValue) { // On Slider Value Changed
        DebugBot.DebugFunctionCall("TCrossoverUI; SliderNewLinkMutateBonus(); ", debugFunctionCalls);
        pendingCrossoverManager.newLinkMutateBonus = sliderValue;
        float dataNewLinkMutateBonus = playerRef.masterCupid.newLinkMutateBonus;
        if (pendingCrossoverManager.newLinkMutateBonus != dataNewLinkMutateBonus) {
            valuesChanged = true;
        }
        else {
            valuesChanged = false;
        }
        UpdateUIWithCurrentData();  // Will update text display of PENDING numPlayers value (NOT the applied value!)
    }
    public void SliderNewLinkBonusDuration(float sliderValue) { // On Slider Value Changed
        DebugBot.DebugFunctionCall("TCrossoverUI; SliderNewLinkBonusDuration(); ", debugFunctionCalls);
        pendingCrossoverManager.newLinkBonusDuration = (int)sliderValue;
        int dataNewLinkBonusDuration = playerRef.masterCupid.newLinkBonusDuration;
        if (pendingCrossoverManager.newLinkBonusDuration != dataNewLinkBonusDuration) {
            valuesChanged = true;
        }
        else {
            valuesChanged = false;
        }
        UpdateUIWithCurrentData();  // Will update text display of PENDING numPlayers value (NOT the applied value!)
    }
    public void SliderExistingNetworkBias(float sliderValue) { // On Slider Value Changed
        DebugBot.DebugFunctionCall("TCrossoverUI; SliderExistingNetworkBias(); ", debugFunctionCalls);
        pendingCrossoverManager.existingNetworkBias = sliderValue;
        float dataExistingNetworkBias = playerRef.masterCupid.existingNetworkBias;
        if (pendingCrossoverManager.existingNetworkBias != dataExistingNetworkBias) {
            valuesChanged = true;
        }
        else {
            valuesChanged = false;
        }
        UpdateUIWithCurrentData();  // Will update text display of PENDING numPlayers value (NOT the applied value!)
    }
    public void SliderExistingFromNodeBias(float sliderValue) { // On Slider Value Changed
        DebugBot.DebugFunctionCall("TCrossoverUI; SliderExistingFromNodeBias(); ", debugFunctionCalls);
        pendingCrossoverManager.existingFromNodeBias = sliderValue;
        float dataExistingFromNodeBias = playerRef.masterCupid.existingFromNodeBias;
        if (pendingCrossoverManager.existingFromNodeBias != dataExistingFromNodeBias) {
            valuesChanged = true;
        }
        else {
            valuesChanged = false;
        }
        UpdateUIWithCurrentData();  // Will update text display of PENDING numPlayers value (NOT the applied value!)
    }
    #endregion

    #region ui functions CROSSOVER TAB
    public void SliderCrossoverRandomLinkChance(float sliderValue) { // On Slider Value Changed
        DebugBot.DebugFunctionCall("TCrossoverUI; SliderCrossoverRandomLinkChance(); ", debugFunctionCalls);
        pendingCrossoverManager.crossoverRandomLinkChance = sliderValue;
        float dataCrossoverRandomLinkChance = playerRef.masterCupid.crossoverRandomLinkChance;
        if (pendingCrossoverManager.crossoverRandomLinkChance != dataCrossoverRandomLinkChance) {
            valuesChanged = true;
        }
        else {
            valuesChanged = false;
        }
        UpdateUIWithCurrentData();  // Will update text display of PENDING numPlayers value (NOT the applied value!)
    }
    #endregion

    #region ui functions SPECIES TAB
    // SPECIATION:
    public void SliderSimilarityThreshold(float sliderValue) { // On Slider Value Changed
        DebugBot.DebugFunctionCall("TCrossoverUI; SliderSimilarityThreshold(); ", debugFunctionCalls);
        pendingCrossoverManager.speciesSimilarityThreshold = sliderValue;
        float dataSimilarityThreshold = playerRef.masterCupid.speciesSimilarityThreshold;
        if (pendingCrossoverManager.speciesSimilarityThreshold != dataSimilarityThreshold) {
            valuesChanged = true;
        }
        else {
            valuesChanged = false;
        }
        UpdateUIWithCurrentData();  // Will update text display of PENDING numPlayers value (NOT the applied value!)
    }
    public void SliderExcessLinkWeight(float sliderValue) { // On Slider Value Changed
        DebugBot.DebugFunctionCall("TCrossoverUI; SliderExcessLinkWeight(); ", debugFunctionCalls);
        pendingCrossoverManager.excessLinkWeight = sliderValue;
        float dataExcessLinkWeight = playerRef.masterCupid.excessLinkWeight;
        if (pendingCrossoverManager.excessLinkWeight != dataExcessLinkWeight) {
            valuesChanged = true;
        }
        else {
            valuesChanged = false;
        }
        UpdateUIWithCurrentData();  // Will update text display of PENDING numPlayers value (NOT the applied value!)
    }
    public void SliderDisjointLinkWeight(float sliderValue) { // On Slider Value Changed
        DebugBot.DebugFunctionCall("TCrossoverUI; SliderDisjointLinkWeight(); ", debugFunctionCalls);
        pendingCrossoverManager.disjointLinkWeight = sliderValue;
        float dataDisjointLinkWeight = playerRef.masterCupid.disjointLinkWeight;
        if (pendingCrossoverManager.disjointLinkWeight != dataDisjointLinkWeight) {
            valuesChanged = true;
        }
        else {
            valuesChanged = false;
        }
        UpdateUIWithCurrentData();  // Will update text display of PENDING numPlayers value (NOT the applied value!)
    }
    public void SliderLinkWeightWeight(float sliderValue) { // On Slider Value Changed
        DebugBot.DebugFunctionCall("TCrossoverUI; SliderLinkWeightWeight(); ", debugFunctionCalls);
        pendingCrossoverManager.linkWeightWeight = sliderValue;
        float dataLinkWeightWeight = playerRef.masterCupid.linkWeightWeight;
        if (pendingCrossoverManager.linkWeightWeight != dataLinkWeightWeight) {
            valuesChanged = true;
        }
        else {
            valuesChanged = false;
        }
        UpdateUIWithCurrentData();  // Will update text display of PENDING numPlayers value (NOT the applied value!)
    }
    public void SliderNormalizeExcess(float sliderValue) { // On Slider Value Changed
        DebugBot.DebugFunctionCall("TCrossoverUI; SliderNormalizeExcess(); ", debugFunctionCalls);
        pendingCrossoverManager.normalizeExcess = sliderValue;
        float dataNormalizeExcess = playerRef.masterCupid.normalizeExcess;
        if (pendingCrossoverManager.normalizeExcess != dataNormalizeExcess) {
            valuesChanged = true;
        }
        else {
            valuesChanged = false;
        }
        UpdateUIWithCurrentData();  // Will update text display of PENDING numPlayers value (NOT the applied value!)
    }
    public void SliderNormalizeDisjoint(float sliderValue) { // On Slider Value Changed
        DebugBot.DebugFunctionCall("TCrossoverUI; SliderNormalizeDisjoint(); ", debugFunctionCalls);
        pendingCrossoverManager.normalizeDisjoint = sliderValue;
        float dataNormalizeDisjoint = playerRef.masterCupid.normalizeDisjoint;
        if (pendingCrossoverManager.normalizeDisjoint != dataNormalizeDisjoint) {
            valuesChanged = true;
        }
        else {
            valuesChanged = false;
        }
        UpdateUIWithCurrentData();  // Will update text display of PENDING numPlayers value (NOT the applied value!)
    }
    public void SliderNormalizeLinkWeight(float sliderValue) { // On Slider Value Changed
        DebugBot.DebugFunctionCall("TCrossoverUI; SliderNormalizeLinkWeight(); ", debugFunctionCalls);
        pendingCrossoverManager.normalizeLinkWeight = sliderValue;
        float dataNormalizeLinkWeight = playerRef.masterCupid.normalizeLinkWeight;
        if (pendingCrossoverManager.normalizeLinkWeight != dataNormalizeLinkWeight) {
            valuesChanged = true;
        }
        else {
            valuesChanged = false;
        }
        UpdateUIWithCurrentData();  // Will update text display of PENDING numPlayers value (NOT the applied value!)
    }
    public void SliderAdoptionRate(float sliderValue) { // On Slider Value Changed
        DebugBot.DebugFunctionCall("TCrossoverUI; SliderAdoptionRate(); ", debugFunctionCalls);
        pendingCrossoverManager.adoptionRate = sliderValue;
        float dataAdoptionRate = playerRef.masterCupid.adoptionRate;
        if (pendingCrossoverManager.adoptionRate != dataAdoptionRate) {
            valuesChanged = true;
        }
        else {
            valuesChanged = false;
        }
        UpdateUIWithCurrentData();  // Will update text display of PENDING numPlayers value (NOT the applied value!)
    }
    public void SliderSpeciesSizePenalty(float sliderValue) { // On Slider Value Changed
        DebugBot.DebugFunctionCall("TCrossoverUI; SliderSpeciesSizePenalty(); ", debugFunctionCalls);
        pendingCrossoverManager.largeSpeciesPenalty = sliderValue;
        float dataSpeciesSizePenalty = playerRef.masterCupid.largeSpeciesPenalty;
        if (pendingCrossoverManager.largeSpeciesPenalty != dataSpeciesSizePenalty) {
            valuesChanged = true;
        }
        else {
            valuesChanged = false;
        }
        UpdateUIWithCurrentData();  // Will update text display of PENDING numPlayers value (NOT the applied value!)
    }
    public void SliderInterspeciesMatingRate(float sliderValue) { // On Slider Value Changed
        DebugBot.DebugFunctionCall("TCrossoverUI; InterspeciesMatingRate(); ", debugFunctionCalls);
        pendingCrossoverManager.interspeciesBreedingRate = sliderValue;
        float dataInterspeciesMatingRate = playerRef.masterCupid.interspeciesBreedingRate;
        if (pendingCrossoverManager.interspeciesBreedingRate != dataInterspeciesMatingRate) {
            valuesChanged = true;
        }
        else {
            valuesChanged = false;
        }
        UpdateUIWithCurrentData();  // Will update text display of PENDING numPlayers value (NOT the applied value!)
    }
    #endregion

    #region ui functions BODY TAB
    // BODY:
    public void SliderMaxAttributeValueChange(float sliderValue) { // On Slider Value Changed
        DebugBot.DebugFunctionCall("TCrossoverUI; SliderMaxAttributeValueChange(); ", debugFunctionCalls);
        pendingCrossoverManager.maxAttributeValueChange = sliderValue;
        float data = playerRef.masterCupid.maxAttributeValueChange;
        if (pendingCrossoverManager.maxAttributeValueChange != data) {
            valuesChanged = true;
        }
        else {
            valuesChanged = false;
        }
        UpdateUIWithCurrentData();  // Will update text display of PENDING numPlayers value (NOT the applied value!)
    }
    public void SliderNewSegmentChance(float sliderValue) { // On Slider Value Changed
        DebugBot.DebugFunctionCall("TCrossoverUI; SliderNewSegmentChance(); ", debugFunctionCalls);
        pendingCrossoverManager.newSegmentChance = sliderValue;
        float data = playerRef.masterCupid.newSegmentChance;
        if (pendingCrossoverManager.newSegmentChance != data) {
            valuesChanged = true;
        }
        else {
            valuesChanged = false;
        }
        UpdateUIWithCurrentData();  // Will update text display of PENDING numPlayers value (NOT the applied value!)
    }
    public void SliderRemoveSegmentChance(float sliderValue) { // On Slider Value Changed
        DebugBot.DebugFunctionCall("TCrossoverUI; SliderRemoveSegmentChance(); ", debugFunctionCalls);
        pendingCrossoverManager.removeSegmentChance = sliderValue;
        float data = playerRef.masterCupid.removeSegmentChance;
        if (pendingCrossoverManager.removeSegmentChance != data) {
            valuesChanged = true;
        }
        else {
            valuesChanged = false;
        }
        UpdateUIWithCurrentData();  // Will update text display of PENDING numPlayers value (NOT the applied value!)
    }
    public void SliderSegmentProportionChance(float sliderValue) { // On Slider Value Changed
        DebugBot.DebugFunctionCall("TCrossoverUI; SliderSegmentProportionChance(); ", debugFunctionCalls);
        pendingCrossoverManager.segmentProportionChance = sliderValue;
        float data = playerRef.masterCupid.segmentProportionChance;
        if (pendingCrossoverManager.segmentProportionChance != data) {
            valuesChanged = true;
        }
        else {
            valuesChanged = false;
        }
        UpdateUIWithCurrentData();  // Will update text display of PENDING numPlayers value (NOT the applied value!)
    }
    public void SliderSegmentAttachSettingsChance(float sliderValue) { // On Slider Value Changed
        DebugBot.DebugFunctionCall("TCrossoverUI; SliderSegmentAttachSettingsChance(); ", debugFunctionCalls);
        pendingCrossoverManager.segmentAttachSettingsChance = sliderValue;
        float data = playerRef.masterCupid.segmentAttachSettingsChance;
        if (pendingCrossoverManager.segmentAttachSettingsChance != data) {
            valuesChanged = true;
        }
        else {
            valuesChanged = false;
        }
        UpdateUIWithCurrentData();  // Will update text display of PENDING numPlayers value (NOT the applied value!)
    }
    public void SliderJointSettingsChance(float sliderValue) { // On Slider Value Changed
        DebugBot.DebugFunctionCall("TCrossoverUI; SliderJointSettingsChance(); ", debugFunctionCalls);
        pendingCrossoverManager.jointSettingsChance = sliderValue;
        float data = playerRef.masterCupid.jointSettingsChance;
        if (pendingCrossoverManager.jointSettingsChance != data) {
            valuesChanged = true;
        }
        else {
            valuesChanged = false;
        }
        UpdateUIWithCurrentData();  // Will update text display of PENDING numPlayers value (NOT the applied value!)
    }
    public void SliderNewAddonChance(float sliderValue) { // On Slider Value Changed
        DebugBot.DebugFunctionCall("TCrossoverUI; SliderNewAddonChance(); ", debugFunctionCalls);
        pendingCrossoverManager.newAddonChance = sliderValue;
        float data = playerRef.masterCupid.newAddonChance;
        if (pendingCrossoverManager.newAddonChance != data) {
            valuesChanged = true;
        }
        else {
            valuesChanged = false;
        }
        UpdateUIWithCurrentData();  // Will update text display of PENDING numPlayers value (NOT the applied value!)
    }
    public void SliderRemoveAddonChance(float sliderValue) { // On Slider Value Changed
        DebugBot.DebugFunctionCall("TCrossoverUI; SliderRemoveAddonChance(); ", debugFunctionCalls);
        pendingCrossoverManager.removeAddonChance = sliderValue;
        float data = playerRef.masterCupid.removeAddonChance;
        if (pendingCrossoverManager.removeAddonChance != data) {
            valuesChanged = true;
        }
        else {
            valuesChanged = false;
        }
        UpdateUIWithCurrentData();  // Will update text display of PENDING numPlayers value (NOT the applied value!)
    }
    public void SliderAddonSettingsChance(float sliderValue) { // On Slider Value Changed
        DebugBot.DebugFunctionCall("TCrossoverUI; SliderAddonSettingsChance(); ", debugFunctionCalls);
        pendingCrossoverManager.addonSettingsChance = sliderValue;
        float data = playerRef.masterCupid.addonSettingsChance;
        if (pendingCrossoverManager.addonSettingsChance != data) {
            valuesChanged = true;
        }
        else {
            valuesChanged = false;
        }
        UpdateUIWithCurrentData();  // Will update text display of PENDING numPlayers value (NOT the applied value!)
    }
    public void SliderRecursionChance(float sliderValue) { // On Slider Value Changed
        DebugBot.DebugFunctionCall("TCrossoverUI; SliderRecursionChance(); ", debugFunctionCalls);
        pendingCrossoverManager.recursionChance = sliderValue;
        float data = playerRef.masterCupid.recursionChance;
        if (pendingCrossoverManager.recursionChance != data) {
            valuesChanged = true;
        }
        else {
            valuesChanged = false;
        }
        UpdateUIWithCurrentData();  // Will update text display of PENDING numPlayers value (NOT the applied value!)
    }
    public void SliderSymmetryChance(float sliderValue) { // On Slider Value Changed
        DebugBot.DebugFunctionCall("TCrossoverUI; SliderSymmetryChance(); ", debugFunctionCalls);
        pendingCrossoverManager.symmetryChance = sliderValue;
        float data = playerRef.masterCupid.symmetryChance;
        if (pendingCrossoverManager.symmetryChance != data) {
            valuesChanged = true;
        }
        else {
            valuesChanged = false;
        }
        UpdateUIWithCurrentData();  // Will update text display of PENDING numPlayers value (NOT the applied value!)
    }
    #endregion

    public void SliderSurvivalRate(float sliderValue) { // On Slider Value Changed
		DebugBot.DebugFunctionCall("TCrossoverUI; SliderSurvivalRate(); ", debugFunctionCalls);
		pendingCrossoverManager.survivalRate = sliderValue;
		float dataSurvivalRate = playerRef.masterCupid.survivalRate;
		if(pendingCrossoverManager.survivalRate != dataSurvivalRate) {
			valuesChanged = true;
		}
		else {
			valuesChanged = false;
		}
		UpdateUIWithCurrentData();  // Will update text display of PENDING numPlayers value (NOT the applied value!)
	}
	public void SliderBreedingRate(float sliderValue) { // On Slider Value Changed
		DebugBot.DebugFunctionCall("TCrossoverUI; SliderBreedingRate(); ", debugFunctionCalls);
		pendingCrossoverManager.breedingRate = sliderValue;
		float dataBreedingRate = playerRef.masterCupid.breedingRate;
		if(pendingCrossoverManager.breedingRate != dataBreedingRate) {
			valuesChanged = true;
		}
		else {
			valuesChanged = false;
		}
		UpdateUIWithCurrentData();  // Will update text display of PENDING numPlayers value (NOT the applied value!)
	}
	public void ToggleSurvivalByRank(bool value) {
		DebugBot.DebugFunctionCall("TCrossoverUI; ToggleSurvivalByRank(); ", debugFunctionCalls);
		if(value) {  // if was off, turning on:
			pendingCrossoverManager.survivalByRank = true;
			pendingCrossoverManager.survivalStochastic = false;
			pendingCrossoverManager.survivalByRaffle = false;
			toggleSurvivalRandom.isOn = false;
			toggleSurvivalByFitLottery.isOn = false;
			valuesChanged = true;
		}
		else {
			if(pendingCrossoverManager.survivalByRank) {
				toggleSurvivalByRanking.isOn = true;
			}
		}
        UpdateUIWithCurrentData();
    }
	public void ToggleSurvivalStochastic(bool value) {
		DebugBot.DebugFunctionCall("TCrossoverUI; ToggleSurvivalStochastic(); ", debugFunctionCalls);
		if(value) {  // if was off, turning on:
			pendingCrossoverManager.survivalByRank = false;
			pendingCrossoverManager.survivalStochastic = true;
			pendingCrossoverManager.survivalByRaffle = false;
			toggleSurvivalByRanking.isOn = false;
			toggleSurvivalByFitLottery.isOn = false;
			valuesChanged = true;
		}
		else {
			if(pendingCrossoverManager.survivalStochastic) {
				toggleSurvivalRandom.isOn = true;
			}
		}
        UpdateUIWithCurrentData();
    }
	public void ToggleSurvivalByRaffle(bool value) {
		DebugBot.DebugFunctionCall("TCrossoverUI; ToggleSurvivalByRaffle(); ", debugFunctionCalls);
		if(value) {  // if was off, turning on:
			pendingCrossoverManager.survivalByRank = false;
			pendingCrossoverManager.survivalStochastic = false;
			pendingCrossoverManager.survivalByRaffle = true;
			toggleSurvivalByRanking.isOn = false;
			toggleSurvivalRandom.isOn = false;
			valuesChanged = true;
		}
		else {  // if was on and turning off:
			if(pendingCrossoverManager.survivalByRaffle) { // make sure it only affects the current toggle being turned off -- 
				// before this was being triggered by the other toggles being switched off programattically, creating infinite loop
				toggleSurvivalByFitLottery.isOn = true;
			}
		}
        UpdateUIWithCurrentData();
    }
	public void ToggleBreedingByRank(bool value) {
		DebugBot.DebugFunctionCall("TCrossoverUI; ToggleBreedingByRank(); ", debugFunctionCalls);
		if(value) {  // if was off, turning on:
			pendingCrossoverManager.breedingByRank = true;
			pendingCrossoverManager.breedingStochastic = false;
			pendingCrossoverManager.breedingByRaffle = false;
			//toggleBreedingByRanking.isOn = true;
			toggleBreedingRandom.isOn = false;
			toggleBreedingByFitLottery.isOn = false;
			valuesChanged = true;
		}
		else {
			if(pendingCrossoverManager.breedingByRank) {
				toggleBreedingByRanking.isOn = true;
			}
		}
        UpdateUIWithCurrentData();
    }
	public void ToggleBreedingStochastic(bool value) {
		DebugBot.DebugFunctionCall("TCrossoverUI; ToggleBreedingStochastic(); ", debugFunctionCalls);
		if(value) {  // if was off, turning on:
			pendingCrossoverManager.breedingByRank = false;
			pendingCrossoverManager.breedingStochastic = true;
			pendingCrossoverManager.breedingByRaffle = false;
			toggleBreedingByRanking.isOn = false;
			toggleBreedingByFitLottery.isOn = false;
			valuesChanged = true;
		}
		else {
			if(pendingCrossoverManager.breedingStochastic) {
				toggleBreedingRandom.isOn = true;
			}
		}
        UpdateUIWithCurrentData();
    }
	public void ToggleBreedingByRaffle(bool value) {
		DebugBot.DebugFunctionCall("TCrossoverUI; ToggleBreedingByRaffle(); ", debugFunctionCalls);
		if(value) {  // if was off, turning on:
			pendingCrossoverManager.breedingByRank = false;
			pendingCrossoverManager.breedingStochastic = false;
			pendingCrossoverManager.breedingByRaffle = true;
			toggleBreedingByRanking.isOn = false;
			toggleBreedingRandom.isOn = false;
			valuesChanged = true;
		}
		else {
			if(pendingCrossoverManager.breedingByRaffle) {
				toggleBreedingByFitLottery.isOn = true;
			}
		}
        UpdateUIWithCurrentData();
    }
    public void ToggleMutation(bool value) {
        DebugBot.DebugFunctionCall("TCrossoverUI; ToggleMutation(" + value.ToString() + "); ", debugFunctionCalls);
        pendingCrossoverManager.useMutation = value;
        valuesChanged = true;
        UpdateUIWithCurrentData();
    }
    public void ToggleCrossover(bool value) {
        DebugBot.DebugFunctionCall("TCrossoverUI; ToggleCrossover(" + value.ToString() + "); ", debugFunctionCalls);
        pendingCrossoverManager.useCrossover = value;
        valuesChanged = true;
        UpdateUIWithCurrentData();
    }
    public void ToggleSpeciation(bool value) {
        DebugBot.DebugFunctionCall("TCrossoverUI; ToggleSpeciation(" + value.ToString() + "); ", debugFunctionCalls);
        pendingCrossoverManager.useSpeciation = value;
        valuesChanged = true;
        UpdateUIWithCurrentData();
    }

    public void ClickMutationPanel() {
        DebugBot.DebugFunctionCall("TCrossoverUI; ClickMutationPanel(); ", debugFunctionCalls);
        mutationPanelOn = true;
        crossoverPanelOn = false;
        speciesPanelOn = false;
        bodyPanelOn = false;
        UpdateUIWithCurrentData();
    }
    public void ClickCrossoverPanel() {
        DebugBot.DebugFunctionCall("TCrossoverUI; ClickCrossoverPanel(); ", debugFunctionCalls);
        mutationPanelOn = false;
        crossoverPanelOn = true;
        speciesPanelOn = false;
        bodyPanelOn = false;
        UpdateUIWithCurrentData();
    }
    public void ClickSpeciesPanel() {
        DebugBot.DebugFunctionCall("TCrossoverUI; ClickSpeciesPanel(); ", debugFunctionCalls);
        mutationPanelOn = false;
        crossoverPanelOn = false;
        speciesPanelOn = true;
        bodyPanelOn = false;
        UpdateUIWithCurrentData();
    }
    public void ClickBodyPanel() {
        DebugBot.DebugFunctionCall("TCrossoverUI; ClickBodyPanel(); ", debugFunctionCalls);
        mutationPanelOn = false;
        crossoverPanelOn = false;
        speciesPanelOn = false;
        bodyPanelOn = true;
        UpdateUIWithCurrentData();
    }

    public void ClickApply() {
		DebugBot.DebugFunctionCall("TCrossoverUI; ClickApply(); ", debugFunctionCalls);
		applyPressed = true;
		UpdateUIElementStates();  // change background color to indicate pending changes
		Trainer trainer = trainerModuleScript.gameController.masterTrainer;
		
		// !!!!!!!!!!! TEMPORARY !!!!!! Replace this code once play/pause/fastMode etc. are in and the Trainer class will trigger this when ApplyCriteria are met
		if(trainer.applyPanelCrossover) {  // if apply criteria are met currently:
			SetTrainerDataFromUIApply();
		}
		//DebugFunctionCall("ClickApply()");
	}
	public void ClickCancel() {
		DebugBot.DebugFunctionCall("TCrossoverUI; ClickCancel(); ", debugFunctionCalls);
		InitializePanelWithTrainerData();
		//DebugFunctionCall("ClickCancel()");
	}
    #endregion
}
