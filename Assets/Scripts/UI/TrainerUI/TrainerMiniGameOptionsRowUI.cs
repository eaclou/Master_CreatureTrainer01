using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TrainerMiniGameOptionsRowUI : MonoBehaviour {

	public bool debugFunctionCalls = false;

	public int optionsListIndex;
	
	public TrainerModuleUI trainerModuleScript;
	public TrainerMiniGameUI trainerMiniGameScript;
	//public Toggle toggleOutputSource;
	public Text textOptionName;
	public Slider sliderOptionChannel;
	public Text textOptionValue;
	//public Button buttonMapKeys;
	//public Image bgImage;

	//public int pendingNumNodes;
	//public bool pendingChannelOn;
	public float pendingOptionValue;

	public bool pendingUpdateFromData = false;


	// UI Settings:
	//private int minNumNodes = 1;
	//private int maxNumNodes = 4;
	
	public void InitializePanelWithTrainerData() {
		//DebugBot.DebugFunctionCall("TMiniGameOptionsRowUI; InitializePanelWithTrainerData(); ", debugFunctionCalls);

		Player currentPlayer = trainerModuleScript.gameController.masterTrainer.PlayerList[trainerModuleScript.gameController.masterTrainer.CurPlayer-1];

		sliderOptionChannel.value = trainerMiniGameScript.pendingMiniGameSettings.gameOptionsList[optionsListIndex].channelValue[0];
		pendingOptionValue = trainerMiniGameScript.pendingMiniGameSettings.gameOptionsList[optionsListIndex].channelValue[0];
		pendingUpdateFromData = true;

		DebugBot.DebugFunctionCall("TMiniGameOptionsRowUI; InitializePanelWithTrainerData(); pend: " + pendingOptionValue.ToString() + ", val: " + sliderOptionChannel.value.ToString() + ", channel: " + trainerMiniGameScript.pendingMiniGameSettings.gameOptionsList[optionsListIndex].channelValue[0].ToString(), debugFunctionCalls);
		UpdateUIWithCurrentData();
	}
	
	public void CheckActivationCriteria() {  // checks which buttons/elements should be active/inactive based on the current data
		DebugBot.DebugFunctionCall("TMiniGameOptionsRowUI; CheckActivationCriteria(); ", debugFunctionCalls);
				
	}
	
	public void UpdateUIElementStates() {
		// Changing Button Displays !!
		Player currentPlayer = trainerModuleScript.gameController.masterTrainer.PlayerList[trainerModuleScript.gameController.masterTrainer.CurPlayer-1];

		sliderOptionChannel.minValue = trainerMiniGameScript.pendingMiniGameSettings.gameOptionsList[optionsListIndex].minValue;
		sliderOptionChannel.maxValue = trainerMiniGameScript.pendingMiniGameSettings.gameOptionsList[optionsListIndex].maxValue;
		sliderOptionChannel.value = pendingOptionValue;
		textOptionName.text = trainerMiniGameScript.pendingMiniGameSettings.gameOptionsList[optionsListIndex].channelName; // set display name
		textOptionValue.text = trainerMiniGameScript.pendingMiniGameSettings.gameOptionsList[optionsListIndex].channelValue[0].ToString(); // set value display

		//DebugBot.DebugFunctionCall("TMiniGameOptionsRowUI; UpdateUIElementStates(); " + textOutputSourceName.text.ToString(), debugFunctionCalls);
	}
	
	public void UpdateUIWithCurrentData() {
		//DebugBot.DebugFunctionCall("TMiniGameOptionsRowUI; UpdateUIWithCurrentData(); pend: " + pendingOptionValue.ToString() + ", val: " + sliderOptionChannel.value.ToString(), true);

		//sliderOptionChannel.value = pendingOptionValue;

		DebugBot.DebugFunctionCall("TMiniGameOptionsRowUI; UpdateUIWithCurrentData(); pend: " + pendingOptionValue.ToString() + ", val: " + sliderOptionChannel.value.ToString(), debugFunctionCalls);
		CheckActivationCriteria();
		UpdateUIElementStates();
	}

	#region OnClick & UIElement changed Functions:

	public void SliderValue(float val) { // On Slider Value Changed
		DebugBot.DebugFunctionCall("TMiniGameOptionsRowUI; SliderValue() Before; " + pendingOptionValue.ToString() + ", val: " + val.ToString(), debugFunctionCalls);
		if(pendingUpdateFromData) {

		}
		pendingOptionValue = val;
		trainerMiniGameScript.pendingMiniGameSettings.gameOptionsList[optionsListIndex].channelValue[0] = pendingOptionValue;
		trainerMiniGameScript.valuesChanged = true;
		trainerMiniGameScript.UpdateUIWithCurrentData();
		UpdateUIWithCurrentData();			

		DebugBot.DebugFunctionCall("TMiniGameOptionsRowUI; SliderValue() After; " + pendingOptionValue.ToString() + ", val: " + val.ToString(), debugFunctionCalls);
	}

	#endregion
}
