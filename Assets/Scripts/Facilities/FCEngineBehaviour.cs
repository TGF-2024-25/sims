using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FCEngineBehaviour : FCBehaviour
{
    public const string NAME = "Engine";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject crewMember = collision.gameObject;
        CMBehaviour crewScript = crewMember.GetComponent<CMBehaviour>();
        GameAction action = crewScript.getCurrentAction();

        if (action.correctFacility(NAME))
        {
            RefillAction refillAction = (RefillAction)crewScript.getCurrentAction();
            string justEnough = refillAction.getJustEnough();
            int percentage = refillAction.getPercentage();
            if (justEnough.Equals("yes"))
            {
                Debug.Log("Filling engine just enough");
            }
            else
            {
                Debug.Log("Filling engine to " + percentage + "%");
            }
            crewScript.orderDone();
            crewScript.setDoingAction(false);
        }

    }
}
