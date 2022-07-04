using MelonLoader;
using PyroMod;
using PyroMod.API.QuickMenu;
using System;
using System.Collections;
using UnhollowerBaseLib.Attributes;
using UnhollowerRuntimeLib;
using UnityEngine;
using static PyroMod.Main;

namespace PyroPortal
{
    public class BuildInfo
    {
        public const string Name = "PyroPortal";
        public const string Version = "1.0.2";
        public const string Author = "Xavi";
        public const string DownloadLink = "https://github.com/xavion-lux/PyroPortal";
    }

    public class Mod : MelonMod
    {
        public static PyroModule module;
        public static PyroLogs.Instance logger;
        public static QMCategory menu;

        public override void OnApplicationStart()
        {
            ClassInjector.RegisterTypeInIl2Cpp<InfinitePortal>();

            module = RegisterModule(BuildInfo.Name, BuildInfo.Version, BuildInfo.Author, moduleDownloadUrl: BuildInfo.DownloadLink);
            menu = module.CreateCategory(BuildInfo.Name);

            QMSingleButton infiniteTimerButton = module.CreateButton(menu, "Infinite Timer",
                () =>
                {
                    foreach(PortalInternal p in GameObject.FindObjectsOfType<PortalInternal>())
                    {
                        if(p.field_Private_Int32_0 == VRC.Player.prop_Player_0.prop_Int32_0 && p.gameObject.GetComponent<InfinitePortal>() == null)
                        {
                            var i = p.gameObject.AddComponent<InfinitePortal>();
                            MelonCoroutines.Start(i.SetTimer());
                        }
                    }
                }, "Set your portals timer to max");

            QMSingleButton destroyPortal = module.CreateButton(menu, "Destroy Portals",
                () =>
                {
                    foreach (PortalInternal p in GameObject.FindObjectsOfType<PortalInternal>())
                    {
                        if (p.field_Private_Int32_0 != -1)
                            GameObject.DestroyImmediate(p.gameObject);
                    }
                }, "Destroy all player portals");

            QMSingleButton destroyOtherPortal = module.CreateButton(menu, "Destroy Other Portals",
                () =>
                {
                    foreach (PortalInternal p in GameObject.FindObjectsOfType<PortalInternal>())
                    {
                        if (p.field_Private_Int32_0 != -1 && p.field_Private_Int32_0 != VRC.Player.prop_Player_0.prop_Int32_0)
                            GameObject.DestroyImmediate(p.gameObject);
                    }
                }, "Destroy other player's portals");
        }
    }

    public class InfinitePortal : MonoBehaviour
    {
        [HideFromIl2Cpp]
        public IEnumerator SetTimer()
        {
            while(true)
            {
                try
                {
                    transform.GetComponent<PortalInternal>().SetTimerRPC(float.MinValue, VRC.Player.prop_Player_0);
                }
                catch 
                { 
                    break; 
                }
                
                yield return null;
            }
        }
        public InfinitePortal(IntPtr ptr) : base(ptr)
        {
            //Don't delete, it's necessary
        }
    }
}
