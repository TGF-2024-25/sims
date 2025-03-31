using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class FCWorkshopBehaviour : FCBehaviour
{
    public const string NAME = "Workshop";
    bool colliding;
    private CMBehaviour crewScript;
    private List<(Recipe, bool)> recipeList = new List<(Recipe, bool)>();
    [SerializeField] private TextAsset jsonFile;
    private ShipBehaviour shipScript;
    Recipe currentRecipe;
    // Start is called before the first frame update
    void Start()
    {
        string jsonFile = Resources.Load<TextAsset>("RecipeData").ToString();
        LoadRecipeFromJson(jsonFile);
        colliding = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (colliding && crewScript.getInFacility() && crewScript.getCurrentAction() != null && crewScript.getCurrentAction().correctFacility(NAME))
            startCraft();
    }

    public void setShip(ShipBehaviour shipBehaviour)
    {
        shipScript = shipBehaviour;
    }

    void LoadRecipeFromJson(string json)
    {
        Dictionary<string, Dictionary<string, List<string>>> researchDict = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, List<string>>>>(json);

        foreach (var recipe in researchDict)
        {
            List<Material> materialsNeeded = new List<Material>();
            foreach (var material  in recipe.Value["materials"])
            {
                materialsNeeded.Add(new Material(material));
            }
            List<Resource> resourcesNeeded = new List<Resource>();
            foreach (var resource in recipe.Value["resources"])
            {
                resourcesNeeded.Add(new Resource(resource,0));
            }
            recipeList.Add((new Recipe(new Material(recipe.Key),  materialsNeeded,resourcesNeeded ), false));
           
        }
    }

    public void unlockRecipe(Material unlock)
    {
        for (int i = 0; i < recipeList.Count; i++)
        {
            if (recipeList[i].Item1.GetCraftedMaterial().getName() == unlock.getName())
            {
                recipeList[i] = (recipeList[i].Item1, true);
                CraftAction.addMaterial(unlock);
            }
           
        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject crewMember = collision.gameObject;
        CMBehaviour crewScriptAux = crewMember.GetComponent<CMBehaviour>();
        GameAction action = crewScriptAux.getCurrentAction();

        if (action.correctFacility(NAME))
        {
            crewScript = crewScriptAux;
            colliding = true;
            crewScript.setInFacility(true);
        }

    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        colliding = false;
    }

    private void startCraft()
    {
        CraftAction craftAction = (CraftAction)crewScript.getCurrentAction();
        int quantity = craftAction.getQuantity();
        string material = craftAction.getMaterial();

        currentRecipe = null;

        for (int i = 0; i < recipeList.Count; i++)
        {
            if (recipeList[i].Item1.GetCraftedMaterial().getName() == material)
            {
                currentRecipe = recipeList[i].Item1;
            }

        }
        //TODO EL METODO ESTE POR AHORA SIEMPRE TE DEJA CRAFTEAR
        bool canCraft = false;
        if (currentRecipe != null)
        {
            canCraft = currentRecipe.canCraft(shipScript.GetInventoryMaterials(), shipScript.GetInventoryResources());
        }
        

        if (canCraft)
        {
            crewScript.setInFacility(false);
            Invoke("craft", 5f);
        }
        else
        {
            Debug.Log("not enough resources");
            crewScript.orderDone();
            crewScript.setDoingAction(false);
            crewScript.setInFacility(true);
        }
        
       
    }

    private void craft()
    {
        Debug.Log("Crafting " + currentRecipe.GetCraftedMaterial().getName());
        Dictionary<Material,int> materials = shipScript.GetInventoryMaterials();
        
        materials[currentRecipe.GetCraftedMaterial()] = materials[currentRecipe.GetCraftedMaterial()] + 1;
        foreach (var material in currentRecipe.GetMaterialsNeeded())
        {
            materials[material] = materials[material] - 1;
        }

        Dictionary<Resource, int> resources = shipScript.GetInventoryResources();
        foreach (var resource in currentRecipe.GetResourcesNeeded())
        {
            resources[resource] = resources[resource] - 1;
        }
        crewScript.orderDone();
        crewScript.setDoingAction(false);
        crewScript.setInFacility(true);
    }

    public string getContext()
    {
        System.Text.StringBuilder context = new System.Text.StringBuilder();
        context.Append("In the workshop, I can craft the following unlocked recipes:\n");

        foreach (var recipeEntry in recipeList)
        {
            Recipe recipe = recipeEntry.Item1;
            bool isUnlocked = recipeEntry.Item2;

            if (isUnlocked)
            {
                context.Append("- " + recipe.GetCraftedMaterial().getName() + " requires:\n");

                foreach (var material in recipe.GetMaterialsNeeded())
                {
                    context.Append("  * Material: " + material.getName() + "\n");
                }

                foreach (var resource in recipe.GetResourcesNeeded())
                {
                    context.Append("  * Resource: " + resource.getName() + "\n");
                }
            }
        }

        return context.ToString() + ". Its not posible to do the craft action if no recipes are avaible.";
    }

    public override void OnClick()
    {
        Debug.Log("Click on Workshop");
    }

}
