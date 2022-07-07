using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public enum LODMode
{
    AllLOD0,
    SpltLOD
}

public class GPUInstanceTestItem_LOD : MonoBehaviour
{
    private GameObject m_InstancePrefab0;
    private GameObject m_InstancePrefab1;
    private GameObject m_InstancePrefab2;
    [SerializeField] private Mesh m_Mesh0;
    [SerializeField] private Mesh m_Mesh1;
    [SerializeField] private Mesh m_Mesh2;
    [SerializeField] private Material m_Material;

    private const int MAX_COUNT_0 = 20;
    private const int MAX_COUNT_1 = 30;
    private const int MAX_COUNT_2 = 50;

    private Matrix4x4[] m_StoreMatrix_Total = new Matrix4x4[MAX_COUNT_0 + MAX_COUNT_1 + MAX_COUNT_2];
    private Matrix4x4[] m_StoreMatrix0 = new Matrix4x4[MAX_COUNT_0];
    private Matrix4x4[] m_StoreMatrix1 = new Matrix4x4[MAX_COUNT_1];
    private Matrix4x4[] m_StoreMatrix2 = new Matrix4x4[MAX_COUNT_2];


    // private List<GameObject> m_NormalGOs = new List<GameObject>();

    private LODMode m_LODMode = LODMode.AllLOD0;
    // private GameObject m_Root;


    public void Init(GameObject prefab0, GameObject prefab1, GameObject prefab2)
    {
        m_InstancePrefab0 = prefab0;
        m_InstancePrefab1 = prefab1;
        m_InstancePrefab2 = prefab2;

        var meshRenderer = m_InstancePrefab0.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            m_Material = Material.Instantiate(meshRenderer.sharedMaterial);
            m_Material.enableInstancing = true;
        }

        var meshFilter = m_InstancePrefab0.GetComponent<MeshFilter>();
        if (meshFilter != null) m_Mesh0 = meshFilter.sharedMesh;

        meshFilter = m_InstancePrefab1.GetComponent<MeshFilter>();
        if (meshFilter != null) m_Mesh1 = meshFilter.sharedMesh;

        meshFilter = m_InstancePrefab2.GetComponent<MeshFilter>();
        if (meshFilter != null) m_Mesh2 = meshFilter.sharedMesh;

    }


    public void StartTest()
    {

        // GameObject root = new GameObject("Normal");
        // root.transform.position = Vector3.zero;
        // root.transform.rotation = Quaternion.identity;
        // root.transform.localScale = Vector3.one;
        // root.transform.SetParent(transform);
        // m_Root = root;


        //随机1023个坐标
        for (int i = 0; i < MAX_COUNT_0; i++)
        {
            Vector3 pos = Random.insideUnitSphere * 500;
            float randomScale = Random.Range(0.5f, 4.0f);
            Vector3 scale = Vector3.one * randomScale;
            Quaternion rotation = Quaternion.Euler(Random.Range(0, 360f), Random.Range(0, 360f), Random.Range(0, 360f));
            Matrix4x4 trs = Matrix4x4.TRS(pos, rotation, scale);
            m_StoreMatrix0[i] = trs;
            m_StoreMatrix_Total[i] = trs;
        }

        for (int i = 0; i < MAX_COUNT_1; i++)
        {
            Vector3 pos = Random.insideUnitSphere * 500;
            float randomScale = Random.Range(0.5f, 4.0f);
            Vector3 scale = Vector3.one * randomScale;
            Quaternion rotation = Quaternion.Euler(Random.Range(0, 360f), Random.Range(0, 360f), Random.Range(0, 360f));
            Matrix4x4 trs = Matrix4x4.TRS(pos, rotation, scale);
            m_StoreMatrix1[i] = trs;
            m_StoreMatrix_Total[MAX_COUNT_0 + i] = trs;
        }

        for (int i = 0; i < MAX_COUNT_2; i++)
        {
            Vector3 pos = Random.insideUnitSphere * 500;
            float randomScale = Random.Range(0.5f, 4.0f);
            Vector3 scale = Vector3.one * randomScale;
            Quaternion rotation = Quaternion.Euler(Random.Range(0, 360f), Random.Range(0, 360f), Random.Range(0, 360f));
            Matrix4x4 trs = Matrix4x4.TRS(pos, rotation, scale);
            m_StoreMatrix2[i] = trs;
            m_StoreMatrix_Total[MAX_COUNT_0 + MAX_COUNT_1 + i] = trs;
        }

    }

    public void Enable(bool enable)
    {
        gameObject.SetActive(enable);
    }

    public void SetLODMode(LODMode mode)
    {
        m_LODMode = mode;
    }



    CommandBuffer m_buff = null;
    private void Update()
    {
        if (m_Material != null && m_Mesh0 != null)
        {
            if (m_LODMode == LODMode.SpltLOD)
            {
                Graphics.DrawMeshInstanced(m_Mesh0, 0, m_Material, m_StoreMatrix0, MAX_COUNT_0);
                Graphics.DrawMeshInstanced(m_Mesh1, 0, m_Material, m_StoreMatrix1, MAX_COUNT_1);
                Graphics.DrawMeshInstanced(m_Mesh2, 0, m_Material, m_StoreMatrix2, MAX_COUNT_2);
            }
            else
            {
                Graphics.DrawMeshInstanced(m_Mesh0, 0, m_Material, m_StoreMatrix_Total, MAX_COUNT_0 + MAX_COUNT_1 + MAX_COUNT_2);
            }

        }

    }

}
