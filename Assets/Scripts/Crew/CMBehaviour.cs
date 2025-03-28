using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMBehaviour : MonoBehaviour
{
    private GameObject ActionManagerObject;
    private ActionManager AM;

    private GameObject CMInfoCanvas;

    private List<GameAction> orderedActionsList;
    private List<string> possibleActions;
    private List<string> previousActions;

    private GameAction currentAction;
    private bool doingAction;
    private bool inFacility;
    private ShipBehaviour shipScript;

    private int hunger;
    private string job;
    private string cmName;
    private string personality;

    void Awake()
    {
      
        orderedActionsList = new List<GameAction>();
        possibleActions = new List<string>();
        previousActions = new List<string>();

        possibleActions.Add(EatAction.NAME);
        possibleActions.Add(ResearchAction.NAME);
        possibleActions.Add(RefillAction.NAME);
        possibleActions.Add(CraftAction.NAME);

        currentAction = null;
        doingAction = false;
        inFacility = false;
        hunger = 50;
        InvokeRepeating(nameof(loseHunger), 10f, 10f);
    }

    void Start()
    {

    }

    void Update()
    {
        if (!doingAction)
        {
            doingAction = true;
            AM.chooseNextAction(orderedActionsList, gameObject, possibleActions, shipScript);
        }
    }

    public void Initialize(string newName, string newPersonality, string newJob, ShipBehaviour newShipScript, GameObject AMObject, GameObject cmInfoCanvas)
    {
        cmName = newName;
        personality = newPersonality;
        job = newJob;
        shipScript = newShipScript;
        ActionManagerObject = AMObject;
        CMInfoCanvas = cmInfoCanvas;

        AM = ActionManagerObject.GetComponent<ActionManager>();
        Debug.Log(newName + " " + newPersonality + " " + job);
    }

    public void simulateOrder(string order)
    {
        AM.generateOrder(order, gameObject, shipScript);
    }

    public void updateActionList(GameAction action)
    {
        this.orderedActionsList.Add(action);
    }

    internal void orderDone()
    {
        currentAction = null;
    }

    public GameAction getCurrentAction()
    {
        return this.currentAction;
    }
    public void setCurrentAction(GameAction gameAction)
    {
        this.currentAction = gameAction;
        orderedActionsList.Remove(gameAction);
    }

    public void setDoingAction(bool newDoingAction)
    {
        this.doingAction = newDoingAction;
    }

    public string getContext()
    {
        string context = "I am ";
        context += cmName;
        context += ", my personality is ";
        context += personality;
        context += ", and my job is to ";
        context += job;
        context += ". My hunger level is at ";
        context += hunger;
        context += ". ";
        context += "My previous actions are: ";
        foreach(var action in previousActions)
        {
            context += action;
            context += ", ";
        }
        context += ". ";


        return context;
    }

    internal void addPreviousAction(GameAction action)
    {
        if(previousActions.Count == 10)
        {
            previousActions.RemoveAt(0);
        }
        previousActions.Add(action.ToString());
    }

    public void explore()
    {
        Debug.Log(cmName + " going to explore");
    }

    public int getHunger()
    {
        return this.hunger;
    }

    public void setHunger(int hunger)
    {
        this.hunger = hunger;
    }
    public string getName()
    {
        return this.cmName;
    }
    public string getPersonality()
    {
        return this.personality;
    }
    public string getJob()
    {
        return this.job;
    }

    public void setInFacility(bool inFacility)
    {
        this.inFacility = inFacility;
    }
    public bool getInFacility()
    {
        return inFacility;
    }

    private void loseHunger()
    {
        this.hunger -= 2;
    }

    public void OnClick()
    {
        GameObject cmUIPanel = CMInfoCanvas.transform.Find("CMInfoPanel").gameObject;
        CMInfoUI crewUI = cmUIPanel.GetComponent<CMInfoUI>();
        crewUI.ShowCrewInfo(gameObject);
    }

}