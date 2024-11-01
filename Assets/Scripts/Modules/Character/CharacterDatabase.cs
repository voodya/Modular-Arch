using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
[CreateAssetMenu(menuName = "Installers/Modules/Character", fileName = "CharacterDatabase")]
public class CharacterDatabase : InstallerSO
{
    [SerializeField] public CharacterView CharacterPfb;

    public override void Install(IContainerBuilder builder)
    {
        builder.RegisterInstance(this);
        builder.Register<CharacterModule>(Lifetime.Singleton).As<IModule>();
    }
}
