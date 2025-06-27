using HarmonyLib;
using System.Reflection;
using Verse;


namespace BowaXenotypeFollower
{
    [StaticConstructorOnStartup]
    public class HarmonyPatcher
    {
        static HarmonyPatcher()
        {
            Harmony harmony = new Harmony("com.bowaxenotypefollower");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
