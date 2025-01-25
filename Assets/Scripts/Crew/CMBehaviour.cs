using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMBehaviour : MonoBehaviour
{
    public GameObject ActionManagerObject;
    private ActionManager AM;

    private List<GameAction> posibleActionsList;

    void Awake()
    {
        AM = ActionManagerObject.GetComponent<ActionManager>();
        posibleActionsList = new List<GameAction>();
    }

    void Start()
    {

        simulateOrder();

    }

    public void simulateOrder()
    {
        //string content = "I want you to fill up the engine about 10 percent of its capacity";
        //string content = "I want you to go to the bathroom";
        string content = "I want you to eat 2 rations of bad food";
        List<string> possibleActions = new List<string> { EatAction.NAME, RefillAction.NAME , "idle"};

        AM.generateAction(content, possibleActions, gameObject);
    }

    void Update()
    {
        if(posibleActionsList.Count != 0)
        {
            GameAction actionToDo = posibleActionsList[0];
            actionToDo.doAction();
            posibleActionsList.Remove(actionToDo);
        }
    }
    
    public void updateActionList(GameAction action)
    {
        this.posibleActionsList.Add(action);
    }
}