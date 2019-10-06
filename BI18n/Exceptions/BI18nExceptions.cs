using System;
using System.Collections.Generic;
using System.Text;

namespace BI18n.Exceptions
{
    class ValueNotFoundException : Exception
    {
        string message;
        public ValueNotFoundException(string p_strItem)
        {
            message = "Item not found: " + p_strItem;
        }
        public override string Message => message;
    }

    class LanguageSetNotFoundException : Exception
    {
        string message;
        public LanguageSetNotFoundException(string p_strKey)
        {
            message = "Language Set not found: " + p_strKey;
        }
        public override string Message => message;
    }

    class LanguageSetAlreadyExistsException : Exception
    {
        string message;
        public LanguageSetAlreadyExistsException(string p_strKey)
        {
            message = "Language Already Exists: " + p_strKey;
        }
        public override string Message => message;
    }

}
