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
                player.GetKitchenObject().DestroyTheObj();  //Bye bye plate
            }
        }
    }
}
