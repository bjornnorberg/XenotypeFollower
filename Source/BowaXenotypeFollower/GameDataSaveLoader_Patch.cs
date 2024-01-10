using BowaXenotypeFollower.Loader;
using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;


namespace BowaXenotypeFollower
{
    [HarmonyPatch(typeof(GameDataSaveLoader), nameof(GameDataSaveLoader.SaveXenotype))]
    public class GameDataSaveLoader_Patch
    {
        [HarmonyPrefix]
        public static void Prefix(CustomXenotype xenotype) => CustomLoader.CustomXenotypes.Add(xenotype);
    }
}
