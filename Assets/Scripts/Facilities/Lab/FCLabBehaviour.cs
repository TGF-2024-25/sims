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
    private int currentResearchId;
    private Research currentResearch;
    private CMBehaviour crewScript;
    private bool colliding;
    private FCWorkshopBehaviour workshopScript;
    void Start()
    {
        string jsonFile = Resources.Load<TextAsset>("ResearchData").ToString();
        LoadResearchFromJson(jsonFile);
        progress = 0;
        currentProgressGoal = 0;
        researchTime = 0;
        currentResearch = researchList[0].Item1;
        currentResearchId = 0;
        colliding = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (colliding && crewScript.getInFacility() && crewScript.getCurrentAction() != null && crewScript.getCurrentAction().correctFacility(NAME))
            startResearch();
            
    }
    public void setWorkshop(FCWorkshopBehaviour fCWorkshopBehaviour)
    {
        workshopScript = fCWorkshopBehaviour;
    }
    void LoadResearchFromJson(string json)
    {
        Dictionary<string, Dictionary<string, int>> researchDict = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, int>>>(json);

        foreach (var research in researchDict)
        {
            researchList.Add((new Research(research.Value["duration"], new Material(research.Key), research.Value["level"]), research.Value["level"] == 1 ? "unlocked" : "locked",0));
        }

    }

    public void unlockResearchLevel(int level)
    {
        for(int i = 0;i < researchList.Count; i++)
        {
            if(researchList[i].Item1.getLevel() == level)
                researchList[i] = (researchList[i].Item1, "unlocked", researchList[i].Item3);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject crewMember = collision.gameObject;
        CMBehaviour crewScriptAux = crewMember.GetComponent<CMBehaviour>();
        GameAction action = crewScriptAux.getCurrentAction();
        //Debug.Log(crewScript.getInFacility());
        if (action.correctFacility(NAME))
        {
            crewScript = crewScriptAux;
            colliding = true;
            crewScript.setInFacility(true);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        colliding = false;
    }

    private void startResearch()
    {
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
            currentResearchId = i;
            crewScript.setInFacility(false);
            currentResearch = researchList[i].Item1;
            InvokeRepeating("research", 1f, 1f);
        }
        else
        {
            crewScript.orderDone();
            crewScript.setDoingAction(false);
            crewScript.setInFacility(true);
        }

    }

    private void research()
    {
        progress += 1;
        researchTime -= 1;
        //Debug.Log("Current progress: " + progress + "in: " + researchList[currentResearchId].Item1.getMaterial().getName());
        //Debug.Log("Current time left: " + researchTime);

        if (currentProgressGoal == progress || researchTime == 0)
        {
            researchList[currentResearchId] = (researchList[currentResearchId].Item1, researchList[currentResearchId].Item2, progress);
            crewScript.orderDone();
            crewScript.setDoingAction(false);
            crewScript.setInFacility(true);
            if(currentProgressGoal == progress){
                researchList[currentResearchId] = (researchList[currentResearchId].Item1, "finished", progress);
                workshopScript.unlockRecipe(currentResearch.getMaterial());
                currentResearch = researchList[currentResearchId + 1].Item2 == "unlocked" ? researchList[currentResearchId + 1].Item1 : null;
                progress = 0;
                currentProgressGoal = 0;
            }
            else
            {
                currentResearch = researchList[currentResearchId].Item1;
            }
           
            CancelInvoke("research");
        }
    }

    public string getContext()
    {
        string context = "Lab Research Status:\n";

        foreach (var research in researchList)
        {
            string status = research.Item2;
            string researchName = research.Item1.getMaterial().getName();
            int progress = research.Item3;
            int duration = research.Item1.getDuration();

            if (status == "unlocked")
            {
                context += $"- [Unlocked] {researchName}: {progress}/{duration} progress\n";
            }
            else if (status == "finished")
            {
                context += $"- [Finished] {researchName}\n";
            }
            else
            {
                context += $"- [Locked] {researchName}\n";
            }
        }

        return context + ".If all research are locked or finished the research action cant be made";
    }

    public override void OnClick()
    {
        Debug.Log("Click on Lab");
    }


}
