using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjParent
{
    
    [SerializeField] private Transform counterTopPoint;
    private KitchenObject kitchenObject;
    public virtual void Interact(Player player)
    {
        Debug.LogError("BaseCounter.Interact();");
    }

    public virtual void InteractAlt(Player player)
    {
        //Debug.LogError("BaseCounter.InteractAlt();");
    }

    public Transform GetKitchenObjectFollowTransform() { return counterTopPoint; }
    public void SetKitchenObj(KitchenObject kitchenObject) { this.kitchenObject = kitchenObject; }
    public KitchenObject GetKitchenObject() { return kitchenObject; }
    public void ClearKitchenObj() { kitchenObject = null; }
    public bool HasKitchenObj() { return kitchenObject != null; }
}
