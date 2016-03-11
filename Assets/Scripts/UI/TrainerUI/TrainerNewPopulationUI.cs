using UnityEngine;
//using UnityEditor;
using System.Collections;
using UnityEngine.UI;

public class TrainerNewPopulationUI : MonoBehaviour {

	public bool debugFunctionCalls = true;

	public TrainerModuleUI trainerModuleScript;
	public Slider sliderPopulationSize;
	public Text textPopulationSize;
	public Slider sliderNumInputs;
	public Text textNumInputs;
	public Slider sliderNumOutputs;
	public Text textNumOutputs;
	public Button buttonBrainTypeDropDown;
	public Text textButtonBrainTypeDropDown;
	public Transform dropDownChooseBrainTypePanel;
	[SerializeField] GameObject buttonBrainTypeDropDownPrefab;
	public GameObject panelBrainTypeOptionsVisible;
	public Transform transformBrainTypeOptionsPlacement; // needed?
	public GameObject panelTransferFunctionsVisible;

	public Button buttonLoadBodyTemplate;
	public Text textLoadedBodyTemplateName;

	public Button buttonBrainTypeOptionsPanel;
	public Button buttonTransferFunctionOptionsPanel;
	public Toggle toggleLinear;
	public Toggle toggleSigmoid;
	public Toggle toggleStep;
	public Toggle toggleSin;
	public Toggle toggleCos;
	public Toggle toggleTan;
	public Toggle toggleAbs;
	public Toggle toggleExp;
	public Toggle toggleLog;
	public Toggle toggleInverse;
	public Toggle toggleRandom;
	public Toggle toggleConstant;
	public Toggle toggleRandomWeights;
	public Toggle toggleZeroedWeights;
	public Button buttonCreateNewPopulation;
	public Image bgImage;

	private bool newSettingsValid = false;

	public bool brainTypeOptionsPanelOn = true;
	private bool dropDownPopulated = false;

	// UI Settings:
	private int minMaxPopulationSize = 1;
	private int maxMaxPopulationSize = 100;
	//public int maxPopulationSize = 16;
	private int minBrainInputs = 0;
	private int maxBrainInputs = 20;
	private int minBrainOutputs = 0;
	private int maxBrainOutputs = 20;

	private int pendingPopulationSize = 40;
	private int pendingNumInputs = 4;
	private int pendingNumOutputs = 4;

	private string pendingBodyTemplateName = "";

	private Population populationRef;


	public void InitializePanelWithTrainerData() {

		Player currentPlayer = trainerModuleScript.gameController.masterTrainer.PlayerList[trainerModuleScript.gameController.masterTrainer.CurPlayer-1];
		populationRef = currentPlayer.masterPopulation;
		if(populationRef.templateBrain == null) {
			populationRef.templateBrain = new BrainBase();
			
			//InitializePanelInputList();
			//InitializePanelOutputList();
		}

		if(!dropDownPopulated) {  // So it only creates buttons on startup
			foreach (string type in System.Enum.GetNames(typeof(Population.BrainType))) {
				//DebugBot.DebugFunctionCall("TMiniGameUI; " + type, debugFunctionCalls);
				GameObject button = (GameObject)Instantiate (buttonBrainTypeDropDownPrefab);
				button.GetComponentInChildren<Text>().text = type;
				string enumType = "";
				enumType = type;
				button.GetComponent<Button>().onClick.AddListener (
					() => {ChooseBrainType(enumType);}
				);
				button.transform.SetParent(dropDownChooseBrainTypePanel);
			}
			dropDownPopulated = true;
		}
		
		DebugBot.DebugFunctionCall("TNewPopUI; InitializePanelWithTrainerData(); ", debugFunctionCalls);
		sliderPopulationSize.minValue = minMaxPopulationSize;
		sliderPopulationSize.maxValue = maxMaxPopulationSize;
		sliderNumInputs.minValue = minBrainInputs;
		sliderNumInputs.maxValue = maxBrainInputs;
		sliderNumOutputs.minValue = minBrainOutputs;
		sliderNumOutputs.maxValue = maxBrainOutputs;

		UpdateUIWithCurrentData();
	}

	private void UpdateUIWithCurrentData() {
		//int dataCurPlayer = trainerModuleScript.gameController.masterTrainer.CurPlayer;
		DebugBot.DebugFunctionCall("TNewPopUI; UpdateUIWithCurrentData(); " + pendingPopulationSize.ToString(), debugFunctionCalls);
		sliderPopulationSize.value = pendingPopulationSize;
		sliderNumInputs.value = pendingNumInputs;
		sliderNumOutputs.value = pendingNumOutputs;

		textPopulationSize.text = pendingPopulationSize.ToString();
		textNumInputs.text = pendingNumInputs.ToString();
		textNumOutputs.text = pendingNumOutputs.ToString();

		textLoadedBodyTemplateName.text = pendingBodyTemplateName;

		UpdateUIElementStates();
	}

