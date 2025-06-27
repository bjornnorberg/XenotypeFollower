using BowaXenotypeFollower.Loader;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace BowaXenotypeFollower.Settings
{
    public class XenotypeFollowerSettings : ModSettings
    {
        public static List<string> BaseXenotypeDefNames = new List<string>();

        public static List<string> CustomXenotypesDefNames = new List<string>();

        public XenotypeFollowerSettings()
        {
        }

        public override void ExposeData()
        {
            base.ExposeData();


            if (Scribe.mode == LoadSaveMode.LoadingVars)
            {
                List<string> defNames = null;
                List<string> cleanDefNames = new List<string>();
                Scribe_Collections.Look(ref defNames, "baseXenotypeDefNames");

                // Gotta wait for DefOfs to load to use DefDatabase
                if (defNames != null)
                {
                    LongEventHandler.ExecuteWhenFinished(() =>
                    {
                        IEnumerable<string> availableBaseXenoTypesDefNames = DefDatabase<XenotypeDef>.AllDefs.OrderBy((XenotypeDef x) => x.defName).Select((XenotypeDef z) => z.defName);

                        // Remove potential defNames that no longer exists in the def database.
                        foreach (string defName in defNames)
                        {
                            if (availableBaseXenoTypesDefNames.Contains(defName)) cleanDefNames.Add(defName);
                        }
                        BaseXenotypeDefNames = cleanDefNames;
                    });
                }
            }


            Scribe_Collections.Look(ref BaseXenotypeDefNames, "baseXenotypeDefNames", LookMode.Value);
            Scribe_Collections.Look(ref CustomXenotypesDefNames, "customXenotypesDefNames", LookMode.Value);
        }



    }
}
