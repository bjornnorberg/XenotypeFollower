using RimWorld;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace BowaXenotypeFollower.Loader
{
    public static class CustomLoader
    {
        public static List<CustomXenotype> CustomXenotypes = new List<CustomXenotype>();

        static CustomLoader()
        {
            LoadAllCustomXenotypes(); // Load from XML files in %appdata%                     
        }

        private static void LoadAllCustomXenotypes()
        {
            var customXenotypeDatabase = Current.Game?.customXenotypeDatabase?.customXenotypes;
            if (customXenotypeDatabase != null) // always false tbh
            {
                foreach (var customXenotype in customXenotypeDatabase)
                {
                    CustomXenotype loadedCustomXenotype = TryLoadCustomXenotype(customXenotype.name);
                    if (loadedCustomXenotype != null)
                        CustomXenotypes.Add(TryLoadCustomXenotype(loadedCustomXenotype.name));
                }
            }

            // CharacterCardUtility.CustomXenotypes, but without mod mismatch and version check
            foreach (var xenotypeFile in GenFilePaths.AllCustomXenotypeFiles.OrderBy((FileInfo f) => f.LastWriteTime))
            {
                CustomXenotype customXenotype = TryLoadCustomXenotype(Path.GetFileNameWithoutExtension(xenotypeFile.Name));
                if (customXenotype != null)
                {
                    Log.Message("customXenotype " + customXenotype.name);
                    CustomXenotypes.Add(customXenotype);
                }
            }
        }

        private static CustomXenotype TryLoadCustomXenotype(string name)
        {
            var xenotype = Current.Game?.customXenotypeDatabase?.customXenotypes.Find((x) => x.name == name); // Mostly null since main menu don't have a Current Game.
            if (xenotype is null)
                GameDataSaveLoader.TryLoadXenotype(GenFilePaths.AbsFilePathForXenotype(GenFile.SanitizedFileName(name)), out xenotype);

            return xenotype is null ? null : xenotype;
        }
    }
}
