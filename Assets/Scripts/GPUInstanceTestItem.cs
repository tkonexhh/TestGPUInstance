using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public enum ShowMode
{
    Normal,
    Instance
}

public class GPUInstanceTestItem : MonoBehaviour
{
    private GameObject m_InstancePrefab;
    [SerializeField] private Mesh m_Mesh;
    [SerializeField] private Material m_Material;

    private const int MAX_COUNT = 1022;
    private int m_ShowCount = MAX_COUNT;

    private Matrix4x4[] m_StoreMatrix = new Matrix4x4[MAX_COUNT];
    private List<GameObject> m_NormalGOs = new List<GameObject>();

    private ShowMode m_ShowMode = ShowMode.Normal;
    private GameObject m_Root;


    public void Init(GameObject prefab)
    {
        m_InstancePrefab = prefab;

        var meshRenderer = m_InstancePrefab.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            m_Material = Material.Instantiate(meshRenderer.sharedMaterial);
            m_Material.enableInstancing = true;
        }

        var meshFilter = m_InstancePrefab.GetComponent<MeshFilter>();
        if (meshFilter != null)
        {
            m_Mesh = meshFilter.sharedMesh;
        }

    }


    public void StartTest()
    {
        if (m_InstancePrefab == null)
            return;

        GameObject root = new GameObject("Normal");
        root.transform.position = Vector3.zero;
        root.transform.rotation = Quaternion.identity;
        root.transform.localScale = Vector3.one;
        root.transform.SetParent(transform);
        m_Root = root;


        //随机1023个坐标
        for (int i = 0; i < MAX_COUNT; i++)
        {
            Vector3 pos = Random.insideUnitSphere * 500;
            float randomScale = Random.Range(0.5f, 4.0f);
            Vector3 scale = Vector3.one * randomScale;
            Quaternion rotation = Quaternion.Euler(Random.Range(0, 360f), Random.Range(0, 360f), Random.Range(0, 360f));
            Matrix4x4 trs = Matrix4x4.TRS(pos, rotation, scale);
            m_StoreMatrix[i] = trs;
        }

        for (int i = 0; i < m_StoreMatrix.Length; i++)
        {
            var go = GameObject.Instantiate(m_InstancePrefab);
            Matrix4x4 trs = m_StoreMatrix[i];
            go.transform.position = trs.GetColumn(3);
            go.transform.localScale = trs.lossyScale;
            go.transform.rotation = trs.rotation;
            go.transform.parent = m_Root.transform;
            m_NormalGOs.Add(go);
        }

    }

    public void Enable(bool enable)
    {
        gameObject.SetActive(enable);
    }

    public void SetShowMode(ShowMode mode)
    {
        m_ShowMode = mode;
        OnShowModeChanged();
    }

    public void ShowCount(int count)
    {
        m_ShowCount = count;
        OnShowCountChanged();

    }


    private void OnShowModeChanged()
    {
        m_Root.SetActive(m_ShowMode == ShowMode.Normal);
    }


    private void OnShowCountChanged()
    {
        for (int i = 0; i < m_NormalGOs.Count; i++)
        {
            m_NormalGOs[i].SetActive(i < m_ShowCount);
        }
    }

    CommandBuffer m_buff = null;
    private void Update()
    {
        if (m_ShowMode == ShowMode.Instance && m_ShowCount > 0)
        {
            if (m_Material != null && m_Mesh != null)
            {

                Graphics.DrawMeshInstanced(m_Mesh, 0, m_Material, m_StoreMatrix, m_ShowCount);

                // if (m_buff != null)
                // {
                //     Camera.main.RemoveCommandBuffer(CameraEvent.AfterForwardOpaque, m_buff);
                //     CommandBufferPool.Release(m_buff);
                // }

                // m_buff = CommandBufferPool.Get("DrawMeshInstanced");

                // for (int i = 0; i < 1; i++)
                // {
                //     m_buff.DrawMeshInstanced(m_Mesh, 0, m_Material, 0, m_StoreMatrix, m_ShowCount);
                // }
                // Camera.main.AddCommandBuffer(CameraEvent.AfterForwardOpaque, m_buff);
            }
        }



    }

}
