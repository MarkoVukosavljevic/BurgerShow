using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }
    public enum State
    {
        Idle, //nema nista
        Frying, // meso se przi
        Fried, //isprzilo se
        Burned, // skroz izgorelo
    }
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

    private State state;
    private float fryingTimer;
    private FryingRecipeSO fryingRecipeSO;
    private float burningTimer;
    private BurningRecipeSO burningRecipeSO;





    private void Start()
    {
        //na pocetku stanje je idle
        state = State.Idle;

        //ispaljujem event kad god se promeni state i to saljem u stovecountervisual
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
        {
            state = state
        });

    }
    private void Update()
    {

        //logika przenja koristicemo enum za stanja
        if (HasKitchenObj())
        {
            switch (state)
        {
            case State.Idle:
                break;
            case State.Frying:
                fryingTimer += Time.deltaTime;

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
                }) ;

                if (fryingTimer > fryingRecipeSO.fryingTimerMax)
                {
                    GetKitchenObj().DestroySelf();
                    KitchenObj.SpawnKitchenObject(fryingRecipeSO.output, this);
                        //kada se isprzilo krece stanje fried
                        state = State.Fried;
                        burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObj().GetKitchenObject());
                        burningTimer = 0f;

                        //ispaljujem event kad god se promeni state i to saljem u stovecountervisual
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        }) ;
                        


                }
                break;
            case State.Fried:
                    burningTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = burningTimer / burningRecipeSO.burningTimerMax
                    });


                    if (burningTimer > burningRecipeSO.burningTimerMax)
                    {
                        GetKitchenObj().DestroySelf();
                        KitchenObj.SpawnKitchenObject(burningRecipeSO.output, this);
                        //kada se meso skroz zagorelo krece stanje burned
                        state = State.Burned;

                        //ispaljujem event kad god se promeni state i to saljem u stovecountervisual
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });

                        //ovaj event je samo da bi bar nestao nakon ugljenisanja mesa
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = 0f
                        }) ;
                    }
                    break;
           
            case State.Burned:
                break;
           
        }
             }
    }
    public override void Interact(Player player)
    {

        if (!HasKitchenObj())
        {
            if (player.HasKitchenObj() && HasRecipeWithInput(player.GetKitchenObj().GetKitchenObject()))
            {

                //ubacicemo proveru da li uopste ta hrana moze da se pece, hleb ne bi trebalo seci

                player.GetKitchenObj().SetKitchenObjectParent(this);

                 fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObj().GetKitchenObject());

             //kada igrac baci objekat na stove stanje se menja u frying
             state = State.Frying;
                fryingTimer = 0f;

                //ispaljujem event kad god se promeni state i to saljem u stovecountervisual
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                {
                    state = state
                });

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
                }) ;
            }
        }
        else
        {
            //nesto se nalazi na counteru
            if (player.HasKitchenObj()) {
                //igrac drzi nesto
                if (player.GetKitchenObj().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                { // drzi tanjir igrac

                    if (plateKitchenObject.TryAddIngridient(GetKitchenObj().GetKitchenObject()))
                    {

                        GetKitchenObj().DestroySelf();
                    }
                }
                //posto smo uzeli meso sa tanjirom moramo oper state da podesimo

                state = State.Idle;

                //ispaljujem event kad god se promeni state i to saljem u stovecountervisual
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                {
                    state = state
                });

                //ovaj event je samo da bi bar nestao nakon sto igrac pokupi meso pre zavrsetka
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = 0f
                });
            }
            
            else{
                //igrac ne nosi nista
                this.GetKitchenObj().SetKitchenObjectParent(player);
                //kada igrac uzme meso state je opet idle
                state = State.Idle;

                //ispaljujem event kad god se promeni state i to saljem u stovecountervisual
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                {
                    state = state
                });

                //ovaj event je samo da bi bar nestao nakon sto igrac pokupi meso pre zavrsetka
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = 0f
                });
            }
        }





    }
    private bool HasRecipeWithInput(KitchenObject inputKitchenObject)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObject);
        if (fryingRecipeSO != null)
        {
            return true;
        }
        //ovo je provera ako stavim hleb na secenje da ne moze, nema ga u receptima

        return false;
    }
    public KitchenObject GetOutputForInput(KitchenObject inputKitchenObject)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObject);
        if (fryingRecipeSO != null)
        {
            return fryingRecipeSO.output;
        }

        return null;
    }

    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObject inputKitchenObject)
    {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        {
            if (fryingRecipeSO.input == inputKitchenObject)
            {
                return fryingRecipeSO;
            }
        }
        return null;
    }


    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObject inputKitchenObject)
    {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
        {
            if (burningRecipeSO.input == inputKitchenObject)
            {
                return burningRecipeSO;
            }
        }
        return null;
    }

    public bool IsFried()
    {
        return state == State.Fried;
    }
}
