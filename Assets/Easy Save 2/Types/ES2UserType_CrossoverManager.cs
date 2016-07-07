
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_CrossoverManager : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		CrossoverManager data = (CrossoverManager)obj;
        // Add your writer.Write calls here.
        writer.Write(0); // Version 1 is current version number
        // Make sure to edit Read() function to properly handle version control!
        // VERSION 0:
        writer.Write(data.adoptionRate);
        writer.Write(data.breedingByRaffle);
        writer.Write(data.breedingByRank);
        writer.Write(data.breedingRate);
        writer.Write(data.breedingStochastic);
        writer.Write(data.crossoverRandomLinkChance);
        writer.Write(data.disjointLinkWeight);
        writer.Write(data.excessLinkWeight);
        writer.Write(data.existingFromNodeBias);
        writer.Write(data.existingNetworkBias);
        writer.Write(data.interspeciesBreedingRate);
        writer.Write(data.largeBrainPenalty);
        writer.Write(data.largeSpeciesPenalty);
        writer.Write(data.linkWeightWeight);
        writer.Write(data.masterMutationRate);
        //writer.Write(data.maxBodyMutationFactor);
        writer.Write(data.maximumWeightMagnitude);
        writer.Write(data.mutationActivationFunctionChance);
        writer.Write(data.mutationAddLinkChance);
        writer.Write(data.mutationAddNodeChance);
        //writer.Write(data.mutationBodyChance);
        writer.Write(data.mutationDriftScale);
        writer.Write(data.mutationRemoveLinkChance);
        writer.Write(data.mutationRemoveNodeChance);
        writer.Write(data.newLinkBonusDuration);
        writer.Write(data.newLinkMutateBonus);
        writer.Write(data.normalizeDisjoint);
        writer.Write(data.normalizeExcess);
        writer.Write(data.normalizeLinkWeight);
        writer.Write(data.speciesSimilarityThreshold);
        //Body:
        writer.Write(data.maxAttributeValueChange);
        writer.Write(data.newSegmentChance);
        writer.Write(data.removeSegmentChance);
        writer.Write(data.segmentProportionChance);
        writer.Write(data.segmentAttachSettingsChance);
        writer.Write(data.jointSettingsChance);
        writer.Write(data.newAddonChance);
        writer.Write(data.removeAddonChance);
        writer.Write(data.addonSettingsChance);
        writer.Write(data.recursionChance);
        writer.Write(data.symmetryChance);

        writer.Write(data.survivalByRaffle);
        writer.Write(data.survivalByRank);
        writer.Write(data.survivalRate);
        writer.Write(data.survivalStochastic);
        writer.Write(data.useCrossover);
        writer.Write(data.useMutation);
        writer.Write(data.useSpeciation);
    }
	
	public override object Read(ES2Reader reader)
	{
		CrossoverManager data = new CrossoverManager();
		Read(reader, data);
		return data;
	}
	
	public override void Read(ES2Reader reader, object c)
	{
		CrossoverManager data = (CrossoverManager)c;
        // Add your reader.Read calls here to read the data into the object.
        // Read the version number.
        int fileVersion = reader.Read<int>();

        // VERSION 0:
        if (fileVersion >= 0) {
            data.adoptionRate = reader.Read<float>();
            data.breedingByRaffle = reader.Read<bool>();
            data.breedingByRank = reader.Read<bool>();
            data.breedingRate = reader.Read<float>();
            data.breedingStochastic = reader.Read<bool>();
            data.crossoverRandomLinkChance = reader.Read<float>();
            data.disjointLinkWeight = reader.Read<float>();
            data.excessLinkWeight = reader.Read<float>();
            data.existingFromNodeBias = reader.Read<float>();
            data.existingNetworkBias = reader.Read<float>();
            data.interspeciesBreedingRate = reader.Read<float>();
            data.largeBrainPenalty = reader.Read<float>();
            data.largeSpeciesPenalty = reader.Read<float>();
            data.linkWeightWeight = reader.Read<float>();
            data.masterMutationRate = reader.Read<float>();
            //data.maxBodyMutationFactor = reader.Read<float>();
            data.maximumWeightMagnitude = reader.Read<float>();
            data.mutationActivationFunctionChance = reader.Read<float>();
            data.mutationAddLinkChance = reader.Read<float>();
            data.mutationAddNodeChance = reader.Read<float>();
            //data.mutationBodyChance = reader.Read<float>();
            data.mutationDriftScale = reader.Read<float>();
            data.mutationRemoveLinkChance = reader.Read<float>();
            data.mutationRemoveNodeChance = reader.Read<float>();
            data.newLinkBonusDuration = reader.Read<int>();
            data.newLinkMutateBonus = reader.Read<float>();
            data.normalizeDisjoint = reader.Read<float>();
            data.normalizeExcess = reader.Read<float>();
            data.normalizeLinkWeight = reader.Read<float>();
            data.speciesSimilarityThreshold = reader.Read<float>();
            // body:
            data.maxAttributeValueChange = reader.Read<float>();
            data.newSegmentChance = reader.Read<float>();
            data.removeSegmentChance = reader.Read<float>();
            data.segmentProportionChance = reader.Read<float>();
            data.segmentAttachSettingsChance = reader.Read<float>();
            data.jointSettingsChance = reader.Read<float>();
            data.newAddonChance = reader.Read<float>();
            data.removeAddonChance = reader.Read<float>();
            data.addonSettingsChance = reader.Read<float>();
            data.recursionChance = reader.Read<float>();
            data.symmetryChance = reader.Read<float>();

            data.survivalByRaffle = reader.Read<bool>();
            data.survivalByRank = reader.Read<bool>();
            data.survivalRate = reader.Read<float>();
            data.survivalStochastic = reader.Read<bool>();
            data.useCrossover = reader.Read<bool>();
            data.useMutation = reader.Read<bool>();
            data.useSpeciation = reader.Read<bool>();

            if (fileVersion >= 1) {
                // new attributes
            }
        }
    }
	
	/* ! Don't modify anything below this line ! */
	public ES2UserType_CrossoverManager():base(typeof(CrossoverManager)){}
}
