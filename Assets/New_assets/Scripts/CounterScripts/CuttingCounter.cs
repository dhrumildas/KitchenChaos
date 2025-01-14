using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public event EventHandler OnCut;

    [SerializeField] private RecipeSO[] cuttingRecipeSOArray;

    private int cuttingCount;
    public override void Interact(Player player)
    {
        if (!HasKitchenObj())
        {
            //No kitchen obj present
            if (player.HasKitchenObj())
            {
                //player is carrying something
                if(HasRecipe(player.GetKitchenObject().GetKitchenObjectSO()))
                { 
                    //The Item can be cut
                    player.GetKitchenObject().SetKitchenObjParent(this);
                    cuttingCount = 0;
                    RecipeSO recipeSO = GetCuttingRecipeSOInput(GetKitchenObject().GetKitchenObjectSO());
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = (float)cuttingCount/recipeSO.cuttingProgressMax
                    });
                }
            }
            else
            {
                //Player is empty handed
            }
        }
        else
        {
            //kitchen obj present
            if (player.HasKitchenObj())
            {
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObj plateKitchenObj))
                {
                    //Player is holding a plate
                    if (plateKitchenObj.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroyTheObj();
                    }
                }
            }
            else
            {
                //Player is empty handed
                GetKitchenObject().SetKitchenObjParent(player);
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = 0f
                });
            }
        }
    }

    public override void InteractAlt(Player player)
    {
        if (HasKitchenObj() && HasRecipe(GetKitchenObject().GetKitchenObjectSO())) {
            cuttingCount++;
            OnCut?.Invoke(this, EventArgs.Empty);
            RecipeSO recipeSO = GetCuttingRecipeSOInput(GetKitchenObject().GetKitchenObjectSO());
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = (float)cuttingCount / recipeSO.cuttingProgressMax
            });
            if (cuttingCount >= recipeSO.cuttingProgressMax)
            {
                //there is a kitchenObj AND it can be cut
                KitchenObjectSO opKitchenObjSO = GetOPforIP(GetKitchenObject().GetKitchenObjectSO());
                //Cut the obj
                GetKitchenObject().DestroyTheObj();
                KitchenObject.SpawnKitchenObj(opKitchenObjSO, this);
            }
            
        }
    }

    private bool HasRecipe(KitchenObjectSO kitchenObjSO)
    {
        RecipeSO recipeSO = GetCuttingRecipeSOInput(kitchenObjSO);
        return recipeSO != null;
    }

    private KitchenObjectSO GetOPforIP(KitchenObjectSO objSO)
    {
        RecipeSO recipeSO = GetCuttingRecipeSOInput(objSO);
        if (recipeSO != null)
            return recipeSO.output;
        else
            return null;
    }

    private RecipeSO GetCuttingRecipeSOInput(KitchenObjectSO ipKitchenSO)
    {
        foreach (RecipeSO recipeSO in cuttingRecipeSOArray)
        {
            if (recipeSO.input == ipKitchenSO)
            {
                return recipeSO;
            }
        }
        return null;
    }
}
