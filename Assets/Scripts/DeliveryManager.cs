using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    //2 eventa za menjanje recepata na ekranu
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    //2 recepta za sound efect kod servisiranja - dobro i lose
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;

    //pravimo da je singleton
    public static DeliveryManager Instance
    {
        get;
        private set;
    }

    [SerializeField] private RecipeListSO recipeListSO;
    private List<RecipeSO> waitingRecipeSOList;

    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f; //4sekunde
    private int waitingRecipesMax =4;
    private int successfulRecipesAmount;
    private void Awake()
    {
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>();
    }
    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer <= 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax; //resetujemo timer

            //bitno da proverimo da li je igra u state gameplaying da onda tek krenemo da spawnamo porudzbine
            if (KitchenGameManager.Instance.IsGamePlaying() && waitingRecipeSOList.Count < waitingRecipesMax) //4 recepta max u listi cekanja
            {
                //uzimamo iz liste recepeta random recept, indeks od 0 do velicine liste
                RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];
                Debug.Log(waitingRecipeSO.recipeName);
                waitingRecipeSOList.Add(waitingRecipeSO);


                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);

            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for(int i =0; i<waitingRecipeSOList.Count; i++)
        {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];

            if(waitingRecipeSO.kitchenObjectList.Count == plateKitchenObject.GetKitchenObjectList().Count)
            {
                // imaju isti broj elemenata tj sastojaka
                bool podudarajuSe = true;
                foreach(KitchenObject ko in  waitingRecipeSO.kitchenObjectList) {
                    //prolazimo kroz elemente u receptu
                    bool nadjenSastojak=false;
                    foreach (KitchenObject ko2 in plateKitchenObject.GetKitchenObjectList())
                    {
                        //prolazimo kroz elemente na tanjiru
                        if(ko == ko2)
                        {
                            nadjenSastojak = true;
                            break;
                        }
                       

                    }
                    if (!nadjenSastojak)
                    {
                        podudarajuSe=false;
                        //nije nadjen neki sastojak recepta
                    }
                }

                if (podudarajuSe)
                {
                    //igrac je servisirao tacan recept
                    Debug.Log("Igrac je servisirao tacan recept");
                    //brisemo recept iz liste cekanja
                    waitingRecipeSOList.RemoveAt(i);

                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
                    successfulRecipesAmount++;

                    return;
                }
            }
        }
        //igrac nije servisirao tacan recept
        Debug.Log("Igrac nije servisirao tacan recept");
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);


    }
    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return waitingRecipeSOList;
    }

    public int GetSuccessfulRecipesAmount()
    {
        return successfulRecipesAmount;
    }
}
