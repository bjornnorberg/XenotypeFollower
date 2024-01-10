using BowaXenotypeFollower.Loader;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace BowaXenotypeFollower.Settings
{
    public class SettingWindow
    {
        private IEnumerable<XenotypeDef> BaseXenotypes = new List<XenotypeDef>();

        private List<CustomXenotype> CustomXenotypes = new List<CustomXenotype>();

        private float scrollViewHeight = 96f;

        private Vector2 scrollPosition = Vector2.zero;

        public SettingWindow()
        {
        }

        public void DoSettingsWindowContents(Rect inRect)
        {
            BaseXenotypes = PossibleBaseXenoTypes();
            CustomXenotypes = CustomLoader.CustomXenotypes;

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

                foreach (var baseXenoType in BaseXenotypes)
                {
                    if (!XenotypeFollowerSettings.BaseXenotypeDefNames.Contains(baseXenoType.defName))
                    {
                        listOfAddableBaseXenotypes.Add(new FloatMenuOption(baseXenoType.defName, delegate ()
                        {
                            XenotypeFollowerSettings.BaseXenotypeDefNames.Add(baseXenoType.defName);
                        }, itemIcon: baseXenoType.Icon, iconColor: Color.white));
                    }
                }

                foreach (var customXenoType in CustomXenotypes)
                {
                    if (!XenotypeFollowerSettings.BaseXenotypeDefNames.Contains(customXenoType.name))
                    {
                        listOfAddableBaseXenotypes.Add(new FloatMenuOption(customXenoType.name, delegate ()
                        {
                            XenotypeFollowerSettings.BaseXenotypeDefNames.Add(customXenoType.name);
                        }));
                    }
                    Log.Message("foreach in : Mod" + customXenoType.name);
                }

                FloatMenu addWindow = new FloatMenu(listOfAddableBaseXenotypes);
                Find.WindowStack.Add(addWindow);
            }


            #endregion Add baseXenoType

            #region Remove baseXenoType

            Rect removeBaseXenoType = new Rect(0f, 204f, 200f, 32f);
            bool removeFlag = Widgets.ButtonText(removeBaseXenoType, "Remove from list", true, true, true);
            if (removeFlag && (XenotypeFollowerSettings.CustomXenotypesDefNames.Count > 0 || XenotypeFollowerSettings.BaseXenotypeDefNames.Count > 0))
            {
                List<FloatMenuOption> listOfRemovableBaseXenotypes = new List<FloatMenuOption>();
                List<string> addedBaseXenotypeDefNames = XenotypeFollowerSettings.BaseXenotypeDefNames;
                IEnumerable<XenotypeDef> addedBaseXenotypeDefs = BaseXenotypes.Where((x) => addedBaseXenotypeDefNames.Contains(x.defName));

                foreach (XenotypeDef addedBaseXenotypeDef in addedBaseXenotypeDefs)
                {
                    listOfRemovableBaseXenotypes.Add(new FloatMenuOption(addedBaseXenotypeDef.defName, delegate ()
                    {
                        XenotypeFollowerSettings.BaseXenotypeDefNames.Remove(addedBaseXenotypeDef.defName);

                    }, itemIcon: addedBaseXenotypeDef.Icon, iconColor: Color.white));
                }

                //foreach (var customXenoType in CustomXenotypes)
                //{
                //    if (!XenotypeFollowerSettings.BaseXenotypeDefNames.Contains(customXenoType.name))
                //    {
                //        listOfAddableBaseXenotypes.Add(new FloatMenuOption(customXenoType.name, delegate ()
                //        {
                //            XenotypeFollowerSettings.BaseXenotypeDefNames.Add(customXenoType.name);
                //        }));
                //    }
                //    Log.Message("foreach in : Mod" + customXenoType.name);
                //}

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

            foreach (string baseXenotypeDefName in XenotypeFollowerSettings.BaseXenotypeDefNames)
            {
                Rect rectList = new Rect(0f, startHeight, viewRect.width, 32f);

                var baseXenoType = BaseXenotypes.FirstOrDefault(x => x.defName == baseXenotypeDefName);
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
        }

        public static IEnumerable<XenotypeDef> PossibleBaseXenoTypes() => DefDatabase<XenotypeDef>.AllDefs.OrderBy((XenotypeDef x) => x.defName);
    }
}
