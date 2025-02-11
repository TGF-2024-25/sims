using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameAction
{
    protected int id;
    protected string name;
    protected bool ordered;
    protected GameObject crewMember;

    public GameAction(string name, GameObject crewMember,bool ordered)
    {
        this.name = name;
        this.crewMember = crewMember;
        this.ordered = ordered;
    }

    public abstract void doAction();
    public abstract bool correctFacility(string facility);

    public int getId()
    {
        return this.id;
    }

}
