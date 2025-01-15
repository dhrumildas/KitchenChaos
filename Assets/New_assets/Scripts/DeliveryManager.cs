using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    [SerializeField] private RecipeListSO recipeListSO;
    private List<MenuSO> waitingRecipeSOList;
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int maxWaitingRecipeCount = 5;

    private void Awake()
    {
        waitingRecipeSOList = new List<MenuSO>();
    }

    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer <= 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;

            if (waitingRecipeSOList.Count < maxWaitingRecipeCount)
            {

                MenuSO waitingRecipeSO = recipeListSO.recipeSOList[Random.Range(0, recipeListSO.recipeSOList.Count)];
                Debug.Log("Waiting Recipe: " + waitingRecipeSO.menuName);
                waitingRecipeSOList.Add(waitingRecipeSO);
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObj plateKitchenObj)  // to see if the contents of the plate match the recipe
    {
        for(int i =0; i < waitingRecipeSOList.Count; i++)   // Going through the waiting menu items
        {
            MenuSO waitingRecipeSO = waitingRecipeSOList[i];    // Getting the waiting menu item

        }
    }
}
