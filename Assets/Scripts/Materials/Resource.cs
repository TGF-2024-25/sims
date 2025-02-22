using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource
{
    public Resource(string name, int rarity)
    {
        this.name = name;
        this.rarity = rarity;
    }
    private string name;
    private int rarity;
    public override string ToString()
    {
        return name;
    }
    public string getName()
    {
        return name;
    }
}
