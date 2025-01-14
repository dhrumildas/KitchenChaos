using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjSO;
    private IKitchenObjParent kitchenObjParent;

    public KitchenObjectSO GetKitchenObjectSO() { return kitchenObjSO; }

    public void SetKitchenObjParent(IKitchenObjParent kitchenObjParent)
    {
        if(this.kitchenObjParent != null)
        {
            this.kitchenObjParent.ClearKitchenObj();
        }
        this.kitchenObjParent = kitchenObjParent;

        if (kitchenObjParent.HasKitchenObj())
        {
            Debug.LogError("IKitchenObjParent Occupied");
        }

        kitchenObjParent.SetKitchenObj(this);
        transform.parent = kitchenObjParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }
    public IKitchenObjParent GetClearCounter()
    {
        return kitchenObjParent;
    }
    public void DestroyTheObj()
    {
        kitchenObjParent.ClearKitchenObj() ;
        Destroy(gameObject);
    }

    public static KitchenObject SpawnKitchenObj(KitchenObjectSO kitchenSO, IKitchenObjParent parent)
    {
        Transform kitchenObjectTransform = Instantiate(kitchenSO.prefab);
        KitchenObject kitchenObj = kitchenObjectTransform.GetComponent<KitchenObject>();
        kitchenObj.SetKitchenObjParent(parent);
        return kitchenObj;
    }

    public bool TryGetPlate(out PlateKitchenObj plateKitchenObj)
    {
        if(this is PlateKitchenObj)
        {
            plateKitchenObj = this as PlateKitchenObj;
            return true;
        }
        else
        {
            plateKitchenObj=null;   // the out parameter has to be set to "something"
            return false;           // hence the error if pKO wasn't set to null
        }
    }
}
