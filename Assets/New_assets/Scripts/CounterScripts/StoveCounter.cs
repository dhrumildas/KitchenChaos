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

    //Changed private to public due to inconsistent accessibility
    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burnt
    }

    [SerializeField] private FryingRecipeSO[] fryingRecicpeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecicpeSOArray;

    private State state;
    private float fryingTimer;
    private float burningTimer;
    private FryingRecipeSO fryingRecipeSO;
    private BurningRecipeSO burningRecipeSO;

    private void Start()
    {
        state = State.Idle;
    }

    private void Update()
    {
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
                        progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMAX
                    });
                    if (fryingTimer > fryingRecipeSO.fryingTimerMAX)
                    {
                        // Fried

                        GetKitchenObject().DestroyTheObj();
                        KitchenObject.SpawnKitchenObj(fryingRecipeSO.output, this);
                        state = State.Fried;
                        burningTimer = 0f;
                        burningRecipeSO = GetBurningRecipeSOInput(GetKitchenObject().GetKitchenObjectSO());
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });
                    }
                    break;

                case State.Fried:
                    burningTimer += Time.deltaTime;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = burningTimer / burningRecipeSO.burningTimerMAX
                    });
                    if (burningTimer > burningRecipeSO.burningTimerMAX)
                    {
                        // Fried

                        GetKitchenObject().DestroyTheObj();
                        KitchenObject.SpawnKitchenObj(burningRecipeSO.output, this);
                        state = State.Burnt;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = 0f
                        });
                    }
                    break;

                case State.Burnt:
                    break;
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObj())
        {
            // No kitchen object present
            if (player.HasKitchenObj())
            {
                // Player is carrying something
                if (HasRecipe(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    // The item can be fried
                    player.GetKitchenObject().SetKitchenObjParent(this);

                    // When the player drops the meat
                    fryingRecipeSO = GetFryingRecipeSOInput(GetKitchenObject().GetKitchenObjectSO());
                    state = State.Frying;
                    fryingTimer = 0f;
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                    {
                        state = state
                    });
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = 0f
                    });
                }
            }
            else
            {
                // Player is empty-handed
            }
        }
        else
        {
            // Kitchen object present
            if (player.HasKitchenObj())
            {
                // Player is carrying something
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObj plateKitchenObj))
                {
                    //Player is holding a plate
                    if (plateKitchenObj.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroyTheObj();

                        state = State.Idle;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = 0f
                        });
                    }
                }
            }
            else
            {
                // Player is empty-handed
                GetKitchenObject().SetKitchenObjParent(player);
                state = State.Idle;
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                {
                    state = state
                });
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = 0f
                });
            }
        }
    }

    private bool HasRecipe(KitchenObjectSO kitchenObjSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOInput(kitchenObjSO);
        return fryingRecipeSO != null;
    }

    private KitchenObjectSO GetOPforIP(KitchenObjectSO objSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOInput(objSO);
        return fryingRecipeSO != null ? fryingRecipeSO.output : null;
    }

    private FryingRecipeSO GetFryingRecipeSOInput(KitchenObjectSO ipKitchenSO)
    {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecicpeSOArray)
        {
            if (fryingRecipeSO.input == ipKitchenSO)
            {
                return fryingRecipeSO;
            }
        }
        return null;
    }

    private BurningRecipeSO GetBurningRecipeSOInput(KitchenObjectSO ipKitchenSO)
    {
        foreach (BurningRecipeSO burningRecipeSO in burningRecicpeSOArray)
        {
            if (burningRecipeSO.input == ipKitchenSO)
            {
                return burningRecipeSO;
            }
        }
        return null;
    }
}
