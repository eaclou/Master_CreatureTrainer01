
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_TrainingModifier : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		TrainingModifier data = (TrainingModifier)obj;
        // Add your writer.Write calls here.
        writer.Write(0); // Version 1 is current version number
        // Make sure to edit Read() function to properly handle version control!
        // VERSION 0:
        writer.Write(data.adoptionRate);
        writer.Write(data.beginMaxAngle);
        writer.Write(data.beginMaxDistance);
        writer.Write(data.beginMaxTime);
        writer.Write(data.beginMinAngle);
        writer.Write(data.beginMinDistance);
        writer.Write(data.beginMinTime);
        writer.Write(data.decayEffectOverDuration);
        writer.Write(data.duration);
        writer.Write(data.endMaxAngle);
        writer.Write(data.endMaxDistance);
        writer.Write(data.endMaxTime);
        writer.Write(data.endMinAngle);
        writer.Write(data.endMinDistance);
        writer.Write(data.endMinTime);
        writer.Write(data.forward);
        writer.Write(data.horizontal);
        writer.Write(data.largeBrainPenalty);
        writer.Write(data.largeSpeciesPenalty);
        writer.Write(data.linksPerNode);
        writer.Write(data.liveForever);
        writer.Write(data.minMultiplier);
        writer.Write(data.modifierType);
        writer.Write(data.nodesPerLink);
        writer.Write(data.numRounds);
        writer.Write(data.removeLinkChance);
        writer.Write(data.removeNodeChance);
        writer.Write(data.speciesSimilarityThreshold);
        writer.Write(data.startGen);
        writer.Write(data.vertical);
        // VERSION 1:
    }

    public override object Read(ES2Reader reader)
	{
		TrainingModifier data = new TrainingModifier();
		Read(reader, data);
		return data;
	}
	
	public override void Read(ES2Reader reader, object c)
	{
		TrainingModifier data = (TrainingModifier)c;
        // Add your reader.Read calls here to read the data into the object.
        // Read the version number.
        int fileVersion = reader.Read<int>();

        // VERSION 0:
        if (fileVersion >= 0) {
            
            data.adoptionRate = reader.Read<float>();
            data.beginMaxAngle = reader.Read<float>();
            data.beginMaxDistance = reader.Read<float>();
            data.beginMaxTime = reader.Read<float>();
            data.beginMinAngle = reader.Read<float>();
            data.beginMinDistance = reader.Read<float>();
            data.beginMinTime = reader.Read<float>();
            data.decayEffectOverDuration = reader.Read<bool>();
            data.duration = reader.Read<int>();
            data.endMaxAngle = reader.Read<float>();
            data.endMaxDistance = reader.Read<float>();
            data.endMaxTime = reader.Read<float>();
            data.endMinAngle = reader.Read<float>();
            data.endMinDistance = reader.Read<float>();
            data.endMinTime = reader.Read<float>();
            data.forward = reader.Read<bool>();
            data.horizontal = reader.Read<bool>();
            data.largeBrainPenalty = reader.Read<float>();
            data.largeSpeciesPenalty = reader.Read<float>();
            data.linksPerNode = reader.Read<float>();
            data.liveForever = reader.Read<bool>();
            data.minMultiplier = reader.Read<float>();
            data.modifierType = reader.Read<TrainingModifier.TrainingModifierType>();
            data.nodesPerLink = reader.Read<float>();
            data.numRounds = reader.Read<int>();
            data.removeLinkChance = reader.Read<float>();
            data.removeNodeChance = reader.Read<float>();
            data.speciesSimilarityThreshold = reader.Read<float>();
            data.startGen = reader.Read<int>();
            data.vertical = reader.Read<bool>();

            if (fileVersion >= 1) {
                // new attributes
            }
        }
    }
	
	/* ! Don't modify anything below this line ! */
	public ES2UserType_TrainingModifier():base(typeof(TrainingModifier)){}
}
