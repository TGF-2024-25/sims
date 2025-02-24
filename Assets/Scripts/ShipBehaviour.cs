using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ShipBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject ActionManagerObject;

    [SerializeField]
    private GameObject CMInfoCanvas;

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
    private GameObject NavigationObject;
    private FCNavigation navigationScript;

    [SerializeField]
    private GameObject crewMemberPrefab;

    private List<string> crewNameOptions;
    private List<string> crewPersonalityOptions;
    private List<string> crewJobOptions;


    public Dictionary<Resource,int> inventoryResources;
    public Dictionary<Material, int> inventoryMaterials;

    private List<GameObject> crewMembers;
    private List<string> crewMembersNames;
    private int level;




    void Awake()
    {
        level = 1;
        crewMembers = new List<GameObject>();
        crewMembersNames = new List<string>();

        labScript = LabObject.GetComponent<FCLabBehaviour>();
        workshopScript = WorkshopObject.GetComponent<FCWorkshopBehaviour>();
        navigationScript = NavigationObject.GetComponent<FCNavigation>();
        navigationScript.setShip(this);
        workshopScript.setShip(this);
        labScript.setWorkshop(workshopScript);
        kitchenScript = KitchenObject.GetComponent<FCKitchenBehaviour>();
        engineScript = EngineObject.GetComponent<FCEngineBehaviour>();
    }

    void Start()
    {
        

        CreateCrewMember();
        

        CMBehaviour cm1script = crewMembers[0].GetComponent<CMBehaviour>();

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void loadData(Dictionary<Resource, int> inventoryRes, Dictionary<Material, int> inventoryMat, List<string> nameOps, List<string> personalityOps, List<string> jobOps)
    {
        inventoryResources = inventoryRes;
        inventoryMaterials = inventoryMat;

        crewNameOptions = nameOps;
        crewPersonalityOptions = personalityOps;
        crewJobOptions = jobOps;

}

    public void CreateCrewMember()
    {
        Vector3 position = Vector3.zero;


        GameObject newCrewMember = Instantiate(crewMemberPrefab, position, Quaternion.identity);
        CMBehaviour crewScript = newCrewMember.GetComponent<CMBehaviour>();

        string name = crewNameOptions[Random.Range(0, crewNameOptions.Count())];
        while (crewMembersNames.Contains(name))
        {
            name = crewNameOptions[Random.Range(0, crewNameOptions.Count())];
        }
        string personality = crewPersonalityOptions[Random.Range(0, crewPersonalityOptions.Count())];
        string job = crewJobOptions[Random.Range(0, crewJobOptions.Count())];
        
        crewScript.Initialize(name, personality, job, this, ActionManagerObject, CMInfoCanvas);
        crewMembers.Add(newCrewMember);
        crewMembersNames.Add(name);
    }

    public string getContext()
    {
        string context = "Within the ship ";
        context += engineScript.getContext() + ", ";
        context += labScript.getContext() + ", ";
        context += workshopScript.getContext() + ", ";
        context += kitchenScript.getContext();

        return context;
    }

    public int getLevel()
    {
        return level;
    }

    public void setLevel(int lvl)
    {
        level = lvl;
    }
    // Getters
    public Dictionary<Resource, int> GetInventoryResources()
    {
        return inventoryResources;
    }

    public Dictionary<Material, int> GetInventoryMaterials()
    {
        return inventoryMaterials;
    }

    // Setters
    public void SetInventoryResources(Dictionary<Resource, int> inventoryRes)
    {
        inventoryResources = inventoryRes;
    }

    public void SetInventoryMaterials(Dictionary<Material, int> inventoryMat)
    {
        inventoryMaterials = inventoryMat;
    }

}
