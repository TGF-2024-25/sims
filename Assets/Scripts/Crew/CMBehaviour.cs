using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMBehaviour : MonoBehaviour
{
    public GameObject ActionManagerObject;
    private ActionManager AM;

    private List<Action> posibleActionsList;

    private Vector2 targetPosition;

    void Awake()
    {
        AM = ActionManagerObject.GetComponent<ActionManager>();
        posibleActionsList = new List<Action>();
        targetPosition = transform.position;
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

        AM.generateAction(content, possibleActions, this);
    }

    void Update()
    {
        if(posibleActionsList.Count != 0)
        {
            posibleActionsList[0].doAction();
        }
        if ((Vector2)transform.position != targetPosition)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, Time.deltaTime * 2f);
        }
    }
    
    public void updateActionList(Action action)
    {
        this.posibleActionsList.Add(action);
    }
}