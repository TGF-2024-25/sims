using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FCLabBehaviour : FCBehaviour
{
    public const string NAME = "Lab";
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
            ResearchAction researchAction = (ResearchAction)crewScript.getCurrentAction();
            int hours = researchAction.getHours();

            Debug.Log("Researching for " + hours + " hours");

            crewScript.orderDone();
            crewScript.setDoingAction(false);
        }

    }
}
