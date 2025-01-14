using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
    [Serializable]  //Since it is a user defined class (basically) and won't be visible in the Editor
    public struct KitchenObjectSO_GameObject
    {
        public KitchenObjectSO kitchenObjectSO;
        public GameObject gameObject;

    }
    [SerializeField] private PlateKitchenObj plateKitchenObj;
    [SerializeField] private List<KitchenObjectSO_GameObject> kitchenList;
    private void Start()
    {
        plateKitchenObj.OnIngredientAdded += PlateKitchenObj_OnIngredientAdded;
        foreach (KitchenObjectSO_GameObject x in kitchenList)
        {
            x.gameObject.SetActive(false);
        }
    }

    private void PlateKitchenObj_OnIngredientAdded(object sender, PlateKitchenObj.OnIngredientAddedEventArgs e)
    {
        foreach (KitchenObjectSO_GameObject x in kitchenList)
        {
            if (x.kitchenObjectSO == e.kitchenObjectSO)
            {
                x.gameObject.SetActive(true);
            }
        }
    }
}
