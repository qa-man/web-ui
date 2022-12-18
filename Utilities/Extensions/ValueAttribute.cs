﻿using System;

namespace Utilities.Extensions
{
    public class ValueAttribute : Attribute
    {

        #region Properties

        public string Value { get; protected set; }

        #endregion

        #region Constructor

        public ValueAttribute(string value)
        {
            Value = value;
        }

        #endregion
    }

    public static class EnumExtension
    {
        public static string GetValue(this Enum value)
        {
            return value.GetType().GetField(value.ToString())?.GetCustomAttributes(typeof(ValueAttribute), false) is ValueAttribute[] { Length: > 0 } attributes ? attributes[0].Value : null;
        }
    }
}