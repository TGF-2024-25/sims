using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionFactory
{
    List<string> actionList;
    ActionFactory aF;

    public ActionFactory createActionFactory()
    {
        if(this.aF != null)
        {
            aF = new ActionFactory();
        }
        return aF;
    }

    public ActionFactory()
    {
        actionList.Add(EatAction.NAME);
        actionList.Add(RefillAction.NAME);
    }
    
    public Action createActionByName(string name, Dictionary<string, string> parameters)
    {
        switch (name)
        {
            case EatAction.NAME:
                return new EatAction(parameters);
            case RefillAction.NAME:
                return new RefillAction(parameters);
            default:
                return null;
        }
    }

    public void loadParameterOptions()
    {

    }
}
