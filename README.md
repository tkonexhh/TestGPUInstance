# TestGPUInstance

测试GPUInstnace和SRP Batcher性能
大量还是GPIInstance更好

测试GPUInstance 
直接DrawMesh 
全部LOD0的情况
LOD0 LOD1 LOD2测试

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
            
数量大的情况还是分组比较好

