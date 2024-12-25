using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmosDrower : MonoBehaviour
{
    private MapGeneratorRuntimeData _mapGeneratorRuntimeData;

    public GameObject Configure(MapGeneratorRuntimeData mapGeneratorRuntimeData)
    {
        _mapGeneratorRuntimeData = mapGeneratorRuntimeData;
        return this.gameObject;
    }

    private void OnDrawGizmos()
    {
        if (_mapGeneratorRuntimeData is null) return;
        foreach (var item in _mapGeneratorRuntimeData.HightPoints)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(new Vector3(item.x, 2f, item.y), 0.1f);
        }
        foreach (var item in _mapGeneratorRuntimeData.LowPoints)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(new Vector3(item.x, 2f, item.y), 0.1f);
        }
    }
}
