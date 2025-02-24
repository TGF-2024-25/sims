using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIManager
{
    public static int GetHighestSortingOrder()
    {
        Canvas[] canvases = GameObject.FindObjectsOfType<Canvas>();
        int maxOrder = 0;
        foreach (Canvas canvas in canvases)
        {
            if (canvas.sortingOrder > maxOrder)
                maxOrder = canvas.sortingOrder;
        }
        return maxOrder + 1;
    }
}

