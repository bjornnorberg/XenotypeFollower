using BowaXenotypeFollower.Settings;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace BowaXenotypeFollower
{
    public class XenotypeFollower : Mod
    {
        public XenotypeFollowerSettings settings;

        IEnumerable<XenotypeDef> baseXenotypes = new List<XenotypeDef>();

        public XenotypeFollower(ModContentPack content) : base(content)
        {
            this.settings = GetSettings<XenotypeFollowerSettings>();
            Log.Message("baseXenotypeDefNames" + settings.baseXenotypeDefNames.Count());
            baseXenotypes = PossibleBaseXenoTypes();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);
            listingStandard.Label("NOTE: This is a test label");
            listingStandard.End();

            #region Add baseXenoType

            Rect addBaseXenoType = new Rect(0f, 172f, 200f, 32f);
            bool addFlag = Widgets.ButtonText(addBaseXenoType, "Add to list", true, true, true);
            if (addFlag)
            {
                List<FloatMenuOption> listOfAddableBaseXenotypes = new List<FloatMenuOption>();

                foreach (var baseXenoType in baseXenotypes)
                {
                    if (!settings.baseXenotypeDefNames.Contains(baseXenoType.defName))
                    {
                        listOfAddableBaseXenotypes.Add(new FloatMenuOption(baseXenoType.defName, delegate ()
                        {
                            settings.baseXenotypeDefNames.Add(baseXenoType.defName);
                        }));
                    }
                }
                FloatMenu addWindow = new FloatMenu(listOfAddableBaseXenotypes);
                Find.WindowStack.Add(addWindow);
            }


            #endregion Add baseXenoType

            #region Remove baseXenoType

            Rect removeBaseXenoType = new Rect(0f, 204f, 200f, 32f);
            bool removeFlag = Widgets.ButtonText(removeBaseXenoType, "Remove from list", true, true, true);
            if (removeFlag)
            {
                List<FloatMenuOption> listOfRemovableBaseXenotypes = new List<FloatMenuOption>();
                List<string> addedBaseXenotypeDefNames = settings.baseXenotypeDefNames;
                Log.Message("baseXenotypeDefNames" + settings.baseXenotypeDefNames.Count());
                foreach (var baseXenotypeDefName in addedBaseXenotypeDefNames)
                {
                    listOfRemovableBaseXenotypes.Add(new FloatMenuOption(baseXenotypeDefName, delegate ()
                    {
                        settings.baseXenotypeDefNames.Remove(baseXenotypeDefName);

                    }, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
                }
                FloatMenu removeWindow = new FloatMenu(listOfRemovableBaseXenotypes);
                Find.WindowStack.Add(removeWindow);
            }

            #endregion Remove baseXenoType

            base.DoSettingsWindowContents(inRect);
        }




        public static IEnumerable<XenotypeDef> PossibleBaseXenoTypes()
        {
            return DefDatabase<XenotypeDef>.AllDefs.OrderBy((XenotypeDef x) => x.defName);
        }

        public override string SettingsCategory()
        {
            return "BowaXenotypeFollower".Translate();
        }

    }
}
