using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IddleAction : GameAction
{
    public const string NAME = "iddle";
    public IddleAction() : base(NAME)
    {

    }
    public override void doAction()
    {
        return;
    }
}
