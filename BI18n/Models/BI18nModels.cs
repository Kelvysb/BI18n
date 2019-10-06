using BI18n.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BI18n.Models
{
    public class BLanguageSet
    {

        #region Constructor
        public BLanguageSet(List<BLanguageItem> itens, string languageKey)
        {
            try
            {
                this.Itens = itens;
                this.LanguageKey = languageKey;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Functions


        public string GetValue(string p_strKey)
        {

            BLanguageItem objAuxReturn;

            try
            {

                objAuxReturn = Itens.Find((BLanguageItem item) => { return item.Key.Equals(p_strKey, StringComparison.InvariantCultureIgnoreCase); });

                if (objAuxReturn == null)
                {
                    throw new ValueNotFoundException(p_strKey);
                }

                return objAuxReturn.Value;

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Properties
        [JsonProperty("LANGUAGEKEY")]
        public string LanguageKey { get; private set; }

        [JsonProperty("ITENS")]
        public List<BLanguageItem> Itens { get; private set; }
        #endregion

    }

    public class BLanguageItem
    {
        public BLanguageItem(string key, string value)
        {
            Key = key;
            Value = value;
        }

        #region Properties
        [JsonProperty("KEY")]
        public string Key { get; private set; }

        [JsonProperty("VALUE")]
        public string Value { get; private set; }
        #endregion

    }

}
