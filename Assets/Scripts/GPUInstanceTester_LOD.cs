using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPUInstanceTester_LOD : MonoBehaviour
{
    public Mesh mesh0;
    public Mesh mesh1;
    public Mesh mesh2;
    public Material material;
    public int totalCount;
    public GameObject TestItemPrefab;
    private GPUInstanceTestItem_LOD[] m_TestItems;

    private int typeCount = 1;
    private bool isSplitLOD = false;


    void Start()
    {
        GameObject autoRoot = new GameObject();
        GameObject[] Prefabs = new GameObject[totalCount * 3];
        for (int i = 0; i < totalCount; ++i)
        {
            // Debug.LogError(i + "----" + (i * 3) + "---" + (i * 3 + 1) + "---" + (i * 3 + 2));
            var mat = Material.Instantiate(material);
            mat.enableInstancing = true;

            GameObject obj0 = new GameObject("Auto" + i + "0");
            obj0.AddComponent<MeshFilter>().mesh = Mesh.Instantiate(mesh0);
            obj0.AddComponent<MeshRenderer>().material = mat;
            obj0.transform.SetParent(autoRoot.transform);
            Prefabs[i * 3] = obj0;

            GameObject obj1 = new GameObject("Auto" + i + "1");
            obj1.AddComponent<MeshFilter>().mesh = Mesh.Instantiate(mesh1);
            obj1.AddComponent<MeshRenderer>().material = mat;
            obj1.transform.SetParent(autoRoot.transform);
            Prefabs[i * 3 + 1] = obj1;

            GameObject obj2 = new GameObject("Auto" + i + "2");
            obj2.AddComponent<MeshFilter>().mesh = Mesh.Instantiate(mesh2);
            obj2.AddComponent<MeshRenderer>().material = mat;
            obj2.transform.SetParent(autoRoot.transform);
            Prefabs[i * 3 + 2] = obj2;
        }

        m_TestItems = new GPUInstanceTestItem_LOD[totalCount];

        for (int i = 0; i < totalCount; i++)
        {
            var go = GameObject.Instantiate(TestItemPrefab);
            go.name = Prefabs[i * 3].name;
            go.transform.position = Vector3.zero;
            go.transform.rotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;
            GPUInstanceTestItem_LOD testItem = go.GetComponent<GPUInstanceTestItem_LOD>();
            testItem.Init(Prefabs[i * 3], Prefabs[i * 3 + 1], Prefabs[i * 3 + 2]);
            m_TestItems[i] = testItem;
        }

        for (int i = 0; i < m_TestItems.Length; i++)
        {
            m_TestItems[i].StartTest();
        }

        TestItemPrefab.SetActive(false);

        typeCount = m_TestItems.Length;
        autoRoot.SetActive(false);
    }

    // private int 
    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(50, 50, 300, 400));
        GUILayout.Label("LOD Test:");
        int newTypeCount = (int)GUILayout.HorizontalSlider(typeCount, 0, totalCount);
        if (newTypeCount != typeCount)
        {
            typeCount = newTypeCount;

            for (int i = 0; i < m_TestItems.Length; i++)
            {
                m_TestItems[i].Enable(i < typeCount);///(isInstance ? ShowMode.Instance : ShowMode.Normal);
            }
        }

        GUILayout.Label("typeCount:" + typeCount);
        GUILayout.Space(15);

        // int newShowCount = (int)GUILayout.HorizontalSlider(showCount, 0, 1022);
        // if (newShowCount != showCount)
        // {
        //     showCount = newShowCount;


        //     for (int i = 0; i < m_TestItems.Length; i++)
        //     {
        //         m_TestItems[i].ShowCount(showCount);///(isInstance ? ShowMode.Instance : ShowMode.Normal);
        //     }

        // }
        // GUILayout.Label("ShowCount:" + showCount);
        // GUILayout.Space(15);


        bool newSplitLOD = GUILayout.Toggle(isSplitLOD, "isSpltLOD");
        if (newSplitLOD != isSplitLOD)
        {
            isSplitLOD = newSplitLOD;

            for (int i = 0; i < m_TestItems.Length; i++)
            {
                m_TestItems[i].SetLODMode(isSplitLOD ? LODMode.SpltLOD : LODMode.AllLOD0);
            }
        }

        GUILayout.EndArea();
    }

    private class GUIStyles
    {
        public static GUIStyle showCount = new GUIStyle("ShowCount");
    }
}
