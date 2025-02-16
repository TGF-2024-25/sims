using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using Newtonsoft.Json;
using UnityEngine;

public class ShipBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject LabObject;
    private FCLabBehaviour labScript;
    [SerializeField]
    private GameObject WorkshopObject;
    private FCWorkshopBehaviour workshopScript;
    [SerializeField]
    private GameObject KitchenObject;
    private FCKitchenBehaviour kitchenScript;
    [SerializeField]
    private GameObject EngineObject;
    private FCEngineBehaviour engineScript;


    public Dictionary<Resource,int> inventoryResources;
    public Dictionary<Material, int> inventoryMaterials;


    void Awake()
    {

        labScript = LabObject.GetComponent<FCLabBehaviour>();
        workshopScript = WorkshopObject.GetComponent<FCWorkshopBehaviour>();
        kitchenScript = KitchenObject.GetComponent<FCKitchenBehaviour>();
        engineScript = EngineObject.GetComponent<FCEngineBehaviour>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadInventory(Dictionary<Resource, int> inventoryRes, Dictionary<Material, int> inventoryMat)
    {
        inventoryResources = inventoryRes;
        inventoryMaterials = inventoryMat;
    }

    public string getContext()
    {
        string context = "In the ship ";
        //context += engineScript.getContext();
        //context += labScript.getContext();
        //context += workshopScript.getContext();
        //context += kitchenScript.getContext();

        return context;
    }
}
