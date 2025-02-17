using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Research
{
    private int duration;
    private Material material;
    private int level;

    // Constructor
    public Research(int duration, Material material, int level)
    {
        this.duration = duration;
        this.material = material;
        this.level = level;
    }

    // Getters
    public int getDuration()
    {
        return duration;
    }

    public Material getMaterial()
    {
        return material;
    }

    public int getLevel()
    {
        return level;
    }
}
