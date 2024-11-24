using CommonBoilerPlateEight.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CommonBoilerPlateEight.Domain.Extensions
{
    public static class EnumExtensions
    {
        public static List<TextValueDropdownDto> ToDropdownList<TEnum>()
         where TEnum : Enum
        {
            var enumType = typeof(TEnum);
            var values = Enum.GetValues(enumType);

            return values.Cast<TEnum>()
                .Select(value => new TextValueDropdownDto
                {
                    Text = GetDisplayText(value, enumType),
                    Value = value.ToString()
                })
                .ToList();
        }
        public static TEnum ToEnum<TEnum>(this string value, bool ignoreCase = true)
       where TEnum : struct, Enum
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (!Enum.TryParse<TEnum>(value, ignoreCase, out var result))
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, "Invalid string value for enum type.");
            }

            return result;
        }
        private static string GetDisplayText(Enum value, Type enumType)
        {
            var fieldInfo = enumType.GetField(value.ToString());
            var displayAttribute = fieldInfo?.GetCustomAttribute<DisplayAttribute>();

            return displayAttribute?.Name ?? value.ToString();
        }
    }
}
