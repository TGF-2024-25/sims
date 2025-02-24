using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Expedition
{
    private int timeInExpedition;
    private List<GameObject> explorers;

    // Constructor
    public Expedition(List<GameObject> initialExplorers)
    {
        timeInExpedition = 0;
        explorers = new List<GameObject>(initialExplorers);
        
    }

    public int GetTimeInExpedition()
    {
        return timeInExpedition;
    }

    public List<GameObject> GetExplorers()
    {
        return explorers;
    }
    
    private void explore()
    {
        timeInExpedition += 1;
    }
}

