using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItemData
{
    public InventoryItem prefab;

    public Color color;

    public float scale = 1;

    public InventoryItemData(InventoryItem prefab)
    {
        this.prefab = prefab;
    }

    public InventoryItemData(InventoryItemData item)
    {
        prefab = item.prefab;
        color = item.color;
        scale = item.scale;
    }
}
