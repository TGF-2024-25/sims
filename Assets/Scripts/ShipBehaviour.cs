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
    private GameObject ActionManagerObject;

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
    private GameObject crewMemberPrefab;

    private List<string> crewNameOptions;
    private List<string> crewPersonalityOptions;
    private List<string> crewJobOptions;


    public Dictionary<Resource,int> inventoryResources;
    public Dictionary<Material, int> inventoryMaterials;

    private List<GameObject> crewMembers;
    private List<string> crewMembersNames;




    void Awake()
    {
        crewMembers = new List<GameObject>();
        crewMembersNames = new List<string>();

        labScript = LabObject.GetComponent<FCLabBehaviour>();
        workshopScript = WorkshopObject.GetComponent<FCWorkshopBehaviour>();
        kitchenScript = KitchenObject.GetComponent<FCKitchenBehaviour>();
        engineScript = EngineObject.GetComponent<FCEngineBehaviour>();
    }

    void Start()
    {
        CreateCrewMember();
        CreateCrewMember();

        CMBehaviour cm1script = crewMembers[0].GetComponent<CMBehaviour>();
        CMBehaviour cm2script = crewMembers[1].GetComponent<CMBehaviour>();

        string content = "please put more fuel in the tank untill is 75% full";
        cm1script.simulateOrder(content);
        string content2 = "go investigate 4 hours";
        cm2script.simulateOrder(content2);
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
        
        crewScript.Initialize(name, personality, job, this, ActionManagerObject);
        crewMembers.Add(newCrewMember);
        crewMembersNames.Add(name);
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
