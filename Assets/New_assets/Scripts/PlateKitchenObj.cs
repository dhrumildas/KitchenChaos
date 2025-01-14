using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObj : KitchenObject
{
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectSO kitchenObjectSO;
    }

    //Here we want the plate to behave like a KitchenObject with a tiny bit of extra logic so inheritance makes sense
    [SerializeField] private List<KitchenObjectSO> validKitchenObjSOList;

    private List<KitchenObjectSO> kitchenObjSOList;
    
    
    private void Awake()
    {
        kitchenObjSOList = new List<KitchenObjectSO>();
    }
    public bool TryAddIngredient(KitchenObjectSO kitchenObjSO)
    {
        if(!validKitchenObjSOList.Contains(kitchenObjSO))   //Not a valid ingredient
        {
            return false;
        }
        if (kitchenObjSOList.Contains(kitchenObjSO))
        {
            //Avoiding same ingredients, i.e., at most 1 ingredient at a time no fancy burgers
            return false;
        }
        else
        {
            kitchenObjSOList.Add(kitchenObjSO);
            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs
            {
                kitchenObjectSO = kitchenObjSO
            });
            return true;
        }
    }

    public List<KitchenObjectSO> GetKitchenObjSOList()
    {
        return kitchenObjSOList;
    }
}