using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipe
{
    // Atributos privados
    private Material craftedMaterial;
    private List<Material> materialsNeeded;
    private List<Resource> resourcesNeeded;

    // Constructor
    public Recipe(Material craftedMaterial, List<Material> materialsNeeded, List<Resource> resourcesNeeded)
    {
        this.craftedMaterial = craftedMaterial;
        this.materialsNeeded = materialsNeeded;
        this.resourcesNeeded = resourcesNeeded;
    }

    // Getters
    public Material GetCraftedMaterial()
    {
        return craftedMaterial;
    }

    public List<Material> GetMaterialsNeeded()
    {
        return materialsNeeded;
    }

    public List<Resource> GetResourcesNeeded()
    {
        return resourcesNeeded;
    }

    // Setters
    public void SetCraftedMaterial(Material craftedMaterial)
    {
        this.craftedMaterial = craftedMaterial;
    }

    public void SetMaterialsNeeded(List<Material> materialsNeeded)
    {
        this.materialsNeeded = materialsNeeded;
    }

    public void SetResourcesNeeded(List<Resource> resourcesNeeded)
    {
        this.resourcesNeeded = resourcesNeeded;
    }

    internal bool canCraft(Func<Dictionary<Material, int>> getInventoryMaterials, Func<Dictionary<Resource, int>> getInventoryResources)
    {
        return true;
    }
}

