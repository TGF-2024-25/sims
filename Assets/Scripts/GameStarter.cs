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
        loadInventoryData();
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

    private void loadInventoryData()
    {
        Dictionary<Resource, int> inventoryResources = new Dictionary<Resource, int>();
        Dictionary<Material, int> inventoryMaterials = new Dictionary<Material, int>();

        string jsonFile = Resources.Load<TextAsset>("InventoryData").ToString();
        Dictionary<string, Dictionary<string, List<string>>> data = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, List<string>>>>(jsonFile);
        Dictionary<string, List<string>> resourcesData = data["resources"];
        Dictionary<string, List<string>> materialsData = data["materials"];

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

        shipScript.loadInventory(inventoryResources, inventoryMaterials);
    }
}
