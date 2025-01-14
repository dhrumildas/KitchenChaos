using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKitchenObjParent
{
    public Transform GetKitchenObjectFollowTransform();
    public void SetKitchenObj(KitchenObject kitchenObject);
    public KitchenObject GetKitchenObject();
    public void ClearKitchenObj();
    public bool HasKitchenObj();
}
