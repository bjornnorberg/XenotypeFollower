using HarmonyLib;
using RimWorld;
using RimWorld.QuestGen;
using System.Collections.Generic;
using System.Security.Cryptography;
using Verse;

namespace BowaXenotypeFollower.Ritual
{
    [HarmonyPatch(typeof(RitualAttachableOutcomeEffectWorker_RandomRecruit), "Apply")]
    public static class Ritual
    {
        [HarmonyPrefix]
        public static bool Prefix(RitualAttachableOutcomeEffectWorker_RandomRecruit __instance, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, OutcomeChance outcome, ref string extraOutcomeDesc, ref LookTargets letterLookTargets)
        {
            Log.Message("BowaXenotypeFollower.Ritual.PostfixApply running!");
            var defField = AccessTools.Field(__instance.GetType().BaseType, "def");
            var def = defField.GetValue(__instance) as RitualAttachableOutcomeEffectDef;

            if (Rand.Chance(0.5f))
            {
                PawnGenerationRequest pawnGenerationRequest = new PawnGenerationRequest(PawnKindDefOf.Villager, null, PawnGenerationContext.NonPlayer, -1, forceGenerateNewPawn: true, allowDead: false, allowDowned: false, canGeneratePawnRelations: true, mustBeCapableOfViolence: false, 20f, forceAddFreeWarmLayerIfNeeded: false, allowGay: true, allowPregnant: false, allowFood: true, allowAddictions: true, inhabitant: false, certainlyBeenInCryptosleep: false, forceRedressWorldPawnIfFormerColonist: false, worldPawnFactionDoesntMatter: false, 0f, 0f, null, 1f, null, null, null, null, null, null, 60f, null, "MAKARON", "MAKARONER", null, jobRitual.Ritual.ideo, false, false, false, false, null, null, null, null, null, 0);
                Slate slate = new Slate();
                slate.Set("map", jobRitual.Map);
                slate.Set("overridePawnGenParams", pawnGenerationRequest);
                QuestUtility.GenerateQuestAndMakeAvailable(QuestScriptDefOf.WandererJoins, slate);
                extraOutcomeDesc = def.letterInfoText;
            }
            else
            {
                extraOutcomeDesc = null;
            }
            return false;
        }
    }
}
