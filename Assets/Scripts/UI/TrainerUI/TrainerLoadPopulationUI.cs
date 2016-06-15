using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TrainerLoadPopulationUI : MonoBehaviour {

	private bool debugFunctionCalls = false;

	public TrainerModuleUI trainerModuleScript;
	public InputField inputFieldFileName;
	public Button buttonLoadPopulation;
	public Image bgImage;

	private Population populationRef;
	private string fileRootPath = "";
    private string fileExt = ".txt";

    #region Main UI init & refresh functions:
    public void InitializePanelWithTrainerData() {
		
		Player currentPlayer = trainerModuleScript.gameController.masterTrainer.PlayerList[trainerModuleScript.gameController.masterTrainer.CurPlayer-1];
		populationRef = currentPlayer.masterPopulation;
		
		DebugBot.DebugFunctionCall("LoadPopulationUI; InitializePanelWithTrainerData(); ", debugFunctionCalls);
		
		UpdateUIWithCurrentData();
	}
	
	private void UpdateUIWithCurrentData() {
		//int dataCurPlayer = trainerModuleScript.gameController.masterTrainer.CurPlayer;
		DebugBot.DebugFunctionCall("LoadPopulationUI; UpdateUIWithCurrentData(); ", debugFunctionCalls);
		
		if(populationRef != null) {
			
		}
		
		UpdateUIElementStates();
	}
	
	private void UpdateUIElementStates() {
		DebugBot.DebugFunctionCall("LoadPopulationUI; UpdateUIElementStates(); ", debugFunctionCalls);
		
		if(populationRef != null) {
		}
		else {
		}
		
		bgImage.color = trainerModuleScript.defaultBGColor;
	}
	#endregion

	#region OnClick & UIElement changed Functions:
	
	public void ClickLoadPopulation() {
		DebugBot.DebugFunctionCall("LoadPopulationUI; ClickLoadPopulation(); ", debugFunctionCalls);
		Player currentPlayer = trainerModuleScript.gameController.masterTrainer.PlayerList[trainerModuleScript.gameController.masterTrainer.CurPlayer-1];
        fileRootPath = Application.dataPath + "/SaveFiles/TrainingSaves/";
        string fileName = inputFieldFileName.text + fileExt;
        Debug.Log(fileRootPath + fileName);
        		
		if(System.IO.File.Exists (fileRootPath + fileName)) { 
			TrainingSave trainingDataToLoad = ES2.Load<TrainingSave>(fileRootPath + fileName);;
            trainerModuleScript.gameController.masterTrainer.loadedTrainingSave = trainingDataToLoad;
            // Leap of Faith:
            currentPlayer.masterPopulation = trainingDataToLoad.savedPopulation;
            GenomeNEAT.nextAvailableInnovationNumber = trainingDataToLoad.savedPopulation.nextAvailableGeneInno;
            currentPlayer.masterPopulation.trainingGenerations = trainingDataToLoad.endGeneration;  // keep track of total gens this population has trained on
            currentPlayer.masterPopulation.InitializeLoadedMasterAgentArray(); // <-- somewhat hacky, re-assess later, but this is where the brains are created from genome
			currentPlayer.masterPopulation.isFunctional = true;
			currentPlayer.hasValidPopulation = true;
            Debug.Log("Loaded Training Save!!! body nodes: " + currentPlayer.masterPopulation.templateGenome.ToString() + ", startGen: " + trainingDataToLoad.beginGeneration.ToString() + ", endGen: " + trainingDataToLoad.endGeneration.ToString());

            currentPlayer.masterCupid = trainingDataToLoad.savedCrossoverManager;

			trainerModuleScript.SetAllPanelsFromTrainerData();
		}
		else {
			Debug.LogError("No TrainingData File Exists!");
		}
	}
	
	
	#endregion
}
