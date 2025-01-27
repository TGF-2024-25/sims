using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FCKitchenBehaviour : FCBehaviour
{
    public const string NAME = "Kitchen";
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
            crewScript.setDoingAction(false);
        }

    }
}
