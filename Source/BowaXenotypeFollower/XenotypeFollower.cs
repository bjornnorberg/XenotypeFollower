using BowaXenotypeFollower.Loader;
using BowaXenotypeFollower.Settings;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using UnityEngine;
using Verse;

namespace BowaXenotypeFollower
{
    public class XenotypeFollower : Mod
    {
        public XenotypeFollowerSettings settings;


        IEnumerable<XenotypeDef> baseXenotypes = new List<XenotypeDef>();
        List<CustomXenotype> customXenotypes = new List<CustomXenotype>();

        private float scrollViewHeight = 96f;

        private Vector2 scrollPosition = Vector2.zero;

        public XenotypeFollower(ModContentPack content) : base(content)
        {
            settings = GetSettings<XenotypeFollowerSettings>();
            baseXenotypes = PossibleBaseXenoTypes();
            customXenotypes = CustomLoader.customXenotypes;
            Log.Message("Custom xenotypes count: " + customXenotypes.Count);
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);
            listingStandard.Label("Add Xenotypes you want to spawn when performing ritual, if none are added vanilla rules are applied.");
            listingStandard.End();

            #region Add baseXenoType

            Rect addBaseXenoType = new Rect(0f, 172f, 200f, 32f);
            bool addFlag = Widgets.ButtonText(addBaseXenoType, "Add to list", true, true, true);
            if (addFlag)
            {
                List<FloatMenuOption> listOfAddableBaseXenotypes = new List<FloatMenuOption>();

                foreach (var baseXenoType in baseXenotypes)
                {
                    if (!XenotypeFollowerSettings.baseXenotypeDefNames.Contains(baseXenoType.defName))
                    {
                        listOfAddableBaseXenotypes.Add(new FloatMenuOption(baseXenoType.defName, delegate ()
                        {
                            XenotypeFollowerSettings.baseXenotypeDefNames.Add(baseXenoType.defName);
                        }, itemIcon: baseXenoType.Icon, iconColor: Color.white));
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
                List<string> addedBaseXenotypeDefNames = XenotypeFollowerSettings.baseXenotypeDefNames;
                Log.Message("baseXenotypeDefNames" + XenotypeFollowerSettings.baseXenotypeDefNames.Count());
                IEnumerable<XenotypeDef> addedBaseXenotypeDefs = baseXenotypes.Where((x) => addedBaseXenotypeDefNames.Contains(x.defName));
                foreach (XenotypeDef addedBaseXenotypeDef in addedBaseXenotypeDefs)
                {
                    listOfRemovableBaseXenotypes.Add(new FloatMenuOption(addedBaseXenotypeDef.defName, delegate ()
                    {
                        XenotypeFollowerSettings.baseXenotypeDefNames.Remove(addedBaseXenotypeDef.defName);

                    }, itemIcon: addedBaseXenotypeDef.Icon, iconColor: Color.white));
                }
                FloatMenu removeWindow = new FloatMenu(listOfRemovableBaseXenotypes);
                Find.WindowStack.Add(removeWindow);
            }

            #endregion Remove baseXenoType

            #region The list GUI

            var startHeight = 268f;
            Rect position = inRect.TopPart(0.90f);
            GUI.BeginGroup(position);
            Rect outRect = new Rect(0f, 268f, position.width, position.height);
            Rect viewRect = new Rect(0f, 268f, position.width, this.scrollViewHeight);
            Widgets.BeginScrollView(outRect, ref this.scrollPosition, viewRect, true);

            foreach (string baseXenotypeDefName in XenotypeFollowerSettings.baseXenotypeDefNames)
            {
                Rect rectList = new Rect(0f, startHeight, viewRect.width, 32f);

                var baseXenoType = baseXenotypes.FirstOrDefault(x => x.defName == baseXenotypeDefName);
                if (baseXenoType != null)
                {
                    Widgets.Label(rectList, baseXenoType.defName);
                    Widgets.DefIcon(rectList, baseXenoType);
                }
                startHeight = startHeight + 32f; // Increase everytime to make the new row in the list.
                this.scrollViewHeight = startHeight;
            }
            Widgets.EndScrollView();
            GUI.EndGroup();

            #endregion The list GUI

            base.DoSettingsWindowContents(inRect);
        }

        public static IEnumerable<XenotypeDef> PossibleBaseXenoTypes() => DefDatabase<XenotypeDef>.AllDefs.OrderBy((XenotypeDef x) => x.defName);

        public override string SettingsCategory()
        {
            return "BowaXenotypeFollower".Translate();
        }

    }
}
