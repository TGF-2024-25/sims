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

    void Start()
    {
        AM = ActionManagerObject.GetComponent<ActionManager>();

        targetPosition = transform.position;

        //SendPromptToLLM(outerPrompt);

        string content = "I want you to fill up the engine about 10 percent of its capacity";
        List<string> possibleActions = new List<string> { EatAction.NAME, RefillAction.NAME };
        List<string> previousActions = new List<string> { "Eat 2 rations", "Refill 20%" };
        string context = "I am a cook, with the next stats currently: 80 energy, 13 food, moral 69. And these are my characteristics: funny, brave";

        AM.generateAction(content, possibleActions, previousActions, context, this);

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