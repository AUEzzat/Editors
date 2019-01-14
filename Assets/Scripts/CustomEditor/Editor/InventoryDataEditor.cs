using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[Flags]
public enum MouseDetection
{
    DetectDrag = 1,
    DetectHover = 2
}

[CustomEditor(typeof(InventoryData))]
public class InventoryDataEditor : Editor
{
    InventoryData inventoryData;
    int listLenth;
    bool showList = true;
    int selectedItem;
    int hoveredItem;
    Vector2 scrollPos;

    private void OnEnable()
    {
        inventoryData = (InventoryData)target;
        listLenth = inventoryData.items.Count;
        selectedItem = -1;
        hoveredItem = -1;
    }

    public override void OnInspectorGUI()
    {
        GUILayout.Space(10);

        Rect listLabelRect = EditorGUILayout.BeginHorizontal();

        DetectMouseDrag(listLabelRect, draggedObj =>
        {
            inventoryData.items.Add(new InventoryItemData(((GameObject)draggedObj).GetComponent<InventoryItem>()));
            listLenth++;
        });

        showList = EditorGUILayout.Foldout(showList, "Items");

        listLenth = EditorGUILayout.IntField(listLenth);

        EditorGUILayout.EndHorizontal();

        if (listLenth < 0)
            listLenth = 0;


        while (listLenth > inventoryData.items.Count)
        {
            if (inventoryData.items.Count > 0)
                inventoryData.items.Add(new InventoryItemData(inventoryData.items[inventoryData.items.Count - 1]));
            else
                inventoryData.items.Add(null);
        }

        while (listLenth < inventoryData.items.Count)
        {
            inventoryData.items.RemoveAt(inventoryData.items.Count - 1);
        }

        if (selectedItem > inventoryData.items.Count - 1)
            selectedItem = inventoryData.items.Count - 1;

        if (hoveredItem > inventoryData.items.Count - 1)
            hoveredItem = inventoryData.items.Count - 1;

        GUILayout.Space(10);

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        if (showList)
            for (int i = 0; i < listLenth; i++)
            {
                DisplayInventoryItem(i);
            }

        EditorGUILayout.EndScrollView();

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.BeginVertical();
        DisplayInventoryItem(selectedItem, 0);

        EditorGUI.BeginDisabledGroup(selectedItem < 0);

        EditorGUILayout.BeginHorizontal();
        if (selectedItem > -1)
        {
            inventoryData.items[selectedItem].scale = EditorGUILayout.FloatField("Scale", inventoryData.items[selectedItem].scale);
            inventoryData.items[selectedItem].color = EditorGUILayout.ColorField("Color", inventoryData.items[selectedItem].color);
        }
        else
        {
            EditorGUILayout.FloatField("Scale", 1.0f);
            EditorGUILayout.ColorField("Color", Color.black);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUI.EndDisabledGroup();


        EditorGUILayout.EndVertical();

        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(inventoryData);
    }

    void DisplayInventoryItem(int index,
        MouseDetection mouseDetection = MouseDetection.DetectHover | MouseDetection.DetectDrag)
    {
        Color backgroundColor = GUI.backgroundColor;

        ItemColor(index);

        GUIStyle style = new GUIStyle(EditorStyles.helpBox);

        RectOffset rect = new RectOffset(0, 0, 10, 10);

        style.padding = rect;

        Rect itemRect = EditorGUILayout.BeginHorizontal(style);

        InventoryItemData inventoryItem = null;

        if (index > -1)
            inventoryItem = inventoryData.items[index];

        Texture image = new Texture2D(0, 0);
        string name = "None";
        if (inventoryItem != null)
        {
            image = AssetPreview.GetAssetPreview(inventoryItem.prefab.gameObject);
            name = inventoryItem.prefab.name;
        }

        if ((mouseDetection & MouseDetection.DetectDrag) == MouseDetection.DetectDrag)
            DetectMouseDrag(itemRect, draggedObj =>
            {
                inventoryData.items[index] = new InventoryItemData(((GameObject)draggedObj).GetComponent<InventoryItem>());
            });

        if ((mouseDetection & MouseDetection.DetectHover) == MouseDetection.DetectHover)
            DetectMouseHover(itemRect, index);

        GUIStyle imageStyle = new GUIStyle(EditorStyles.helpBox)
        {
            fixedHeight = 70,
        };

        float labelWidth = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = 20f;

        EditorGUILayout.LabelField(index.ToString() + ".");

        EditorGUIUtility.labelWidth = labelWidth;

        GUIContent itemLabel = new GUIContent(name, image);

        EditorGUILayout.LabelField(itemLabel, imageStyle);

        EditorGUILayout.EndHorizontal();

        GUI.backgroundColor = backgroundColor;
    }

    void ItemColor(int index)
    {
        if (index == selectedItem)
            GUI.backgroundColor = Color.green;
        else if (index == hoveredItem)
            GUI.backgroundColor = Color.gray;
    }

    void DetectMouseDrag(Rect itemRect, Action<UnityEngine.Object> actionOnItem)
    {
        if (itemRect.Contains(Event.current.mousePosition))
        {
            switch (Event.current.type)
            {
                case EventType.DragUpdated:
                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                    Event.current.Use();
                    break;
                case EventType.DragPerform:
                    for (int i = 0; i < DragAndDrop.objectReferences.Length; i++)
                    {
                        actionOnItem(DragAndDrop.objectReferences[i]);
                    }
                    Event.current.Use();
                    break;
            }
        }
    }

    void DetectMouseHover(Rect itemRect, int index)
    {
        if (itemRect.Contains(Event.current.mousePosition))
        {
            switch (Event.current.type)
            {
                case EventType.MouseDown:
                    selectedItem = index;
                    break;
                default:
                    hoveredItem = index;
                    break;
            }
        }
    }
}
