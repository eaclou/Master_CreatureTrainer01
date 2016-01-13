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
	private string populationRootPath = "E:/Unity Projects/GitHub/ANNTrainer/CreatureTrainer/Assets/TrainingSaves/Populations/";
	
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
			//sliderSaveAgentNumberN.minValue = 1;
			//sliderSaveAgentNumberN.maxValue = populationRef.populationMaxSize;
			//sliderSaveAgentNumberN.value = pendingAgentNumberN;			
			//textSaveAgentNumberN.text = pendingAgentNumberN.ToString ();
		}
		
		UpdateUIElementStates();
	}
	
	private void UpdateUIElementStates() {
		DebugBot.DebugFunctionCall("LoadPopulationUI; UpdateUIElementStates(); ", debugFunctionCalls);
		
		if(populationRef != null) {
			//buttonSaveTopAgent.interactable = true;
		}
		else {
			//buttonSaveTopAgent.interactable = false;
		}
		
		bgImage.color = trainerModuleScript.defaultBGColor;
	}
	#endregion

	#region OnClick & UIElement changed Functions:
	
	public void ClickLoadPopulation() {
		DebugBot.DebugFunctionCall("SavePopulationUI; ClickSaveCurrentPopulation(); ", debugFunctionCalls);
		Player currentPlayer = trainerModuleScript.gameController.masterTrainer.PlayerList[trainerModuleScript.gameController.masterTrainer.CurPlayer-1];
		//populationRef = currentPlayer.masterPopulation;
		
		string fileName = inputFieldFileName.text;
		Debug.Log( populationRootPath + fileName);
		
		if(System.IO.File.Exists (populationRootPath + fileName)) { 
			Population populationToLoad = ES2.Load<Population>(populationRootPath + fileName);;  

			Debug.Log ("genomeBiases.Length: " + populationToLoad.masterAgentArray[0].genome.genomeBiases.Length.ToString());
			Debug.Log ("genomeBias[0]: " + populationToLoad.masterAgentArray[0].genome.genomeBiases[0].ToString());
			Debug.Log ("root_body_size: " + populationToLoad.masterAgentArray[0].bodyGenome.creatureBodySegmentGenomeList[0].size.ToString());

			// Leap of Faith:
			currentPlayer.masterPopulation = populationToLoad;
			currentPlayer.masterPopulation.InitializeLoadedMasterAgentArray(); // <-- somewhat hacky, re-assess later, but this is where the brains are created from genome
			currentPlayer.masterPopulation.isFunctional = true;
			currentPlayer.hasValidPopulation = true;
			trainerModuleScript.SetAllPanelsFromTrainerData();
		}
		else {
			Debug.LogError("No Population File Exists!");
		}
	}
	
	/*public void ClickCreateNewPopulation() {
		DebugBot.DebugFunctionCall("SavePopulationUI; ClickCreateNewPopulation(); ", debugFunctionCalls);
		
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
					CreatureBodyGenome bodyGenome = CreatureBodyLoader.LoadBodyGenome(pendingBodyTemplateName);
					populationRef.InitializeMasterAgentArray(bodyGenome);
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
	}*/
	
	#endregion
}
