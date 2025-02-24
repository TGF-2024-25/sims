using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet
{
    private string name;
    private bool investigated;
    private int foodAvaible;
    private Dictionary<Resource, int> obtainableResources;
    public Planet(int shipLevel)
    {
        string jsonFile = Resources.Load<TextAsset>("PlanetData").ToString();
        Dictionary<string, object> data = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonFile);

        // Extract resources data
        Dictionary<string, List<string>> resourcesData = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(data["resources"].ToString());
        Dictionary<string, List<string>> quantityData = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(data["quantity"].ToString());
        List<string> namesData = JsonConvert.DeserializeObject<List<string>>(data["names"].ToString());

        int randomNameIndex = Random.Range(0, namesData.Count);
        name = namesData[randomNameIndex];

        investigated = false;
        obtainableResources = new Dictionary<Resource, int>();

        List<string> foodRange = quantityData["food"];
        int foodMin = int.Parse(foodRange[0]);
        int foodMax = int.Parse(foodRange[1]);
        foodAvaible = Random.Range(foodMin, foodMax);

        foreach (var resourceEntry in resourcesData)
        {
            int resourceTier = int.Parse(resourceEntry.Key);
            if (resourceTier <= shipLevel)
            {
                foreach (string resourceName in resourceEntry.Value)
                {
                    Resource resource = new Resource(resourceName,resourceTier);
                    List<string> resourceRange = quantityData[resourceEntry.Key];
                    int minQuantity = int.Parse(resourceRange[0]);
                    int maxQuantity = int.Parse(resourceRange[1]);
                    int generatedAmount = Random.Range(minQuantity, maxQuantity);
                    obtainableResources[resource] = generatedAmount;

                }
            }
        }
    }

    public string GetName()
    {
        return name;
    }

    public bool IsInvestigated()
    {
        return investigated;
    }

    public int GetFoodAvailable()
    {
        return foodAvaible;
    }

    public Dictionary<Resource, int> GetObtainableResources()
    {
        return obtainableResources;
    }

}
