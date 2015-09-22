using System;
using System.ComponentModel;
using System.Reflection;
using Catel;

namespace Poker.Application
{
    public class Extensions
    {
        /// <summary>
        /// Gets the value of the <see cref="DescriptionAttribute"/> from the specified enum value.
        /// </summary>
        /// <param name="value">the enum value to check.</param>
        /// <returns>The description value of the specified enumeration value.</returns>
        public static string GetEnumDescription(Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[]) fieldInfo.GetCustomAttributes(typeof (DescriptionAttribute), false);

            if (attributes.Length > 0)
                return attributes[0].Description;

            return value.ToString();
        }
    }
}