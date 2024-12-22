using Module.Attcker;
using UnityEngine;


public class AttackerDrower : MonoBehaviour
{
    private AttakerModuleRuntimeData _attakerRuntimeData;

    public void Configure(AttakerModuleRuntimeData mapGeneratorRuntimeData)
    {
        _attakerRuntimeData = mapGeneratorRuntimeData;
    }

    private void OnDrawGizmos()
    {
        if (_attakerRuntimeData is null) return;
        foreach (var item in _attakerRuntimeData.Views)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(item.AttackPosition, item.AttackRange);
        }
    }
}
