using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace BowaXenotypeFollower.Settings
{
    public class XenotypeFollowerSettings : ModSettings
    {
        public List<string> baseXenotypeDefNames = new List<string>();
        //public List<XenotypeDef> baseXenotypeDefs = new List<XenotypeDef>();
        public List<string> customXenotypesDefNames = new List<string>();
        //public List<CustomXenotype> customXenotypesDefs = new List<CustomXenotype>();

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Collections.Look(ref baseXenotypeDefNames, "baseXenotypeDefNames", LookMode.Value);
            Scribe_Collections.Look(ref customXenotypesDefNames, "customXenotypesDefNames", LookMode.Value);
        }

    }
}
