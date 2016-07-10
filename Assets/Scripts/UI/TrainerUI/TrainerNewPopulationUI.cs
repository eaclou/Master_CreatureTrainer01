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

    public Slider sliderNumHiddenNodes;
    public Text textNumHiddenNodes;
    public Slider sliderConnectedness;
    public Text textConnectedness;

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

	private int pendingPopulationSize = 50;
	private int pendingNumInputs = 4;
	private int pendingNumOutputs = 4;

    private int pendingNumHiddenNodes = 0;
    private float pendingConnectedness = 0f;

	private string pendingBodyTemplateFilename = "";

	private Population populationRef;


	public void InitializePanelWithTrainerData() {

		Player currentPlayer = trainerModuleScript.gameController.masterTrainer.PlayerList[trainerModuleScript.gameController.masterTrainer.CurPlayer-1];
		populationRef = currentPlayer.masterPopulation;
				
		DebugBot.DebugFunctionCall("TNewPopUI; InitializePanelWithTrainerData(); ", debugFunctionCalls);
		sliderPopulationSize.minValue = minMaxPopulationSize;
		sliderPopulationSize.maxValue = maxMaxPopulationSize;

		UpdateUIWithCurrentData();
	}

	private void UpdateUIWithCurrentData() {		
		DebugBot.DebugFunctionCall("TNewPopUI; UpdateUIWithCurrentData(); " + pendingPopulationSize.ToString(), debugFunctionCalls);
		sliderPopulationSize.value = pendingPopulationSize;
		sliderNumInputs.value = pendingNumInputs;
		sliderNumOutputs.value = pendingNumOutputs;

		textPopulationSize.text = pendingPopulationSize.ToString();
		textNumInputs.text = pendingNumInputs.ToString();
		textNumOutputs.text = pendingNumOutputs.ToString();

        sliderNumHiddenNodes.value = pendingNumHiddenNodes;
        textNumHiddenNodes.text = pendingNumHiddenNodes.ToString();
        sliderConnectedness.value = pendingConnectedness;
        textConnectedness.text = pendingConnectedness.ToString();

		textLoadedBodyTemplateName.text = pendingBodyTemplateFilename;

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
		}
		else {
			buttonBrainTypeOptionsPanel.interactable = true;
			buttonTransferFunctionOptionsPanel.interactable = false;
			panelBrainTypeOptionsVisible.SetActive (false);
			panelTransferFunctionsVisible.SetActive (true);
		}
		bgImage.color = trainerModuleScript.defaultBGColor;
	}

	#region OnClick & UIElement changed Functions:
    
	public void ClickLoadBodyTemplate() {
		DebugBot.DebugFunctionCall("TNewPopUI; ClickLoadBodyTemplate(); EDITOR TURNED OFF!!!", true);
        UniFileBrowser.use.OpenFileWindow(LoadBodyTemplate);        
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
            //CritterGenome genomeToLoad = ES2.Load<CritterGenome>(filename);
            Debug.Log("filename.Length: " + filename.ToString());
            
            pendingBodyTemplateFilename = filename;
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

    public void SliderNumHiddenNodes(float sliderValue) { // On Slider Value Changed
        DebugBot.DebugFunctionCall("TNewPopUI; SliderNumHiddenNodes(); ", debugFunctionCalls);
        pendingNumHiddenNodes = (int)sliderValue;
        UpdateUIWithCurrentData();
    }

    public void SliderConnectedness(float sliderValue) { // On Slider Value Changed
        DebugBot.DebugFunctionCall("TNewPopUI; SliderConnectedness(); ", debugFunctionCalls);
        pendingConnectedness = sliderValue;
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
        
		Trainer trainer = trainerModuleScript.gameController.masterTrainer;  //  ?? why only set up reference here???

		if(trainer.PlayerList != null) {	// if there is a valid PlayerList		
			int curPlayer = trainer.CurPlayer;
			if(trainer.PlayerList[curPlayer-1] != null) {  // If Current Player exists

				populationRef = trainer.PlayerList[curPlayer-1].masterPopulation; // grab current player's population -- this might not be needed
				populationRef.SetMaxPopulationSize(pendingPopulationSize);
                if (toggleZeroedWeights.isOn) {
                    populationRef.initRandom = false;
                }
                else {
                    populationRef.initRandom = true;
                }
                populationRef.initNumHiddenNodes = pendingNumHiddenNodes;
                populationRef.initConnectedness = pendingConnectedness;

                // CREATE AGENT ARRAY!!!!!!! :                
                CritterGenome genomeToLoad = ES2.Load<CritterGenome>(pendingBodyTemplateFilename);
                CrossoverManager.nextNodeInnov = genomeToLoad.savedNextNodeInno;
                CrossoverManager.nextAddonInnov = genomeToLoad.savedNextAddonInno;

                populationRef.InitializeMasterAgentArray(genomeToLoad, trainer.PlayerList[curPlayer - 1].masterCupid.useSpeciation);
                trainer.PlayerList[curPlayer-1].hasValidPopulation = true;
            }
			else { 
				DebugBot.DebugFunctionCall("TNewPopUI; ClickCreateNewPopulation(); Null Player Ref!", debugFunctionCalls);
			}
		}
		trainerModuleScript.ClickPopulation(); // switches to Population Panel -- might not be necessary?
		trainer.PlayerList[trainer.CurPlayer-1].graphKing.BuildTexturesCurAgentPerAgent(trainer.PlayerList[trainer.CurPlayer-1], 0);
		trainerModuleScript.SetAllPanelsFromTrainerData();
		
	}

	#endregion
}
