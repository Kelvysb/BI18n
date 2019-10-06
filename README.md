# BI18n
Internationalization (i18n) library for .net (.net standard, .net Core, .net framework).

Uses default i18n json files, located on locales folder, located on the application root folder, or custom folder. Auto-detect current system language or manual set.

Install: 

Package manager:
```
  PM> Install-Package kelvysb.BI18n
 ```
.Net CLI:
```
  dotnet add package kelvysb.BI18n
```
Usage:

Get translation:
```
using BI18n;
//...
string text = I18n.Translate("Value_Key");
```

Set Current Language:
```
using BI18n;
//...
string text = I18n.SetCurrentLanguage("us-en");
```

Set Default Language:
```
using BI18n;
//...
string text = I18n.SetDefaultLanguage("us-en");
```

*If not set the default language will be 'us-en' and current language will be the system language.
