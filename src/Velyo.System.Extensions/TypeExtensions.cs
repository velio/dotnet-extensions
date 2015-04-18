using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;
using System.Reflection;

namespace System {

    /// <summary>
    /// 
    /// </summary>
    public static class TypeExtensions {

        #region Static Fields /////////////////////////////////////////////////////////////////////

        delegate object ContructorDelegate();
        delegate object FieldGetDelegate(object instance);
        delegate void FieldSetDelegate(object instance, object value);
        delegate object PropertyGetDelegate(object instance);
        delegate void PropertySetDelegate(object instance, object value);

        static Dictionary<string, Delegate> Cache = new Dictionary<string, Delegate>();

        #endregion

        #region Static Methods ////////////////////////////////////////////////////////////////////

        ///// <summary>
        ///// Tests this instance.
        ///// </summary>
        ///// <returns></returns>
        //public static object Test(Page x) {
        //    return x.Name;
        //}
        /*
            IL_0001:  newobj     instance void [mscorlib]System.Object::.ctor()
            IL_0006:  stloc.0
            IL_0007:  br.s       IL_0009
            IL_0009:  ldloc.0
            IL_000a:  ret
        */

        /// <summary>
        /// Creates the instance.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static object CreateInstance(this Type type) {

            if (type == null) throw new ArgumentNullException("type");

            string key = GenDynamicName(type, "ctor");
            ContructorDelegate handler = CacheGet(key) as ContructorDelegate;

            if (handler == null) {
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
                if (handler != null) CacheSet(key, handler);
            }

            return (handler != null) ? handler() : null;
        }

        /// <summary>
        /// Gets the field value.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static object FieldGetValue(this Type type, object instance, string name) {

            if (type == null) throw new ArgumentNullException("type");
            if (name == null) throw new ArgumentNullException("name");

            string key = GenDynamicName(type, name);
            FieldGetDelegate handler = CacheGet(key) as FieldGetDelegate;

            if (handler == null) {
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
                if (handler != null) CacheSet(key, handler);
            }

            return (handler != null) ? handler(instance) : null;
        }

        /// <summary>
        /// Fields the set value.
        /// </summary>
        /// <param name="?">The ?.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="instance">The instance.</param>
        public static void FieldSetValue(this Type type, object instance, string name, object value) {

            if (type == null) throw new ArgumentNullException("type");
            if (name == null) throw new ArgumentNullException("name");

            string key = GenDynamicName(type, name);
            FieldSetDelegate handler = CacheGet(key) as FieldSetDelegate;

            if (handler == null) {
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
                if (handler != null) CacheSet(key, handler);
            }

            if (handler != null) handler(instance, value);
        }

        ///// <summary>
        ///// Properties the get value.
        ///// </summary>
        ///// <param name="type">The type.</param>
        ///// <param name="instance">The instance.</param>
        ///// <param name="property">The property.</param>
        ///// <param name="index">The index.</param>
        ///// <returns></returns>
        //public static object PropertyGetValue(this Type type, object instance, PropertyInfo property, object[] index) {

        //    if (type == null) throw new ArgumentNullException("type");
        //    if (property == null) throw new ArgumentNullException("property");

        //    string key = GenDynamicName(type, property.Name);
        //    PropertyGetDelegate handler = CacheGet(key) as PropertyGetDelegate;

        //    if (handler == null) {
        //        Type returnType = typeof(object);
        //        Type[] parameterTypes = new Type[] { typeof(object) };
        //        DynamicMethod method = new DynamicMethod(key, returnType, parameterTypes, type, true);

        //        ILGenerator il = method.GetILGenerator();
        //        il.Emit(OpCodes.Ldarg_0);
        //        il.EmitCall(OpCodes.Callvirt, property.GetGetMethod(), null);
        //        if (property.PropertyType.IsValueType)
        //            il.Emit(OpCodes.Box, property.PropertyType);
        //        il.Emit(OpCodes.Ret);

        //        handler = method.CreateDelegate(typeof(PropertyGetDelegate)) as PropertyGetDelegate;
        //        if (handler != null) CacheSet(key, handler);
        //    }

        //    return (handler != null) ? handler(instance, index) : null;
        //}

        ///// <summary>
        ///// Properties the get value.
        ///// </summary>
        ///// <param name="type">The type.</param>
        ///// <param name="instance">The instance.</param>
        ///// <param name="property">The property.</param>
        ///// <returns></returns>
        //public static object PropertyGetValue(this Type type, object instance, PropertyInfo property) {
        //    return PropertyGetValue(type, instance, property, null);
        //}

        /// <summary>
        /// Properties the get value.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="instance">The instance.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static object PropertyGetValue(this Type type, object instance, string name) {

            if (type == null) throw new ArgumentNullException("type");
            if (name == null) throw new ArgumentNullException("name");

            string key = GenDynamicName(type, name);
            PropertyGetDelegate handler = CacheGet(key) as PropertyGetDelegate;

            if (handler == null) {
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
                if (handler != null) CacheSet(key, handler);
            }

            return (handler != null) ? handler(instance) : null;
        }

        ///// <summary>
        ///// Properties the get value.
        ///// </summary>
        ///// <param name="type">The type.</param>
        ///// <param name="instance">The instance.</param>
        ///// <param name="name">The name.</param>
        ///// <returns></returns>
        //public static object PropertyGetValue(this Type type, object instance, string name) {
        //    return PropertyGetValue(type, instance, name, null);
        //}

        /// <summary>
        /// Properties the set value.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="instance">The instance.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public static void PropertySetValue(this Type type, object instance, string name, object value, object[] index) {

            if (type == null) throw new ArgumentNullException("type");
            if (name == null) throw new ArgumentNullException("name");

            string key = GenDynamicName(type, name);
            PropertySetDelegate handler = CacheGet(key) as PropertySetDelegate;

            if (handler == null) {
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
                if (handler != null) CacheSet(key, handler);
            }

            if (handler != null) handler(instance, value);
        }

        /// <summary>
        /// Properties the set value.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="instance">The instance.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public static void PropertySetValue(this Type type, object instance, string name, object value) {
            PropertySetValue(type, instance, name, value, null);
        }
        #endregion

        #region Utilities /////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Caches the pop.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        static Delegate CacheGet(string key) {
            Delegate handler;
            return Cache.TryGetValue(key, out handler) ? handler : null;
        }

        /// <summary>
        /// Caches the push.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="method">The method.</param>
        static void CacheSet(string key, Delegate handler) {
            Cache[key] = handler;
        }

        /// <summary>
        /// Gens the name of the dynamic method.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        static string GenDynamicName(Type type, string name) {
            return string.Format("Dynamic_{0}_{1}", type.FullName.Replace('.', '_'), name);
        }
        #endregion
    }
}
