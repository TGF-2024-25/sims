using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMBehaviour : MonoBehaviour
{
    public GameObject ActionManagerObject;
    private ActionManager AM;

    private List<GameAction> orderedActionsList;

    private GameAction currentAction;
    private bool doingAction;

    void Awake()
    {
        AM = ActionManagerObject.GetComponent<ActionManager>();
        orderedActionsList = new List<GameAction>();
        currentAction = null;
        doingAction = false;
    }

    void Start()
    {
        string content = "please put more fuel in the tank untill is 75% full";
        simulateOrder(content);
        string content2 = "go eat 2 racions";
        simulateOrder(content2);
        string content3 = "go investigate 4 hours";
        simulateOrder(content3);
    }

    public void simulateOrder(string order)
    {
        //string content = "I want you to fill up the engine about 10 percent of its capacity";
        //string content = "I want you to go to the bathroom";

        AM.generateAction(order, gameObject);
    }

    void Update()
    {
        if(!doingAction)
        {
            doingAction = true;
            AM.chooseNextAction(orderedActionsList, this);  
        }
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

    public void setDoingAction(bool doingAction)
    {
        this.doingAction = doingAction;
    }
}