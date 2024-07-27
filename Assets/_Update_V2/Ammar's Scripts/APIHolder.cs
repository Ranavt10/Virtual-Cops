using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class APIHolder : MonoBehaviour
{
    public SoundManager soundManager;
    private static readonly string baseUrl = /*"https://virtualcoop.vrcscan.com/api/";*/"https://virtualcoop713.vrcscan.com/api/";

    public bool isFreeRoom;
    private static int instanceCount = 0;
    public int indexToBeSpawnedOn;

    private void Awake()
    {
        instanceCount++;

        if (instanceCount > 1)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        instanceCount--;
    }

    public static string getBaseUrl()
    {
        return baseUrl;
    }
}


#if UNITY_EDITOR
public class ScriptableObjectSearch : EditorWindow
{
    private string searchString = "";

    [MenuItem("Window/Scriptable Object Search")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ScriptableObjectSearch));
    }

    void OnGUI()
    {
        GUILayout.Label("Search for Scriptable Objects", EditorStyles.boldLabel);
        searchString = EditorGUILayout.TextField("Search", searchString);

        if (GUILayout.Button("Search"))
        {
            SearchForScriptableObjects(searchString);
        }
    }

    void SearchForScriptableObjects(string searchQuery)
    {
        string[] assetGuids = AssetDatabase.FindAssets("t:ScriptableObject");
        foreach (string assetGuid in assetGuids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(assetGuid);
            ScriptableObject scriptableObject = AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPath);
            if (scriptableObject != null && assetPath.ToLower().Contains(searchQuery.ToLower()))
            {
                Debug.Log("Found Scriptable Object: " + assetPath);
            }
        }
    }
}
#endif
