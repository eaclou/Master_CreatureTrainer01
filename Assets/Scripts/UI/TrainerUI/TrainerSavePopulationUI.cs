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
	private string agentRootPath = "E:/Unity Projects/GitHub/ANNTrainer/CreatureTrainer/Assets/TrainingSaves/Agents/";
	private string populationRootPath = "E:/Unity Projects/GitHub/ANNTrainer/CreatureTrainer/Assets/TrainingSaves/Populations/";
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
	
	/*public void ClickLoadBodyTemplate() {
		DebugBot.DebugFunctionCall("SavePopulationUI; ClickLoadBodyTemplate(); ", true);
		
		// Open file explorer window to choose asset filename:
		string absPath = EditorUtility.OpenFilePanel ("Select Creature", "Assets/Resources", "");
		if(absPath.StartsWith (Application.dataPath)) {
			pendingBodyTemplateName = absPath.Substring (Application.dataPath.Length - "Assets".Length);
		}
		pendingBodyTemplateName = pendingBodyTemplateName.Substring("Assets/Resources/".Length); // clips off folders
		pendingBodyTemplateName = pendingBodyTemplateName.Substring(0, pendingBodyTemplateName.Length - ".asset".Length); // clips extension
		
		DebugBot.DebugFunctionCall("SavePopulationUI; ClickLoadBodyTemplate(); " + pendingBodyTemplateName.ToString(), true);
		UpdateUIWithCurrentData();
	}*/
	
	public void SliderAgentNumberN(float sliderValue) { // On Slider Value Changed
		DebugBot.DebugFunctionCall("SavePopulationUI; SliderAgentNumberN(); ", debugFunctionCalls);
		pendingAgentNumberN = (int)sliderValue;
		UpdateUIWithCurrentData();
	}

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

	public void ClickSaveTopAgent() {
		DebugBot.DebugFunctionCall("SavePopulationUI; ClickSaveTopAgent(); ", debugFunctionCalls);
		Player currentPlayer = trainerModuleScript.gameController.masterTrainer.PlayerList[trainerModuleScript.gameController.masterTrainer.CurPlayer-1];
		populationRef = currentPlayer.masterPopulation;

		string fileName = inputFieldFileSaveName.text + fileExt;
		Debug.Log( agentRootPath + fileName);

		if(populationRef.masterAgentArray[0] != null) { // top agent occupies index 0
			Agent agentToSave = populationRef.masterAgentArray[0];

			bool save = true;
			if(System.IO.File.Exists (agentRootPath + fileName) && !toggleOverwriteSaves.isOn) {  
				Debug.Log ("File Already Exists!");
				save = false;
			}
			if(fileName == "") {
				Debug.Log ("No Filename Specified!");
				save = false;
			}

			if(save) {   // SAVE AGENT:
				ES2.Save(agentToSave, agentRootPath + fileName);
				Agent agentToLoad = ES2.Load<Agent>(agentRootPath + fileName);
				Debug.Log ("genomeBiases.Length: " + agentToLoad.genome.genomeBiases.Length);
				Debug.Log ("genomeBias[0]: " + agentToLoad.genome.genomeBiases[0].ToString());
				Debug.Log ("ref genomeBias[0]: " + populationRef.masterAgentArray[0].genome.genomeBiases[0].ToString());
			}
		}
		else {
			Debug.LogError("No Agent Exists!");
		}
	}

	public void ClickSaveAgentNumberN() {
		DebugBot.DebugFunctionCall("SavePopulationUI; ClickSaveAgentNumberN(); ", debugFunctionCalls);
		Player currentPlayer = trainerModuleScript.gameController.masterTrainer.PlayerList[trainerModuleScript.gameController.masterTrainer.CurPlayer-1];
		populationRef = currentPlayer.masterPopulation;
	}

	public void ClickSaveCurrentPopulation() {
		DebugBot.DebugFunctionCall("SavePopulationUI; ClickSaveCurrentPopulation(); ", debugFunctionCalls);
		Player currentPlayer = trainerModuleScript.gameController.masterTrainer.PlayerList[trainerModuleScript.gameController.masterTrainer.CurPlayer-1];
		populationRef = currentPlayer.masterPopulation;

		string fileName = inputFieldFileSaveName.text + fileExt;
		Debug.Log( populationRootPath + fileName);
		
		if(populationRef != null) { 
			Population populationToSave = populationRef;  // Current player's population
			
			bool save = true;
			if(System.IO.File.Exists (populationRootPath + fileName) && !toggleOverwriteSaves.isOn) {  
				Debug.Log ("File Already Exists!");
				save = false;
			}
			if(fileName == "") {
				Debug.Log ("No Filename Specified!");
				save = false;
			}
			
			if(save) {   // SAVE AGENT:
				ES2.Save(populationToSave, populationRootPath + fileName);
				Population populationToLoad = ES2.Load<Population>(populationRootPath + fileName);
				Debug.Log ("genomeBiases.Length: " + populationToLoad.masterAgentArray[0].genome.genomeBiases.Length.ToString());
				Debug.Log ("genomeBias[0]: " + populationToLoad.masterAgentArray[0].genome.genomeBiases[0].ToString());
				Debug.Log ("ref genomeBias[0]: " + populationRef.masterAgentArray[0].genome.genomeBiases[0].ToString());
				Debug.Log ("root_body_size: " + populationToLoad.masterAgentArray[0].bodyGenome.creatureBodySegmentGenomeList[0].size.ToString());
			}
		}
		else {
			Debug.LogError("No Population Exists!");
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
