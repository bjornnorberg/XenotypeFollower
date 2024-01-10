using BowaXenotypeFollower.Loader;
using BowaXenotypeFollower.Settings;
using HarmonyLib;
using RimWorld;
using RimWorld.QuestGen;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace BowaXenotypeFollower.Ritual
{
    [HarmonyPatch(typeof(RitualAttachableOutcomeEffectWorker_RandomRecruit), "Apply")]
    public static class Ritual
    {
        class XenoPawn
        {
            public string Name { get; set; }
            public bool isCustom { get; set; }
        }

        [HarmonyPrefix]
        public static bool Prefix(RitualAttachableOutcomeEffectWorker_RandomRecruit __instance, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, OutcomeChance outcome, ref string extraOutcomeDesc, ref LookTargets letterLookTargets)
        {
            List<XenoPawn> xenoPawns = new List<XenoPawn>();

            foreach (var xeno in XenotypeFollowerSettings.BaseXenotypeDefNames)
            {
                xenoPawns.Add(new XenoPawn
                {
                    Name = xeno,
                    isCustom = false
                });
            }

            foreach (var xeno in XenotypeFollowerSettings.CustomXenotypesDefNames)
            {
                xenoPawns.Add(new XenoPawn
                {
                    Name = xeno,
                    isCustom = true
                });
            }

            if (xenoPawns.Count <= 0) return true;

            var defField = AccessTools.Field(__instance.GetType().BaseType, "def");
            var def = defField.GetValue(__instance) as RitualAttachableOutcomeEffectDef;

            if (Rand.Chance(0.5f))
            {
                PawnGenerationRequest pawnGenerationRequest = new PawnGenerationRequest(PawnKindDefOf.Villager, null, PawnGenerationContext.NonPlayer, -1, forceGenerateNewPawn: true, allowDead: false, allowDowned: false, canGeneratePawnRelations: true, mustBeCapableOfViolence: false, 20f, forceAddFreeWarmLayerIfNeeded: false, allowGay: true, allowPregnant: false, allowFood: true, allowAddictions: true, inhabitant: false, certainlyBeenInCryptosleep: false, forceRedressWorldPawnIfFormerColonist: false, worldPawnFactionDoesntMatter: false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, jobRitual.Ritual.ideo);

                IEnumerable<XenotypeDef> xenotypeDefs = DefDatabase<XenotypeDef>.AllDefs
                    .Where((XenotypeDef x) => XenotypeFollowerSettings.BaseXenotypeDefNames.Contains(x.defName));

                IEnumerable<CustomXenotype> customXenogerms = CustomLoader.CustomXenotypes.Where((CustomXenotype x) => XenotypeFollowerSettings.CustomXenotypesDefNames.Contains(x.name));

                XenoPawn rngXeno = xenoPawns.RandomElement();

                if (rngXeno.isCustom)
                {
                    pawnGenerationRequest.ForcedCustomXenotype = customXenogerms.First((x) => x.name == rngXeno.Name);
                }
                else
                {
                    pawnGenerationRequest.ForcedXenotype = xenotypeDefs.First((x) => x.defName == rngXeno.Name);
                }

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
