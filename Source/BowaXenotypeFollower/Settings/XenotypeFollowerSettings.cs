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
        public List<string> baseXenotypeDefsNames = new List<string>();
        //public List<XenotypeDef> baseXenotypeDefs = new List<XenotypeDef>();
        public List<string> customXenotypesDefsNames = new List<string>();
        //public List<CustomXenotype> customXenotypesDefs = new List<CustomXenotype>();

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Collections.Look(ref baseXenotypeDefsNames, "baseXenotypeDefs", LookMode.Value);
            Scribe_Collections.Look(ref customXenotypesDefsNames, "customXenotypesDefs", LookMode.Value);
        }

    }
}
