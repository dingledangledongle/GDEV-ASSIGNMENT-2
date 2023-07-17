using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class ProbabilityManager
{
    public Node.Encounter SelectWeightedItem(Dictionary<Node.Encounter,float> weightedItems)
    {
        float totalWeight = 0f;

        foreach(var weight in weightedItems.Values)
        {
            totalWeight += weight;
        }

        foreach(var item in weightedItems)
        {
            float randomValue = Random.Range(0f, totalWeight);
            float currentWeight = item.Value;
            if (randomValue < currentWeight)
            {
                return item.Key;
            }
        }

        return default;
    }

}
