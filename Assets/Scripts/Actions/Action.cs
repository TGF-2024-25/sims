using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : MonoBehaviour
{
    protected int id;
    protected string name;
    protected static Dictionary<string, List<string>> parametersOptions;
    protected bool ordered;

    public Action(string name)
    {
        this.name = name;
    }

    public abstract void doAction();

    public int getId()
    {
        return this.id;
    }

}
