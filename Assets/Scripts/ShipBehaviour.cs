using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UIElements;

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

    [SerializeField]
    public GameObject crewMemberPrefab;

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

    public void CreateCrewMember()
    {
        Vector3 position = new Vector3 (0, 0, 0);
        GameObject newCrewMember = Object.Instantiate(crewMemberPrefab, position, Quaternion.identity);

        string name;
        string personality;
        string job;

        
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
