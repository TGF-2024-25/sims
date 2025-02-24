using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Material
{
    public Material(string name)
    {
        this.name = name;
    }
    private string name;
    public string getName()
    {
        return name;
    }
    public override string ToString()
    {
        return name;
    }

    public override bool Equals(object obj)
    {
        return this.ToString() == obj.ToString();
    }
    public override int GetHashCode()
    {
        return name.GetHashCode();
    }
}
