using HarmonyLib;
using Mono.Cecil.Cil;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;
using Verse;

namespace BowaXenotypeFollower.Ritual
{
    [HarmonyPatch(typeof(RitualAttachableOutcomeEffectWorker_RandomRecruit), "Apply")]
    public static class RitualAttachableOutcomeEffectWorker_RandomRecruit_Transpiler
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);

            ConstructorInfo targetConstructor = typeof(PawnGenerationRequest).GetConstructor(new[] { 
                typeof(PawnKindDef),
                typeof(Faction),
                typeof(PawnGenerationContext),
                typeof(int),
                typeof(bool),
                typeof(bool),
                typeof(bool),
                typeof(bool),
                typeof(bool),
                typeof(float),
                typeof(bool),
                typeof(bool),
                typeof(bool),
                typeof(bool),
                typeof(bool),
                typeof(bool),
                typeof(bool),
                typeof(bool),
                typeof(bool),
                typeof(float),
                typeof(float),
                typeof(Pawn),
                typeof(float),
                typeof(Predicate<Pawn>),
                typeof(Predicate<Pawn>),
                typeof(IEnumerable<TraitDef>),
                typeof(IEnumerable<TraitDef>),
                typeof(float?),
                typeof(float?),
                typeof(float?),
                typeof(Gender?),
                typeof(string),
                typeof(string),
                typeof(RoyalTitleDef),
                typeof(Ideo),
                typeof(bool),
                typeof(bool),
                typeof(bool),
                typeof(bool),
                typeof(List<GeneDef>),
                typeof(List<GeneDef>),
                typeof(XenotypeDef),
                typeof(CustomXenotype),
                typeof(List<XenotypeDef>),
                typeof(float),
                typeof(DevelopmentalStage),
                typeof(Func<XenotypeDef, PawnKindDef>),
                typeof(FloatRange?),
                typeof(FloatRange?),
                typeof(bool),
                typeof(bool),
                typeof(bool),
                typeof(int),
                typeof(int),
                typeof(bool)
            });

            MethodInfo modifyMethod = AccessTools.Method(typeof(XenotypeSelector), nameof(XenotypeSelector.ModifyXenotypeInRequest));
            var list = codes.ToList();

            int index = list.FindIndex(code => code.opcode == OpCodes.Newobj && code.operand as ConstructorInfo == targetConstructor);

            if (index != -1)
            {
                list.Insert(index + 1, new CodeInstruction(OpCodes.Call, modifyMethod));
            }

            return list.AsEnumerable();
        }

    }
}
