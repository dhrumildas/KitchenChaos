using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    public event EventHandler OnPlayerGrabObj;

    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    
    public override void Interact(Player player)
    {
        if(!player.HasKitchenObj())
        {
            KitchenObject.SpawnKitchenObj(kitchenObjectSO,player);
            OnPlayerGrabObj?.Invoke(this, EventArgs.Empty);
        }
        
    }
}
