using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameAction
{
    protected int id;
    protected string name;
    protected bool ordered;
    protected GameObject crewMember;

    public GameAction(string name, GameObject crewMember)
    {
        this.name = name;
        this.crewMember = crewMember;
    }

    public abstract void doAction();

    public int getId()
    {
        return this.id;
    }

}
