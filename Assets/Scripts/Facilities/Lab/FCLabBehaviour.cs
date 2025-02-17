using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class FCLabBehaviour : FCBehaviour
{
    public const string NAME = "Lab";
    [SerializeField] private TextAsset jsonFile;
    private List<(Research, string,int)> researchList = new List<(Research, string,int)>();
    private int progress;
    private int currentProgressGoal;
    private int researchTime;
    private int currentResearch;
    CMBehaviour crewScript;
    bool collinding;
    void Start()
    {
        string jsonFile = Resources.Load<TextAsset>("ResearchData").ToString();
        LoadResearchFromJson(jsonFile);
        progress = 0;
        currentProgressGoal = 0;
        researchTime = 0;
        currentResearch = 0;
        collinding = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (collinding && !crewScript.getInFacility() && crewScript.getCurrentAction() != null)
            startResearch();
            
    }

    void LoadResearchFromJson(string json)
    {
        Dictionary<string, Dictionary<string, int>> researchDict = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, int>>>(json);

        foreach (var research in researchDict)
        {
            researchList.Add((new Research(research.Value["duration"], new Material(research.Key), research.Value["level"]), research.Value["level"] == 1 ? "unlocked" : "locked",0));
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        GameObject crewMember = collision.gameObject;
        crewScript = crewMember.GetComponent<CMBehaviour>();
        GameAction action = crewScript.getCurrentAction();
        Debug.Log(crewScript.getInFacility());
        if (action.correctFacility(NAME) && !crewScript.getInFacility())
        {
            startResearch();
            collinding = true;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collinding = false;
    }

    private void startResearch()
    {
        crewScript.setInFacility(true);
        ResearchAction researchAction = (ResearchAction)crewScript.getCurrentAction();
        researchTime = researchAction.getHours();
        bool notFound = true;
        int i = 0;
        while (notFound && i < researchList.Count)
        {
            if (researchList[i].Item2 == "unlocked" && researchList[i].Item3 < researchList[i].Item1.getDuration())
            {
                notFound = false;
            }
            else
            {
                i++;
            }
        }

        if (!notFound)
        {
            progress = researchList[i].Item3;
            currentProgressGoal = researchList[i].Item1.getDuration();
            currentResearch = i;
            InvokeRepeating("research", 2f, 2f);
        }

    }

    private void research()
    {
        progress += 1;
        researchTime -= 1;
        Debug.Log("Current progress: " + progress + "in: " + researchList[currentResearch].Item1.getMaterial().getName());
        Debug.Log("Current time left: " + researchTime);

        if (currentProgressGoal == progress || researchTime == 0)
        {
            researchList[currentResearch] = (researchList[currentResearch].Item1, researchList[currentResearch].Item2, progress);
            crewScript.orderDone();
            crewScript.setDoingAction(false);
            crewScript.setInFacility(false);
            CancelInvoke("research");
        }
    }


}
