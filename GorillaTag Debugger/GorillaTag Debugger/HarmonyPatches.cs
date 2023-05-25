using System;
using System.Reflection;
using HarmonyLib;

namespace lol {
    public class HarmonyPatches {
        public static bool IsPatched { get; private set; }
        private static Harmony instance;
        public const string InstanceId = "org.Emattew1";

        internal static void ApplyHarmonyPatches() {
            if (!HarmonyPatches.IsPatched) {
                if (HarmonyPatches.instance == null) {
                    HarmonyPatches.instance = new Harmony("org.Emattew1");
                }
                HarmonyPatches.instance.PatchAll(Assembly.GetExecutingAssembly());
                HarmonyPatches.IsPatched = true;
            }
        }

        internal static void RemoveHarmonyPatches() {
            if (HarmonyPatches.instance != null && HarmonyPatches.IsPatched) {
                HarmonyPatches.instance.UnpatchSelf();
                HarmonyPatches.IsPatched = false;
            }
        }
    }
}
