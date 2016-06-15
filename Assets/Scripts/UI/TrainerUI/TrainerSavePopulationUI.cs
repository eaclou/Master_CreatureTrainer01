using UnityEngine;
using System.IO;
using System.Collections;
using UnityEngine.UI;

public class TrainerSavePopulationUI : MonoBehaviour {

	private bool debugFunctionCalls = true;

	public TrainerModuleUI trainerModuleScript;
	public Button buttonSaveTopAgent;
	public Button buttonSaveAgentNumberN;
	public Slider sliderSaveAgentNumberN;
	public Text textSaveAgentNumberN;
	public Button buttonSaveCurrentPopulation;
	public InputField inputFieldFileSaveName;
	public Toggle toggleOverwriteSaves;
	public Image bgImage;

	private int pendingAgentNumberN = 1;
	private Population populationRef;

	// CHANGE THESE INSIDE LoadPopulationScript ALSO!!!!!
	//private string agentRootPath = "E:/Unity Projects/GitHub/ANNTrainer/CreatureTrainer/Assets/SaveFiles/Agents/";
	private string saveRootPath = "E:/Unity Projects/GitHub/ANNTrainer/CreatureTrainer/Assets/SaveFiles/TrainingSaves/Populations/";
	private string fileExt = ".txt";

	#region Main UI init & refresh functions:
	public void InitializePanelWithTrainerData() {
		
		Player currentPlayer = trainerModuleScript.gameController.masterTrainer.PlayerList[trainerModuleScript.gameController.masterTrainer.CurPlayer-1];
		populationRef = currentPlayer.masterPopulation;
		if(populationRef != null) {
			DebugBot.DebugFunctionCall("SavePopulationUI; InitializePanelWithTrainerData(); " + populationRef.populationMaxSize.ToString() + ", ", debugFunctionCalls);// + populationRef.masterAgentArray.Length.ToString(), debugFunctionCalls);
			pendingAgentNumberN = populationRef.populationMaxSize;
		}
		else {
			DebugBot.DebugFunctionCall("SavePopulationUI; InitializePanelWithTrainerData(); ", debugFunctionCalls);
		}



		UpdateUIWithCurrentData();
	}
	
	private void UpdateUIWithCurrentData() {
		//int dataCurPlayer = trainerModuleScript.gameController.masterTrainer.CurPlayer;
		DebugBot.DebugFunctionCall("SavePopulationUI; UpdateUIWithCurrentData(); ", debugFunctionCalls);

		if(populationRef != null) {
			sliderSaveAgentNumberN.minValue = 1;
			sliderSaveAgentNumberN.maxValue = populationRef.populationMaxSize;
			sliderSaveAgentNumberN.value = pendingAgentNumberN;

			textSaveAgentNumberN.text = pendingAgentNumberN.ToString ();
		}
		
		UpdateUIElementStates();
	}
	
	private void UpdateUIElementStates() {


		if(populationRef != null) {
			DebugBot.DebugFunctionCall("SavePopulationUI; UpdateUIElementStates(); ", debugFunctionCalls);
			buttonSaveTopAgent.interactable = true;
			buttonSaveAgentNumberN.interactable = true;
			sliderSaveAgentNumberN.interactable = true;
			buttonSaveCurrentPopulation.interactable = true;
		}
		else {
			DebugBot.DebugFunctionCall("SavePopulationUI; UpdateUIElementStates(); populationRefNULL", debugFunctionCalls);
			buttonSaveTopAgent.interactable = false;
			buttonSaveAgentNumberN.interactable = false;
			sliderSaveAgentNumberN.interactable = false;
			buttonSaveCurrentPopulation.interactable = false;
		}

		bgImage.color = trainerModuleScript.defaultBGColor;
	}
	#endregion
	
	#region OnClick & UIElement changed Functions:

	/* Save the value 123 to a file named myFile.txt */
	//ES2.Save(123,  "myFile.txt");	
	/* Now load that int back from the file */
	//int myInt = ES2.Load<int>("myFile.txt");
	/* Save myFile.txt inside myFolder */
	//ES2.Save(123,  "myFolder/myFile.txt");	
	/* And now load it back */
	//int myInt = ES2.Load<int>("myFolder/myFile.txt");

	/* Save our int to an absolute file on Windows */
	//ES2.Save(123, "C:/Users/User/myFile.txt");
	/* Load from an absolute file on OSX */
	//int myInt = ES2.Load<int>("/Users/User/myFile.txt");

	public void ClickSaveCurrentPopulation() {
		DebugBot.DebugFunctionCall("SavePopulationUI; ClickSaveCurrentPopulation(); ", debugFunctionCalls);
		Player currentPlayer = trainerModuleScript.gameController.masterTrainer.PlayerList[trainerModuleScript.gameController.masterTrainer.CurPlayer-1];
		populationRef = currentPlayer.masterPopulation;
        saveRootPath = Application.dataPath + "/SaveFiles/TrainingSaves/";

        string fileName = inputFieldFileSaveName.text + fileExt;
		Debug.Log( saveRootPath + fileName);
		
		if(populationRef != null) { 
			Population populationToSave = populationRef;  // Current player's population
			
			bool save = true;
			if(System.IO.File.Exists (saveRootPath + fileName) && !toggleOverwriteSaves.isOn) {  
				Debug.Log ("File Already Exists!");
				save = false;
			}
			if(fileName == "") {
				Debug.Log ("No Filename Specified!");
				save = false;
			}            
			
			if(save) {   // SAVE:
                Debug.Log("SAVE TRAININGSAVE!!! filename: " + saveRootPath + fileName + ", pop size: " + populationToSave.masterAgentArray.Length.ToString());

                // Create wrapper to hold all save info:
                TrainingSave trainingSave = new TrainingSave();
                populationToSave.nextAvailableGeneInno = GenomeNEAT.nextAvailableInnovationNumber;
                trainingSave.savedPopulation = populationToSave;
                trainingSave.beginGeneration = populationToSave.trainingGenerations;
                trainingSave.endGeneration = trainingSave.beginGeneration + trainerModuleScript.gameController.masterTrainer.PlayingCurGeneration - 1; // Check if -1 is needed
                trainingSave.savedPopulation.trainingGenerations = trainingSave.endGeneration;  // update it so that when it is loaded it has the proper start gen#

                trainingSave.savedCrossoverManager = currentPlayer.masterCupid;
                trainingSave.savedFitnessComponentList = currentPlayer.masterTrialsList[0].fitnessManager.gameFitnessComponentList;
                trainingSave.savedMiniGameSettings = new MiniGameSettingsSaves();
                // Copy the settings from the minigame instance into the SaveWrapper:
                currentPlayer.masterTrialsList[0].miniGameManager.miniGameInstance.gameSettings.CopySettingsToSave(trainingSave.savedMiniGameSettings);

                trainingSave.savedTrialDataBegin = currentPlayer.dataManager.generationDataList[0].trialDataArray[0];
                trainingSave.savedTrialDataEnd = currentPlayer.dataManager.generationDataList[trainerModuleScript.gameController.masterTrainer.PlayingCurGeneration - 1].trialDataArray[0];
               
                ES2.Save(trainingSave, saveRootPath + fileName);
			}
		}
		else {
			Debug.LogError("No Population Exists!");
		}
	}
	
	#endregion
}
