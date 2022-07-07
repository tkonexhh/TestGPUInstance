using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPUInstanceTester : MonoBehaviour
{
    public GameObject[] Prefabs;
    public GameObject TestItemPrefab;
    private GPUInstanceTestItem[] m_TestItems;


    private int typeCount = 1;
    private int showCount = 1022;
    private bool isInstance = false;


    void Start()
    {
        //初始化坐标信息
        if (Prefabs == null)
            return;

        m_TestItems = new GPUInstanceTestItem[Prefabs.Length];

        for (int i = 0; i < Prefabs.Length; i++)
        {
            var go = GameObject.Instantiate(TestItemPrefab);
            go.name = Prefabs[i].name;
            go.transform.position = Vector3.zero;
            go.transform.rotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;
            GPUInstanceTestItem testItem = go.GetComponent<GPUInstanceTestItem>();//new GPUInstanceTestItem(Prefabs[i]);
            testItem.Init(Prefabs[i]);
            m_TestItems[i] = testItem;
        }

        for (int i = 0; i < m_TestItems.Length; i++)
        {
            m_TestItems[i].StartTest();
        }

        TestItemPrefab.SetActive(false);

        typeCount = m_TestItems.Length;
    }


    // private int 
    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(50, 50, 300, 400));

        int newTypeCount = (int)GUILayout.HorizontalSlider(typeCount, 0, Prefabs.Length);
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

        int newShowCount = (int)GUILayout.HorizontalSlider(showCount, 0, 1022);
        if (newShowCount != showCount)
        {
            showCount = newShowCount;


            for (int i = 0; i < m_TestItems.Length; i++)
            {
                m_TestItems[i].ShowCount(showCount);///(isInstance ? ShowMode.Instance : ShowMode.Normal);
            }

        }
        GUILayout.Label("ShowCount:" + showCount);
        GUILayout.Space(15);


        bool newIsInstance = GUILayout.Toggle(isInstance, "isInstance");
        if (newIsInstance != isInstance)
        {
            isInstance = newIsInstance;

            for (int i = 0; i < m_TestItems.Length; i++)
            {
                m_TestItems[i].SetShowMode(isInstance ? ShowMode.Instance : ShowMode.Normal);
            }
        }

        GUILayout.EndArea();
    }

    private class GUIStyles
    {
        public static GUIStyle showCount = new GUIStyle("ShowCount");
    }
}
