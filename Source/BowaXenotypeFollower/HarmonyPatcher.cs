using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;


namespace BowaXenotypeFollower
{
    public class HarmonyPatcher
    {
        HarmonyPatcher() {
            Harmony harmony = new Harmony("BowaXenotypeFollower.HarmonyPatcher");
            harmony.PatchAll();        
        }
    }
}
