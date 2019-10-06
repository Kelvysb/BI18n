using BI18n.Models;
using BI18n.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Globalization;
using System.Threading;
using BI18n.Helpers;

namespace BI18n
{
    public class I18n
    {

        #region Declarations
        private static I18n instance;
        private string defaultLanguage = "us_en";
        private string currentLanguage = "us_en";
        private string strBasePath;
        private List<string> strLanguageSetsKeys;
        private List<BLanguageSet> objLanguageSets;
        #endregion

        #region Constructor
        private I18n(string p_strPath)
        {
            try
            {
                this.objLanguageSets = new List<BLanguageSet>();
                this.strLanguageSetsKeys = new List<string>();
                LoadFolder(p_strPath);
                this.defaultLanguage = "us_en";
                this.currentLanguage = Thread.CurrentThread.CurrentCulture.Name;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private I18n(string p_strPath, string defaultLanguage)
        {
            try
            {
                this.objLanguageSets = new List<BLanguageSet>();
                this.strLanguageSetsKeys = new List<string>();
                LoadFolder(p_strPath);
                this.defaultLanguage = defaultLanguage;
                this.currentLanguage = Thread.CurrentThread.CurrentCulture.Name;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private I18n(string p_strPath, string defaultLanguage, string currentLanguage)
        {
            try
            {
                this.objLanguageSets = new List<BLanguageSet>();
                this.strLanguageSetsKeys = new List<string>();
                LoadFolder(p_strPath);
                this.defaultLanguage = defaultLanguage;
                this.currentLanguage = currentLanguage;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Functions

        public static void Initialize(string p_strPath, string defaultLanguage, string currentLanguage)
        {
            try
            {
                instance = new I18n(p_strPath, defaultLanguage, currentLanguage);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static void Initialize(string p_strPath, string defaultLanguage)
        {
            try
            {
                instance = new I18n(p_strPath, defaultLanguage);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static void Initialize(string p_strPath)
        {
            try
            {
                instance = new I18n(p_strPath);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static void Initialize()
        {
            try
            {
                string path = Path.Combine(".", "locales");
                if (!Directory.Exists(path) || !Directory.GetFiles(path).Any())
                {
                    throw new FileNotFoundException(@"i18n folder or it contents are not found in app root. 
                                                      Create the folder and it or especify other in Initialyze(string) static method.");
                }
                instance = new I18n(path);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static string Translate(string p_strKey)
        {
            CheckInstance();
            return instance.GetValue(p_strKey);
        }

        public static string Translate(string p_strKey, string p_strLanguageKey)
        {
            CheckInstance();
            return instance.GetValue(p_strKey, p_strLanguageKey);
        }

        public static string Translate(string p_strKey, CultureInfo p_objCulture)
        {
            CheckInstance();
            return instance.GetValue(p_strKey, p_objCulture);
        }

        public static void AddLanguageSet(BLanguageSet p_objLanguageSet)
        {
            try
            {
                CheckInstance();
                if (LanguageSets.Any(item => item.LanguageKey.Equals(p_objLanguageSet.LanguageKey, StringComparison.InvariantCultureIgnoreCase)))
                {
                    throw new LanguageSetAlreadyExistsException(p_objLanguageSet.LanguageKey);
                }
                LanguageSets.Add(p_objLanguageSet);
                instance.strLanguageSetsKeys.Add(p_objLanguageSet.LanguageKey);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public static void SetDefaultLanguage(string p_strLanguageKey)
        {
            CheckInstance();
            instance.defaultLanguage = p_strLanguageKey;
        }

        public static void SetDefaultLanguage(CultureInfo p_objCulture)
        {
            CheckInstance();
            instance.defaultLanguage = p_objCulture.Name;
        }

        public static void SetCurrentLanguage(string p_strLanguageKey)
        {
            CheckInstance();
            instance.currentLanguage = p_strLanguageKey;
        }

        public static void SetCurrentLanguage(CultureInfo p_objCulture)
        {
            CheckInstance();
            instance.currentLanguage = p_objCulture.Name;
        }

        public static void Save()
        {
            try
            {
                CheckInstance();
                foreach (BLanguageSet item in LanguageSets)
                {
                    LanguageFileConverter.SaveLanguageFile(item, instance.strBasePath);
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        private string GetValue(string p_strKey, string p_strLanguageKey)
        {
            BLanguageSet objAuxLanguageSet;

            try
            {

                objAuxLanguageSet = LanguageSets.Find(langSet => langSet.LanguageKey.Equals(p_strLanguageKey, StringComparison.InvariantCultureIgnoreCase));

                if (objAuxLanguageSet == null)
                {
                    objAuxLanguageSet = LanguageSets.Find(langSet => langSet.LanguageKey.Equals(instance.defaultLanguage, StringComparison.InvariantCultureIgnoreCase));
                }

                if (objAuxLanguageSet == null && LanguageSets.Count > 0)
                {
                    objAuxLanguageSet = LanguageSets.First();
                }
                else if (LanguageSets.Count == 0)
                {
                    throw new LanguageSetNotFoundException(p_strLanguageKey);
                }

                return objAuxLanguageSet.GetValue(p_strKey);

            }
            catch (Exception)
            {
                throw;
            }

        }

        private string GetValue(string p_strKey)
        {
            try
            {
                return GetValue(p_strKey, currentLanguage);
            }
            catch (Exception)
            {
                throw;
            }

        }

        private string GetValue(string p_strKey, CultureInfo p_objCulture)
        {
            try
            {
                return GetValue(p_strKey, p_objCulture.Name);
            }
            catch (Exception)
            {
                throw;
            }

        }

        private void LoadFolder(string p_strPath)
        {

            string[] strFiles;

            try
            {

                strBasePath = p_strPath;

                if (string.IsNullOrEmpty(strBasePath))
                {
                    strBasePath = ".";
                }

                strFiles = Directory.GetFiles(strBasePath, "*.json");

                objLanguageSets = new List<BLanguageSet>();
                strLanguageSetsKeys = new List<string>();

                foreach (string file in strFiles)
                {
                    try
                    {
                        objLanguageSets.Add(LanguageFileConverter.LoadLanguageFile(file));
                        strLanguageSetsKeys.Add(objLanguageSets.Last().LanguageKey);
                    }
                    catch (Exception)
                    {

                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void CheckInstance()
        {
            if (instance == null)
            {
                Initialize();
            }
        }

        #endregion

        #region Properties        
        public static List<BLanguageSet> LanguageSets { get => instance?.objLanguageSets; }
        public static string BasePath { get => instance?.strBasePath; }
        public static List<string> LanguageSetsKeys { get => instance?.strLanguageSetsKeys; }
        #endregion
    }
}
