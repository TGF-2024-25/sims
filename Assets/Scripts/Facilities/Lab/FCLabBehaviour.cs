using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class FCLabBehaviour : FCBehaviour
{
    public const string NAME = "Lab";
    [SerializeField] private TextAsset jsonFile; 
    private List<Research> researchList = new List<Research>();
    
    void Start()
    {
        string jsonFile = Resources.Load<TextAsset>("ResearchData").ToString();
        LoadResearchFromJson(jsonFile);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadResearchFromJson(string json)
    {
        Dictionary<string, Dictionary<string, int>> researchDict = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, int>>>(json);

        foreach (var research in researchDict)
        {
            researchList.Add(new Research(research.Value["level"], new Material(research.Key), research.Value["duration"]));
        }

        // Debugging: Print the loaded research items
        foreach (var research in researchList)
        {
            Debug.Log($"Material: {research.getMaterial()}, Level: {research.getLevel()}, Duration: {research.getDuration()}");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject crewMember = collision.gameObject;
        CMBehaviour crewScript = crewMember.GetComponent<CMBehaviour>();
        GameAction action = crewScript.getCurrentAction();

        if (action.correctFacility(NAME))
        {
            ResearchAction researchAction = (ResearchAction)crewScript.getCurrentAction();
            int hours = researchAction.getHours();

            Debug.Log("Researching for " + hours + " hours");

            crewScript.orderDone();
            crewScript.setDoingAction(false);
        }

    }
}