	private void UpdateUIElementStates() {
		DebugBot.DebugFunctionCall("TNewPopUI; UpdateUIElementStates(); ", debugFunctionCalls);

		// Inputs / Outputs button toggles
		if(brainTypeOptionsPanelOn) {
			buttonBrainTypeOptionsPanel.interactable = false;
			buttonTransferFunctionOptionsPanel.interactable = true;
			panelBrainTypeOptionsVisible.SetActive (true);
			panelTransferFunctionsVisible.SetActive (false);
			//SetupPanelInputList();
		}
		else {
			buttonBrainTypeOptionsPanel.interactable = true;
			buttonTransferFunctionOptionsPanel.interactable = false;
			panelBrainTypeOptionsVisible.SetActive (false);
			panelTransferFunctionsVisible.SetActive (true);
			//SetupPanelOutputList();
		}		
		// Choose Brain-Type Drop-down:
		if(populationRef.brainType == Population.BrainType.None) {
			textButtonBrainTypeDropDown.text = "Choose Brain Type (Drop-Down)";
		}
		else {
			textButtonBrainTypeDropDown.text = populationRef.brainType.ToString();
		}
		bgImage.color = trainerModuleScript.defaultBGColor;
	}

	#region OnClick & UIElement changed Functions:

	//public void ClickChooseBrainType() {  // Is this needed?
	//	DebugBot.DebugFunctionCall("TNewPopUI; ClickChooseBrainType(); ", debugFunctionCalls);
	//}
	
	public void ChooseBrainType(string brainType) {  // clicked on a drop-down sub-button:
		DebugBot.DebugFunctionCall("TNewPopUI; ChooseBrainType(" + brainType + "); ", debugFunctionCalls);
		Player currentPlayer = trainerModuleScript.gameController.masterTrainer.PlayerList[trainerModuleScript.gameController.masterTrainer.CurPlayer-1];

		Population.BrainType parsed_enum = (Population.BrainType)System.Enum.Parse( typeof( Population.BrainType ), brainType );  // convert string to enum
		populationRef.ChangeTemplateBrainType(parsed_enum);  // change braintype of Population's templateBrain

		Trial dataMiniGameTrial = currentPlayer.masterTrialsList[currentPlayer.currentTrialForEdit];
		//if(pendingMiniGameManager.gameType != dataMiniGameTrial.miniGameManager.gameType) { // if the values are different:
		//	valuesChanged = true;
		//	pendingMiniGameManager.SetMiniGameType(pendingMiniGameManager.gameType); 
			// Inputs / Outputs button toggles

		//	InitializePanelInputList();  // Set up brainTypeOptions Panel
		//	InitializePanelOutputList();  // and possibly Transfer Functions panel also

		UpdateUIWithCurrentData();
	}

	public void ClickLoadBodyTemplate() {
		DebugBot.DebugFunctionCall("TNewPopUI; ClickLoadBodyTemplate(); EDITOR TURNED OFF!!!", true);
        UniFileBrowser.use.OpenFileWindow(LoadBodyTemplate);
        /*  // OLD:
		// Open file explorer window to choose asset filename:
		string absPath = EditorUtility.OpenFilePanel ("Select Creature", "Assets/Resources", "");
		if(absPath.StartsWith (Application.dataPath)) {
			pendingBodyTemplateName = absPath.Substring (Application.dataPath.Length - "Assets".Length);
		}
		pendingBodyTemplateName = pendingBodyTemplateName.Substring("Assets/Resources/".Length); // clips off folders
		pendingBodyTemplateName = pendingBodyTemplateName.Substring(0, pendingBodyTemplateName.Length - ".asset".Length); // clips extension

		DebugBot.DebugFunctionCall("TNewPopUI; ClickLoadBodyTemplate(); " + pendingBodyTemplateName.ToString(), true);
		UpdateUIWithCurrentData();
        */
    }
    public void LoadBodyTemplate(string filename) {
        Debug.Log("LoadBodyTemplate; filename: " + filename);
        char pathChar = '/';
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer) {
            pathChar = '\\';
        }
        var fileIndex = filename.LastIndexOf(pathChar);
        string templateName = filename.Substring(fileIndex + 1, filename.Length - fileIndex - 1);
        templateName = templateName.Substring(0, templateName.Length - ".txt".Length);
        Debug.Log("message: " + templateName);
        
