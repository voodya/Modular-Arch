using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace Module.Attcker
{
    [CreateAssetMenu(menuName = "Installers/Modules/Attaker", fileName = "AttakerModuleDatabase")]
    public class AttakerModuleDatabase : InstallerSO
    {
        [SerializeField] public List<AttakerViewBase> ViewPfbs;
        [SerializeField] public int AttakeCount;

        public override void Install(IContainerBuilder builder)
        {
            builder.RegisterInstance(this);
            builder.Register<AttakerModule>(Lifetime.Singleton).As<IModule>();
        }
    }
}



