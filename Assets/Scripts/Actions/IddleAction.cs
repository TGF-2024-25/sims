using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IddleAction : GameAction
{
    public const string NAME = "iddle";
    public IddleAction(CMBehaviour crewMember) : base(NAME, crewMember)
    {

    }
    public override void doAction()
    {
        return;
    }
}
