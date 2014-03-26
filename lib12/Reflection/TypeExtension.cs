﻿using System;
using System.Linq;
using lib12.Collections;
using lib12.Exceptions;

namespace lib12.Reflection
{
    public static class TypeExtension
    {
        /// <summary>
        /// Determines whether the specified type is numeric or nullable numeric
        /// </summary>
        /// <param name="type">The type to check</param>
        /// <returns></returns>
        public static bool IsTypeNumericOrNullableNumeric(this Type type)
        {
            return type.IsTypeNumeric() || (type.IsNullable() && Nullable.GetUnderlyingType(type).IsTypeNumeric());
        }

        /// <summary>
        /// Determines whether the specified type is numeric
        /// </summary>
        /// <param name="type">The type to check</param>
        /// <returns></returns>
        public static bool IsTypeNumeric(this Type type)
        {
            return type.IsPrimitive || Type.GetTypeCode(type) == TypeCode.Decimal;
        }

        /// <summary>
        /// Determines whether the specified type nullable
        /// </summary>
        /// <param name="type">The type to check</param>
        /// <returns></returns>
        public static bool IsNullable(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        /// <summary>
        /// Gets the attribute decorating given type
        /// </summary>
        /// <param name="type">The type to check</param>
        /// <returns></returns>
        public static T GetAttribute<T>(this Type type) where T : Attribute
        {
            var attribute = type.GetCustomAttributes(typeof(T), false).SingleOrDefault();
            return attribute != null ? (T)attribute : default(T);
        }

        /// <summary>
        /// Gets the default of given type
        /// </summary>
        /// <param name="type">The type to operate</param>
        /// <returns></returns>
        public static object GetDefault(this Type type)
        {
            if (type.IsValueType)
                return Activator.CreateInstance(type);
            else
                return null;
        }

        /// <summary>
        /// Gets the default, parameterless constructor of given type or null if this not exist
        /// </summary>
        /// <param name="type">The type to operate</param>
        /// <returns></returns>
        public static object GetDefaultConstructor(this Type type)
        {
            return type.GetConstructor(Type.EmptyTypes);
        }

        public static object GetPropertyValue(this Type type, object source, string propertyName)
        {
            if (propertyName.IsNullOrEmpty())
                throw new ArgumentException("Provided property name cannot be null or empty", propertyName);

            var prop = type.GetProperty(propertyName);
            if (prop == null)
                throw new lib12Exception(string.Format("Type {0} don't have property named {1}", type.Name, propertyName));

            return prop.GetValue(source, null);
        }
    }
}