        if (System.IO.File.Exists(filename)) {
            CritterGenome genomeToLoad = ES2.Load<CritterGenome>(filename);
            Debug.Log("genomeToLoad.Length: " + genomeToLoad.CritterNodeList.Count.ToString());
            //critterConstructorManager.masterCritter.LoadCritterGenome(genomeToLoad);
            pendingBodyTemplateName = templateName;
            UpdateUIWithCurrentData();
        }
        else {
            Debug.LogError("No CritterGenome File Exists!");
        }
    }

	public void ClickToggleOptionsPanels() {
		DebugBot.DebugFunctionCall("TNewPopUI; ClickToggleOptionsPanels(); ", debugFunctionCalls);
		brainTypeOptionsPanelOn = !brainTypeOptionsPanelOn;
		UpdateUIWithCurrentData();
	}

	public void SliderPopulationSize(float sliderValue) { // On Slider Value Changed
		DebugBot.DebugFunctionCall("TNewPopUI; SliderPopulationSize(); ", debugFunctionCalls);
		pendingPopulationSize = (int)sliderValue;
		UpdateUIWithCurrentData();
	}

	public void SliderNumInputs(float sliderValue) { // On Slider Value Changed
		DebugBot.DebugFunctionCall("TNewPopUI; SliderNumInputs(); ", debugFunctionCalls);
		pendingNumInputs = (int)sliderValue;
		UpdateUIWithCurrentData();
	}

	public void SliderNumOutputs(float sliderValue) { // On Slider Value Changed
		DebugBot.DebugFunctionCall("TNewPopUI; SliderNumOutputs(); ", debugFunctionCalls);
		pendingNumOutputs = (int)sliderValue;
		UpdateUIWithCurrentData();
	}

	public void ToggleRandomWeights(bool value) {
		DebugBot.DebugFunctionCall("TNewPopUI; ToggleRandomWeights(); ", debugFunctionCalls);
		//Debug.Log ("ToggleRandomWeights(), NewPopPanelUI, " + value.ToString());
		toggleZeroedWeights.isOn = !value;
		if(toggleRandomWeights.isOn) {
			populationRef.initRandom = true;
		}
		else {
			populationRef.initRandom = false;
		}
	}

	public void ToggleZeroedWeights(bool value) {
		DebugBot.DebugFunctionCall("TNewPopUI; ToggleZeroedWeights(); ", debugFunctionCalls);
		toggleRandomWeights.isOn = !value;
		if(toggleZeroedWeights.isOn) {
			populationRef.initRandom = false;
		}
		else {
			populationRef.initRandom = true;
		}
	}

	public void ClickCreateNewPopulation() {
		DebugBot.DebugFunctionCall("TNewPopUI; ClickCreateNewPopulation(); ", debugFunctionCalls);

		if(populationRef.brainType != Population.BrainType.None) {  // BrainType needs to BE something
			Trainer trainer = trainerModuleScript.gameController.masterTrainer;

			if(trainer.PlayerList != null) {	// if there is a valid PlayerList		
				int curPlayer = trainer.CurPlayer;
				if(trainer.PlayerList[curPlayer-1] != null) {  // If Current Player exists

					populationRef = trainer.PlayerList[curPlayer-1].masterPopulation; // grab current player's population -- this might not be needed
					populationRef.SetMaxPopulationSize(pendingPopulationSize);
                    //populationRef.numInputNodes = pendingNumInputs;
                    //populationRef.numOutputNodes = pendingNumOutputs;

                    // !!!!!!! Transfer over specific Brain-Type Options!
                    //  Or Do I simply change these values in real-time as they are selected by UI?
                    //===================================================

                    // CREATE AGENT ARRAY!!!!!!! :
                    Debug.Log("BROKEN!!! TrainerNewPopulationUI public void ClickCreateNewPopulation()");
					//CreatureBodyGenome bodyGenome = CreatureBodyLoader.LoadBodyGenome(pendingBodyTemplateName);
					//populationRef.InitializeMasterAgentArray(bodyGenome);
					//trainer.PlayerList[curPlayer-1].hasValidPopulation = true;
				}
				else { 
					DebugBot.DebugFunctionCall("TNewPopUI; ClickCreateNewPopulation(); Null Player Ref!", debugFunctionCalls);
				}
			}
			trainerModuleScript.ClickPopulation(); // switches to Population Panel -- might not be necessary?
			trainer.PlayerList[trainer.CurPlayer-1].graphKing.BuildTexturesCurAgentPerAgent(trainer.PlayerList[trainer.CurPlayer-1], 0);
			trainerModuleScript.SetAllPanelsFromTrainerData();
		}
	}

	#endregion
}
