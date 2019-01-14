using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Enemy))]
public class EnemyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight * 4;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Rect rect = new Rect(position.x, position.y, position.width, position.height / 4);
        EditorGUI.LabelField(position, "Enemy");

        label = EditorGUI.BeginProperty(position, label, property);

        EditorGUIUtility.labelWidth = 60f;
        rect = new Rect(position.x, position.y + position.height / 3, position.width / 4, position.height / 4);
        EditorGUI.PropertyField(rect, property.FindPropertyRelative("damage"));

        SerializedProperty serializedType = property.FindPropertyRelative("type");

        rect = new Rect(position.x + position.width / 3, position.y + position.height / 3, position.width / 4, position.height / 4);
        EditorGUI.PropertyField(rect, serializedType);

        if ((AttackType)serializedType.intValue != AttackType.Melee)
        {
            rect = new Rect(position.x + 2 * position.width / 3, position.y + position.height / 3, position.width / 4, position.height / 4);
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("range"));
        }

        SerializedProperty serializedName = property.FindPropertyRelative("enemyName");
        serializedName.stringValue = property.FindPropertyRelative("damage").intValue.NumberToWords() + ((AttackType)property.FindPropertyRelative("type").intValue).ToString();

        EditorGUIUtility.labelWidth = 100f;
        EditorGUI.BeginDisabledGroup(true);
        rect = new Rect(position.x, position.y + 2 * position.height / 3, position.width, position.height / 4);
        EditorGUI.PropertyField(rect, serializedName);
        EditorGUI.EndDisabledGroup();
        EditorGUI.EndProperty();
    }
}
