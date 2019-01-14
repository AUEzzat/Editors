using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TransformsData", menuName = "Scriptables/TransformsData")]
public class InventoryData : ScriptableObject
{
    public List<InventoryItemData> items;
}
