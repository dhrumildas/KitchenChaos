using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public static DeliveryManager Instance { get; private set; } // Singleton
    [SerializeField] private RecipeListSO recipeListSO; // List of all recipes
    private List<MenuSO> waitingRecipeSOList;   // List of waiting recipes
    private float spawnRecipeTimer; // Timer to spawn a new recipe
    private float spawnRecipeTimerMax = 4f; // Time to spawn a new recipe
    private int maxWaitingRecipeCount = 5;  // Max number of waiting recipes

    private void Awake()
    {
        Instance = this;    // Singleton
        waitingRecipeSOList = new List<MenuSO>();   // Initializing the list
    }

    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime; // Decreasing the timer
        if (spawnRecipeTimer <= 0f) // Time to spawn a new recipe
        {
            spawnRecipeTimer = spawnRecipeTimerMax; // Resetting the timer

            if (waitingRecipeSOList.Count < maxWaitingRecipeCount)  // If the waiting list is not full
            {

                MenuSO waitingRecipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];   // Randomly selecting a recipe
                Debug.Log(waitingRecipeSO.menuName);
                waitingRecipeSOList.Add(waitingRecipeSO);   // Adding the recipe to the waiting list

                OnRecipeSpawned?.Invoke(this, EventArgs.Empty); // Invoking the event
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObj plateKitchenObj)  // to see if the contents of the plate match the recipe
    {
        for(int i =0; i < waitingRecipeSOList.Count; i++)   // Going through the waiting menu items
        {
            MenuSO waitingRecipeSO = waitingRecipeSOList[i];    // Getting the waiting menu item
            if(waitingRecipeSO.menuItemSOList.Count == plateKitchenObj.GetKitchenObjSOList().Count) // No. of ingredients match
            {
                bool plateContentsMatch = true;
                foreach (KitchenObjectSO recipeKitchenObjSO in waitingRecipeSO.menuItemSOList)  // Going through the ingredients of the menu item
                {
                    bool ingredientFound = false;
                    foreach (KitchenObjectSO plateKitchenObjSO in plateKitchenObj.GetKitchenObjSOList())    // Going through the ingredients of the plate
                    {
                        if (plateKitchenObjSO == recipeKitchenObjSO)   // Found a match
                        {
                            ingredientFound = true; 
                            break;
                        }
                    }
                    if (!ingredientFound)   // This recipe ingredient was not found on the plate
                    {
                        plateContentsMatch = false;
                        return;
                    }
                }
                if (plateContentsMatch) // Delivered the correct recipe
                {
                    Debug.Log("Recipe Delivered!");
                    waitingRecipeSOList.RemoveAt(i);
                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);    // Invoking the event   
                    return;
                }
            }
        }
        // If we reach here, the plate contents did not match any of the waiting recipes
    }

    public List<MenuSO> GetWaitingRecipeSOList()
    {
        return waitingRecipeSOList;
    }
}
