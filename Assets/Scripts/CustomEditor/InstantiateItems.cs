using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateItems : MonoBehaviour
{
    [SerializeField]
    InventoryData inventoryData;

    private void Awake()
    {
        foreach (InventoryItemData item in inventoryData.items)
        {
            InventoryItem itemInstance = Instantiate(item.prefab);
            itemInstance.SetData(item);
            itemInstance.transform.position += Vector3.one * Random.Range(-10f, 10f);
        }
    }
}
