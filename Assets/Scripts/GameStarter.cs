using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using System.Xml;
using Newtonsoft.Json;

public class GameStarter : MonoBehaviour 
{
    [SerializeField]
    public GameObject ShipObject;
    private ShipBehaviour shipScript;

    // Start is called before the first frame update
    void Awake()
    {
        shipScript = ShipObject.GetComponent<ShipBehaviour>();

        loadData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadData()
    {
        loadActionParameterOptions();
        loadShipData();
    }




    private void loadActionParameterOptions()
    {
        string jsonFile = Resources.Load<TextAsset>("ActionData").ToString();
        Dictionary<string, Dictionary<string, List<string>>> data = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, List<string>>>>(jsonFile);
        EatAction.loadParameterOptions(data[EatAction.NAME]);
        RefillAction.loadParameterOptions(data[RefillAction.NAME]);
        ResearchAction.loadParameterOptions(data[ResearchAction.NAME]);
        CraftAction.loadParameterOptions(data[CraftAction.NAME]);
    }

    private void loadShipData()
    {
        Dictionary<Resource, int> inventoryResources = new Dictionary<Resource, int>();
        Dictionary<Material, int> inventoryMaterials = new Dictionary<Material, int>();

        string jsonFileInventory = Resources.Load<TextAsset>("InventoryData").ToString();
        Dictionary<string, Dictionary<string, List<string>>> dataInventory = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, List<string>>>>(jsonFileInventory);
        Dictionary<string, List<string>> resourcesData = dataInventory["resources"];
        Dictionary<string, List<string>> materialsData = dataInventory["materials"];

        foreach (string level in resourcesData.Keys)
        {
            foreach (string resource in resourcesData[level])
            {
                Resource newResource = new Resource(resource, int.Parse(level));
                inventoryResources.Add(newResource,0);
            }
        }
        foreach (string level in materialsData.Keys)
        {
            foreach (string material in materialsData[level])
            {
                Material newMaterial = new Material(material);
                inventoryMaterials.Add(newMaterial, 0);
            }
        }

        List<string> crewNameOptions = new List<string>();
        List<string> crewPersonalityOptions = new List<string>();
        List<string> crewJobOptions = new List<string>();

        string jsonFileCM = Resources.Load<TextAsset>("CMData").ToString();
        Dictionary<string, List<string>> dataCM = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(jsonFileCM);
        List<string> nameOps = dataCM["names"];
        List<string> personalityOps = dataCM["personalities"];
        List<string> jobOps = dataCM["jobs"];

        shipScript.loadData(inventoryResources, inventoryMaterials, nameOps, personalityOps, jobOps);
    }
}
