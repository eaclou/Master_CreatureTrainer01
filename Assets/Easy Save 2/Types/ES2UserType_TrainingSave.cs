
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_TrainingSave : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		TrainingSave data = (TrainingSave)obj;
        // Add your writer.Write calls here.
        writer.Write(0); // Version 1 is current version number
        // Make sure to edit Read() function to properly handle version control!
        // VERSION 0:
        writer.Write(data.beginGeneration);
        writer.Write(data.endGeneration);
        writer.Write(data.savedCrossoverManager);
        writer.Write(data.savedFitnessComponentList);
        writer.Write(data.savedMiniGameSettings);
        writer.Write(data.savedPopulation);
        writer.Write(data.savedTrialDataBegin);
        writer.Write(data.savedTrialDataEnd);
        writer.Write(data.savedTrainingModifierList);
        // VERSION 1:

    }

    public override object Read(ES2Reader reader)
	{
		TrainingSave data = new TrainingSave();
		Read(reader, data);
		return data;
	}
	
	public override void Read(ES2Reader reader, object c)
	{
		TrainingSave data = (TrainingSave)c;
        // Add your reader.Read calls here to read the data into the object.
        // Read the version number.
        int fileVersion = reader.Read<int>();

        // VERSION 0:
        if (fileVersion >= 0) {
            data.beginGeneration = reader.Read<int>();
            data.endGeneration = reader.Read<int>();
            data.savedCrossoverManager = reader.Read<CrossoverManager>();
            data.savedFitnessComponentList = reader.ReadList<FitnessComponent>();
            data.savedMiniGameSettings = reader.Read<MiniGameSettingsSaves>();
            data.savedPopulation = reader.Read<Population>();
            data.savedTrialDataBegin = reader.Read<TrialData>();
            data.savedTrialDataEnd = reader.Read<TrialData>();
            data.savedTrainingModifierList = reader.ReadList<TrainingModifier>();
            if (fileVersion >= 1) {
                // new attributes
            }
        }
    }
	
	/* ! Don't modify anything below this line ! */
	public ES2UserType_TrainingSave():base(typeof(TrainingSave)){}
}
