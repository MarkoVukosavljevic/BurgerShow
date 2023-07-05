using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter
{

    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;
    [SerializeField] private KitchenObject plateKitchenObject;
    
    private float spawnPlateTimer;
    private float spawnPlateTimerMax = 4f;

    private int platesSpawnedAmount;
    private int platesSpawnedAmountMax = 4;
    private void Update()
    {
        spawnPlateTimer += Time.deltaTime;
        if (spawnPlateTimer > spawnPlateTimerMax)
        {

            spawnPlateTimer = 0f;
          //  KitchenObj.SpawnKitchenObject(plateKitchenObject, this);

            //spawnovacemo tanjir samo ako je igra u game play statu
            if(KitchenGameManager.Instance.IsGamePlaying() && platesSpawnedAmount <platesSpawnedAmountMax)
            {
                platesSpawnedAmount++;
                OnPlateSpawned?.Invoke(this,  EventArgs.Empty);
            }
        }
    }


    public override void Interact(Player player)
    {
        if (!player.HasKitchenObj())
        {
            if (platesSpawnedAmount > 0)
            {
                //ako ima spawnovanih visual tanjira mozemo da uzmemo pravi tanjir
                platesSpawnedAmount--;

                KitchenObj.SpawnKitchenObject(plateKitchenObject, player);
                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
        }


    }

}
