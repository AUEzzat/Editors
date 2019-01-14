using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

enum Axis
{
    X,
    Y,
    Z
}

public class RotateEditorWindow : EditorWindow
{

    private GameObject[] selectedObjects;
    private string[] options = new string[] { "X", "Y", "Z" };
    private int selectedIndex;
    private int rotationAngle;

    [MenuItem("Window/Rotate Objects")]
    private static void Init()
    {
        RotateEditorWindow window = (RotateEditorWindow)GetWindow(typeof(RotateEditorWindow));
        window.Show();
    }

    void OnGUI()
    {
        selectedIndex = EditorGUILayout.Popup("Axis", selectedIndex, options);
        rotationAngle = EditorGUILayout.IntField("RotationAngle", rotationAngle);

        EditorGUILayout.Space();

        if (GUILayout.Button("RotateObjects"))
        {
            switch (selectedIndex)
            {
                case 0:
                    RotateObjects(gameObject => gameObject.transform.Rotate(Vector3.right, rotationAngle, Space.World));
                    break;
                case 1:
                    RotateObjects(gameObject => gameObject.transform.Rotate(Vector3.up, rotationAngle, Space.World));
                    break;
                case 2:
                    RotateObjects(gameObject => gameObject.transform.Rotate(Vector3.forward, rotationAngle, Space.World));
                    break;
            }
        }
    }

    void OnSelectionChange()
    {
        selectedObjects = Selection.gameObjects;
    }

    void RotateObjects(System.Action<GameObject> transformAction)
    {
        if (selectedObjects == null)
        {
            Debug.Log("Select an object");
            return;
        }

        foreach (GameObject gameObject in selectedObjects)
        {
            transformAction(gameObject);
        }
    }

}
