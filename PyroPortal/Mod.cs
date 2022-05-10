﻿using MelonLoader;
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
    public class Mod : MelonMod
    {
        public static PyroModule module;
        public static PyroLogs.Instance logger;
        public static QMCategory menu;

        public override void OnApplicationStart()
        {
            module = RegisterModule("Pyro Portal", "1.0.0", "Xavi");
            menu = module.CreateCategory("Pyro Portal");

            ClassInjector.RegisterTypeInIl2Cpp<InfinitePortal>();

            QMSingleButton infiniteTimerButton = module.CreateButton(menu, "Infinite Timer",
                () =>
                {
                    foreach (PortalInternal p in GameObject.FindObjectsOfType<PortalInternal>())
                    {
                        if (p.field_Private_Int32_0 == VRC.Player.prop_Player_0.prop_Int32_0)
                        {
                            GameObject g = new GameObject("InfiniteTimer");
                            g.transform.SetParent(p.transform);
                            var i = g.AddComponent<InfinitePortal>();
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
        public void Start()
        {
            MelonCoroutines.Start(SetTimer());
        }

        [HideFromIl2Cpp]
        public IEnumerator SetTimer()
        {
            while (true)
            {
                transform.parent.GetComponent<PortalInternal>().SetTimerRPC(float.MinValue, VRC.Player.prop_Player_0);
                yield return null;
            }
        }
        public InfinitePortal(IntPtr ptr) : base(ptr)
        {
            //Don't delete, it's necessary
        }
    }
}