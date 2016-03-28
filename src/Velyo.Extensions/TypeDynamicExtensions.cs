using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;
using System.Diagnostics;

namespace System
{
    /// <summary>
    /// Extension methods for type reflection using dynamic methods.
    /// </summary>
    [DebuggerStepThrough]
    internal static class TypeDynamicExtensions
    {
        delegate object ContructorDelegate();
        delegate object FieldGetDelegate(object instance);
        delegate void FieldSetDelegate(object instance, object value);
        delegate object PropertyGetDelegate(object instance);
        delegate void PropertySetDelegate(object instance, object value);

        static Dictionary<string, Delegate> Cache = new Dictionary<string, Delegate>();

        /// <summary>
        /// Creates an instance of the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static object CreateInstance(this Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            string key = GenDynamicName(type, "ctor");
            ContructorDelegate handler = GetFromCache(key) as ContructorDelegate;

            if (handler == null)
            {
                ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);
                Type returnType = typeof(object);
                Type[] parameterTypes = Type.EmptyTypes;
                DynamicMethod method = new DynamicMethod(key, returnType, parameterTypes, type, true);

                ILGenerator il = method.GetILGenerator();
                LocalBuilder result = il.DeclareLocal(type);
                il.Emit(OpCodes.Newobj, constructor);
                il.Emit(OpCodes.Stloc, result);
                il.Emit(OpCodes.Ldloc, result);
                il.Emit(OpCodes.Ret);

                handler = method.CreateDelegate(typeof(ContructorDelegate)) as ContructorDelegate;
                if (handler != null) SetToCache(key, handler);
            }

            return (handler != null) ? handler() : null;
        }

        /// <summary>
        /// Gets the field value of specified type instance.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="instance">The instance.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static object GetFieldValue(this Type type, object instance, string name)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (instance == null) throw new ArgumentNullException("instance");
            if (name == null) throw new ArgumentNullException("name");

            string key = GenDynamicName(type, name);
            FieldGetDelegate handler = GetFromCache(key) as FieldGetDelegate;

            if (handler == null)
            {
                FieldInfo field = type.GetField(name);
                Type returnType = typeof(object);
                Type[] parameterTypes = new Type[] { typeof(object) };
                DynamicMethod method = new DynamicMethod(key, returnType, parameterTypes, type, true);

                ILGenerator il = method.GetILGenerator();
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldfld, field);
                if (field.FieldType.IsValueType)
                    il.Emit(OpCodes.Box, field.FieldType);
                il.Emit(OpCodes.Ret);

                handler = method.CreateDelegate(typeof(FieldGetDelegate)) as FieldGetDelegate;
                if (handler != null) SetToCache(key, handler);
            }

            return (handler != null) ? handler(instance) : null;
        }

        /// <summary>
        /// Sets the field value of the specified type instance.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="instance">The instance.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static void SetFieldValue(this Type type, object instance, string name, object value)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (instance == null) throw new ArgumentNullException("instance");
            if (name == null) throw new ArgumentNullException("name");

            string key = GenDynamicName(type, name);
            FieldSetDelegate handler = GetFromCache(key) as FieldSetDelegate;

            if (handler == null)
            {
                FieldInfo field = type.GetField(name);
                Type returnType = typeof(void);
                Type[] parameterTypes = new Type[] { typeof(object), typeof(object) };//, typeof(object) }
                DynamicMethod method = new DynamicMethod(key, returnType, parameterTypes, type, true);

                ILGenerator il = method.GetILGenerator();
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Stfld, field);
                if (field.FieldType.IsValueType)
                    il.Emit(OpCodes.Unbox_Any, field.FieldType);
                il.Emit(OpCodes.Ret);

                handler = method.CreateDelegate(typeof(FieldSetDelegate)) as FieldSetDelegate;
                if (handler != null) SetToCache(key, handler);
            }

            if (handler != null) handler(instance, value);
        }

        /// <summary>
        /// Gets the property value of the specified type instance.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="instance">The instance.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static object GetPropertyValue(this Type type, object instance, string name)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (instance == null) throw new ArgumentNullException("instance");
            if (name == null) throw new ArgumentNullException("name");

            string key = GenDynamicName(type, name);
            PropertyGetDelegate handler = GetFromCache(key) as PropertyGetDelegate;

            if (handler == null)
            {
                PropertyInfo property = type.GetProperty(name);
                Type returnType = typeof(object);
                Type[] parameterTypes = new Type[] { typeof(object) };
                DynamicMethod method = new DynamicMethod(key, returnType, parameterTypes, type, true);

                ILGenerator il = method.GetILGenerator();
                il.Emit(OpCodes.Ldarg_0);
                il.EmitCall(OpCodes.Callvirt, property.GetGetMethod(), null);
                if (property.PropertyType.IsValueType)
                    il.Emit(OpCodes.Box, property.PropertyType);
                il.Emit(OpCodes.Ret);

                handler = method.CreateDelegate(typeof(PropertyGetDelegate)) as PropertyGetDelegate;
                if (handler != null) SetToCache(key, handler);
            }

            return (handler != null) ? handler(instance) : null;
        }

        /// <summary>
        /// Sets the property value of the specified type instance.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="instance">The instance.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <param name="index">The index.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static void SetPropertyValue(this Type type, object instance, string name, object value, object[] index)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (instance == null) throw new ArgumentNullException("instance");
            if (name == null) throw new ArgumentNullException("name");

            string key = GenDynamicName(type, name);
            PropertySetDelegate handler = GetFromCache(key) as PropertySetDelegate;

            if (handler == null)
            {
                PropertyInfo property = type.GetProperty(name);
                Type returnType = typeof(void);
                Type[] parameterTypes = new Type[] { typeof(object), typeof(object), typeof(object) };
                DynamicMethod method = new DynamicMethod(key, returnType, parameterTypes, type, true);

                ILGenerator il = method.GetILGenerator();
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldarg_1);
                il.EmitCall(OpCodes.Callvirt, property.GetSetMethod(true), null);
                if (property.PropertyType.IsValueType)
                    il.Emit(OpCodes.Unbox_Any, property.PropertyType);
                il.Emit(OpCodes.Ret);

                handler = method.CreateDelegate(typeof(PropertySetDelegate)) as PropertySetDelegate;
                if (handler != null) SetToCache(key, handler);
            }

            if (handler != null) handler(instance, value);
        }

        /// <summary>
        /// Sets the property value of the specified type instance.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="instance">The instance.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static void SetPropertyValue(this Type type, object instance, string name, object value)
        {
            SetPropertyValue(type, instance, name, value, null);
        }

        #region Utilities /////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Gets from inner cache a generated dynamic method handler.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        static Delegate GetFromCache(string key)
        {
            Delegate handler;
            return Cache.TryGetValue(key, out handler) ? handler : null;
        }

        /// <summary>
        /// Sets into inner cache a generated dynamic method handler.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="handler">The handler.</param>
        static void SetToCache(string key, Delegate handler)
        {
            Cache[key] = handler;
        }

        /// <summary>
        /// Generates an unique name of the dynamic method.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        static string GenDynamicName(Type type, string name)
        {
            return string.Format("Dynamic_{0}_{1}", type.FullName.Replace('.', '_'), name);
        }
        #endregion
    }
}
