using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu()]   commented this after creating the UltimateRecipeListSO so that no other variant can be created
public class MainListSO : ScriptableObject
{
    public List<RecipeListSO> mainRecipeSOList;
}
