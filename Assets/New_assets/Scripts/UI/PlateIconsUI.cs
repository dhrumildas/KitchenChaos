using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateIconsUI : MonoBehaviour
{
    [SerializeField] private PlateKitchenObj plateKitchenObj;
    [SerializeField] private Transform iconTemplate;

    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);   //So that the template isn't visible right from the beginning of the game
    }

    private void Start()
    {
        plateKitchenObj.OnIngredientAdded += PlateKitchenObj_OnIngredientAdded;
    }

    private void PlateKitchenObj_OnIngredientAdded(object sender, PlateKitchenObj.OnIngredientAddedEventArgs e)
    {
        UpdateVisual();
    }
    private void UpdateVisual()
    {
        foreach(Transform child in transform)
        {
            if (child == iconTemplate) continue;    //to avoid clearing iconTemplate itself
            Destroy(child.gameObject);  //to clear out the previous plate's Icon template
        }

        foreach (KitchenObjectSO kitchenObjectSO in plateKitchenObj.GetKitchenObjSOList()) {    //go through the list to display the visual
            Transform iconTransform = Instantiate(iconTemplate, transform);   //we used transform to make it a child of this object AND spawn it
            iconTransform.gameObject.SetActive(true);    //So that the instantiated objects are visible again
            //NOTE : this iconTransform is very different from the iconTemplate; the later clears the whole DAMN thing
            iconTransform.GetComponent<PlateIconSingleUI>().SetKitchenObjSO(kitchenObjectSO);
        }
    }
}
