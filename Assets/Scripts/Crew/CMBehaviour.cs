using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMBehaviour : MonoBehaviour
{
    public GameObject ActionManagerObject;
    private ActionManager AM;

    private int x;
    private int y;

    private Vector2 targetPosition;

    void Awake()
    {
        AM = ActionManagerObject.GetComponent<ActionManager>();

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
        string content = "I want you to eat 3 rations of food";
        List<string> possibleActions = new List<string> { EatAction.NAME, RefillAction.NAME , "idle"};

        AM.generateAction(content, possibleActions, this);
    }

    void Update()
    {
        if ((Vector2)transform.position != targetPosition)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, Time.deltaTime * 2f);
        }
    }

    public void UpdatePosition(int newX, int newY)
    {
        x = newX;
        y = newY;
        targetPosition = new Vector2(x, y);
    }
}