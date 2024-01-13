using BowaXenotypeFollower.Loader;
using BowaXenotypeFollower.Settings;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace BowaXenotypeFollower.Ritual
{
    public static class XenotypeSelector
    {
        public static PawnGenerationRequest ModifyXenotypeInRequest(PawnGenerationRequest request)
        {

            List<XenoPawn> xenoPawns = new List<XenoPawn>();

            var allXenotypes = XenotypeFollowerSettings.BaseXenotypeDefNames.Select((name => new { Name = name, IsCustom = false })).Concat(
                XenotypeFollowerSettings.CustomXenotypesDefNames.Select(name => new { Name = name, IsCustom = true }));

            xenoPawns.AddRange(allXenotypes.Select(x => new XenoPawn
            {
                Name = x.Name,
                isCustom = x.IsCustom
            }));

            if (xenoPawns.Count <= 0) return request;

            XenoPawn rngXeno = xenoPawns.RandomElement();

            IEnumerable<XenotypeDef> xenotypeDefs = DefDatabase<XenotypeDef>.AllDefs
                    .Where((XenotypeDef x) => XenotypeFollowerSettings.BaseXenotypeDefNames.Contains(x.defName));
            IEnumerable<CustomXenotype> customXenogerms = CustomLoader.CustomXenotypes.Where((CustomXenotype x) => XenotypeFollowerSettings.CustomXenotypesDefNames.Contains(x.name));

            if (rngXeno.isCustom)
            {
                request.ForcedCustomXenotype = customXenogerms.FirstOrDefault((x) => x.name == rngXeno.Name);
            }
            else
            {
                request.ForcedXenotype = xenotypeDefs.FirstOrDefault((x) => x.defName == rngXeno.Name);
            }
            return request;

        }

        class XenoPawn
        {
            public string Name { get; set; }
            public bool isCustom { get; set; }
        }
    }
}
