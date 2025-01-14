using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{

    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    
    public override void Interact(Player player)
    {
        if(!HasKitchenObj())
        {
            //No kitchen obj present
            if(player.HasKitchenObj())
            {
                //player is carrying something
                player.GetKitchenObject().SetKitchenObjParent(this);
            }
            else
            {
                //Player is empty handed
            }
        }
        else
        {
            //kitchen obj present
            if (player.HasKitchenObj()) { 
                //Player is carrying something
                if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObj plateKitchenObj) )
                {
                    //Player is holding a plate
                    if(plateKitchenObj.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroyTheObj();
                    }
                }
                //if the player is not holding a plate
                else
                {
                    if(GetKitchenObject().TryGetPlate(out plateKitchenObj)) // same new variable as above
                    {
                        // there is a plate on top of the counter
                        if(plateKitchenObj.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            player.GetKitchenObject().DestroyTheObj();
                        }
                    }
                }
            }
            else
            {
                //Player is empty handed
                GetKitchenObject().SetKitchenObjParent(player);
            }
        }
    }
}
