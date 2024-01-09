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

        IEnumerable<XenotypeDef> baseXenoTypes = new List<XenotypeDef>();

        public XenotypeFollower(ModContentPack content) : base(content)
        {
            this.settings = GetSettings<XenotypeFollowerSettings>();
            Log.Message("Settings content " + settings.baseXenotypeDefsNames);
            baseXenoTypes = PossibleBaseXenoTypes();
            Log.Message("baseXenoTypes length: " + baseXenoTypes.Count());
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);
            listingStandard.Label("NOTE: This is a test label");
            listingStandard.End();

            Rect addItem = new Rect(0f, 172f, 200f, 32f);
            bool flag = Widgets.ButtonText(addItem, "Add to list", true, true, true);
            if (flag)
            {
                List<FloatMenuOption> list = new List<FloatMenuOption>();

                foreach (var baseXenoType in baseXenoTypes)
                {
                    if (!settings.baseXenotypeDefsNames.Contains(baseXenoType.defName))
                    {
                        list.Add(new FloatMenuOption(baseXenoType.label, delegate ()
                        {
                            settings.baseXenotypeDefsNames.Add(baseXenoType.defName);
                        }));
                    }
                }
                FloatMenu window = new FloatMenu(list);
                Find.WindowStack.Add(window);
            }

            listingStandard.End();
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
