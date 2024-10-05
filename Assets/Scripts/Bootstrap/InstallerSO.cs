using UnityEngine;
using VContainer;

public abstract class InstallerSO : ScriptableObject
{
    public abstract void Install(IContainerBuilder builder);
}
