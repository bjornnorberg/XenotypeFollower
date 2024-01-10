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
        public SettingWindow SettingWindow;

        public XenotypeFollower(ModContentPack content) : base(content)
        {
            settings = GetSettings<XenotypeFollowerSettings>();
            SettingWindow = new SettingWindow();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            SettingWindow.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return "BowaXenotypeFollower".Translate();
        }

    }
}
