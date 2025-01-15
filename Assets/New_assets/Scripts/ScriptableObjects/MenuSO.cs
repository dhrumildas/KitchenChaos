using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class MenuSO : ScriptableObject
{
    public List<KitchenObjectSO> menuItemSOList;
    public string menuName;
}
