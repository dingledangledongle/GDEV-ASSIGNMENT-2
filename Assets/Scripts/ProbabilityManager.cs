using System.Collections.Generic;
using UnityEngine;
public class ProbabilityManager
{

    public static T SelectWeightedItem<T>(Dictionary<T, float> weightedItems)
    {
        float totalWeight = 0f;
        bool itemAcquired = false;
        foreach (float weight in weightedItems.Values)
        {
            totalWeight += weight;
        }
        while (!itemAcquired)
        {
            foreach (var item in weightedItems)
            {
                float randomValue = Random.Range(0f, totalWeight);
                float currentWeight = item.Value;
                if (randomValue < currentWeight)
                {
                    return item.Key;
                }

            }
        }
        
        Debug.Log("returning default");
        return default;
    }

}
