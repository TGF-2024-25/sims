using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMBehaviour : MonoBehaviour
{
    private GameObject ActionManagerObject;
    private ActionManager AM;

    private List<GameAction> orderedActionsList;
    private List<string> possibleActions;
    private List<string> previousActions;

    private GameAction currentAction;
    private bool doingAction;
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

        currentAction = null;
        doingAction = false;
        hunger = 50;
        InvokeRepeating(nameof(loseHunger), 10f, 10f);
    }

    void Start()
    {
        //string content = "please put more fuel in the tank untill is 75% full";
        //simulateOrder(content);
        //string content2 = "go eat 2 racions";
        //simulateOrder(content2);
        //string content3 = "go investigate 4 hours";
        //simulateOrder(content3);
    }

    void Update()
    {
        if (!doingAction)
        {
            doingAction = true;
            AM.chooseNextAction(orderedActionsList, gameObject, possibleActions);
        }
    }

    public void Initialize(string newName, string newPersonality, string newJob, ShipBehaviour newShipScript, GameObject AMObject)
    {
        cmName = newName;
        personality = newPersonality;
        job = newJob;
        shipScript = newShipScript;
        ActionManagerObject = AMObject;

        AM = ActionManagerObject.GetComponent<ActionManager>();
        Debug.Log(newName + " " + newPersonality + " " + job);
    }

    public void simulateOrder(string order)
    {
        AM.generateOrder(order, gameObject);
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
        context += ".";

        Debug.Log(context);

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

    public int getHunger()
    {
        return this.hunger;
    }

    public void setHunger(int hunger)
    {
        this.hunger = hunger;
    }

    private void loseHunger()
    {
        this.hunger -= 2;
    }
}