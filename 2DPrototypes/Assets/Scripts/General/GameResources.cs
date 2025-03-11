using System;
using System.Collections.Generic;

public static class GameResources 
{

    private static Dictionary<ResourceType, int> resourceAmountDictionary;

    public static event EventHandler onResourceAmountChanged;


    public static void Init()
    {
        resourceAmountDictionary = new Dictionary<ResourceType, int>();

        foreach (ResourceType resourceType in Enum.GetValues(typeof(ResourceType)))
        {
            resourceAmountDictionary[resourceType] = 0;
        }
    }

    public static void AddResourceAmount(ResourceType resourceType, int amount)
    {
        resourceAmountDictionary[resourceType] += amount;
        if (onResourceAmountChanged != null)
            onResourceAmountChanged(null, EventArgs.Empty);
    }

    public static int GetResourceAmount(ResourceType resourceType) => resourceAmountDictionary[resourceType];
}

public enum ResourceType
{
    Wood,
    Stone
}