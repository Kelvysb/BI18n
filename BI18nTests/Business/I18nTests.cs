using BI18n;
using BI18n.Helpers;
using BI18n.Models;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace BI18nTests.Business
{
    public class I18nTests
    {

        [Fact]
        public void TranslateTest()
        {

            GenerateTestFiles();

            I18n.SetCurrentLanguage("pt-br");
            string test1 = I18n.Translate("group_one.text_one");
            string test2 = I18n.Translate("group_two.sub_group.text_nine");

            Assert.Equal("text 1 pt", test1);
            Assert.Equal("text 9 pt", test2);


        }

        [Fact]
        public void TranslateWithLangSetTest()
        {
            GenerateTestFiles();
            I18n.SetCurrentLanguage("en-us");
            string test1 = I18n.Translate("group_one.text_one");
            string test2 = I18n.Translate("group_two.sub_group.text_nine");

            Assert.Equal("text 1 us", test1);
            Assert.Equal("text 9 us", test2);
        }

        private void GenerateTestFiles()
        {

            BLanguageSet objAuxTest;

            try
            {
                if (!Directory.Exists(Path.Combine(".", "locales")))
                {
                    Directory.CreateDirectory(Path.Combine(".", "locales"));
                }

                if (File.Exists(Path.Combine(".", "locales", "pt-br.json")))
                {
                    File.Delete(Path.Combine(".", "locales", "pt-br.json"));
                }

                if (File.Exists(Path.Combine(".", "locales", "en-us.json")))
                {
                    File.Delete(Path.Combine(".", "locales", "en-us.json"));
                }

                if (!File.Exists(Path.Combine(".", "locales", "pt-br.json")))
                {
                    objAuxTest = new BLanguageSet(new List<BLanguageItem>()
                    {
                        new BLanguageItem("group_one.text_one", "text 1 pt"),
                        new BLanguageItem("group_one.text_two", "text 2 pt"),
                        new BLanguageItem("group_one.text_three", "text 3 pt"),
                        new BLanguageItem("group_one.text_four", "text 4 pt"),
                        new BLanguageItem("group_two.text_five", "text 5 pt"),
                        new BLanguageItem("group_two.text_six", "text 6 pt"),
                        new BLanguageItem("group_two.text_seven", "text 7 pt"),
                        new BLanguageItem("group_two.text_eight", "text 8 pt"),
                        new BLanguageItem("group_two.sub_group.text_nine", "text 9 pt"),
                    }, "pt-br");
                    LanguageFileConverter.SaveLanguageFile(objAuxTest, Path.Combine(".", "locales"));
                }

                if (!File.Exists(Path.Combine(".", "locales", "en-us.json")))
                {
                    objAuxTest = new BLanguageSet(new List<BLanguageItem>()
                    {
                        new BLanguageItem("group_one.text_one", "text 1 us"),
                        new BLanguageItem("group_one.text_two", "text 2 us"),
                        new BLanguageItem("group_one.text_three", "text 3 us"),
                        new BLanguageItem("group_one.text_four", "text 4 us"),
                        new BLanguageItem("group_two.text_five", "text 5 us"),
                        new BLanguageItem("group_two.text_six", "text 6 us"),
                        new BLanguageItem("group_two.text_seven", "text 7 us"),
                        new BLanguageItem("group_two.text_eight", "text 8 us"),
                        new BLanguageItem("group_two.sub_group.text_nine", "text 9 us"),
                    }, "en-us");
                    LanguageFileConverter.SaveLanguageFile(objAuxTest, Path.Combine(".", "locales"));
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
