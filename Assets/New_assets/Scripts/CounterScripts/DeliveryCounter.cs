using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    public override void Interact(Player player)
    {
        if(player.HasKitchenObj())  //checking if the playe is holding something
        {
            if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObj plateKitchenObj))  //Tries to see if the kitchenObj in player's hands is a plate
            {
                DeliveryManager.Instance.DeliverRecipe(plateKitchenObj);  //Deliver the plate to the DeliveryManager
                player.GetKitchenObject().DestroyTheObj();  //Bye bye plate
            }
        }
    }
}
