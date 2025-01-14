using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;
    [SerializeField] private KitchenObjectSO plateSO;
    private float spawnPlateTimer;
    private float spawnPlateTimerMax = 3.5f;
    private int platesSpawnedAmount = 0;
    private int platesSpawnedAmountMAX = 4;
    private void Update()
    {
        spawnPlateTimer += Time.deltaTime;
        if(spawnPlateTimer > spawnPlateTimerMax)
        {
            spawnPlateTimer = 0f;
            if(platesSpawnedAmount < platesSpawnedAmountMAX)
            {
                platesSpawnedAmount++;
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player)
    {
        if(!player.HasKitchenObj())
        {
            //Player is empty handed
            if(platesSpawnedAmount>0)
            {
                //there is atleast one
                platesSpawnedAmount--;
                KitchenObject.SpawnKitchenObj(plateSO, player);
                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
