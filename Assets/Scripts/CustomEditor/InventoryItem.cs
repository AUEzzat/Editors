using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryItem : MonoBehaviour
{
    [SerializeField]
    List<MeshRenderer> meshRenderers;

    public void SetData(float scale, Color color)
    {
        transform.localScale = Vector3.one * scale;
        foreach(MeshRenderer meshRenderer in meshRenderers)
        {
            meshRenderer.material.SetColor("_Color", color);
        }
    }

    public void SetData(InventoryItemData inventoryItemData)
    {
        SetData(inventoryItemData.scale, inventoryItemData.color);
    }
}
