using BI18n.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BI18n.Helpers
{
    public class LanguageFileConverter
    {
        private LanguageFileConverter()
        {
        }

        public static BLanguageSet LoadLanguageFile(string path)
        {
            try
            {
                StreamReader file = new StreamReader(path);
                BLanguageSet result = Deserialize(file.ReadToEnd(), Path.GetFileNameWithoutExtension(path));
                file.Close();
                file.Dispose();
                file = null;
                return result;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public static void SaveLanguageFile(BLanguageSet bGLanguageSet, string folderPath)
        {
            string filePath = Path.Combine(folderPath, String.Concat(bGLanguageSet.LanguageKey, ".json"));
            string result = Serialize(bGLanguageSet);
            StreamWriter file = new StreamWriter(filePath);
            file.Write(result);
            file.Close();
            file.Dispose();
        }

        private static string Serialize(BLanguageSet bGLanguageSet)
        {
            try
            {
                JObject result = new JObject();
                result.Add(new JProperty("values"));
                bGLanguageSet.Itens.ForEach(item => AddValue(item.Key, item.Value, result.Property("values")));
                return result.ToString(Formatting.Indented);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static BLanguageSet Deserialize(string p_strJson, string languageKey)
        {
            try
            {
                JObject jsonResult = JObject.Parse(p_strJson);
                List<BLanguageItem> itens = GetValues(jsonResult.Property("values"));
                return new BLanguageSet(itens, languageKey);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static List<BLanguageItem> GetValues(JProperty property)
        {
            List<BLanguageItem> result = new List<BLanguageItem>();

            if (property.Value is JObject)
            {
                foreach (JProperty item in ((JObject)property.Value).Properties())
                {
                    result.AddRange(GetValues(item));
                }
            }
            else
            {
                result.Add(new BLanguageItem(property.Path.Replace("values.", ""), property.Value.ToString()));
            }

            return result;
        }

        private static void AddValue(string key, string value, JProperty parent)
        {
            if (parent.Value.Count() == 0)
            {
                parent.Value = new JObject();
            }

            if (key.Split('.').Count() > 1)
            {

                if (!((JObject)parent.Value).Properties().Any(item =>
                        item.Name.Equals(key.Split('.').First(), StringComparison.InvariantCultureIgnoreCase)))
                {
                    ((JObject)parent.Value).Add(new JProperty(key.Split('.').First()));
                }


                AddValue(string.Join(".", key.Split('.').Skip(1)), value, ((JObject)parent.Value).Property(key.Split('.').First()));
            }
            else
            {
                ((JObject)parent.Value).Add(new JProperty(key, value));
            }
        }

    }
}
