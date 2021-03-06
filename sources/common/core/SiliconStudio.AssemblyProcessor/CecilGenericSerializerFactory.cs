// Copyright (c) 2014-2017 Silicon Studio Corp. All rights reserved. (https://www.siliconstudio.co.jp)
// See LICENSE.md for full license information.
using System;
using System.Linq;
using Mono.Cecil;

namespace SiliconStudio.AssemblyProcessor
{
    /// <summary>
    /// Gives the specified serializer type constructed with generic arguments of the serialized type.
    /// As an example, this instance with <see cref="ListSerializer{}"/> will give <see cref="ListSerializer{int}"/> from <see cref="int"/>.
    /// </summary>
    public class CecilGenericSerializerFactory : ICecilSerializerFactory
    {
        private readonly Type genericSerializableType;
        private readonly bool checkInterfaces;

        protected Func<TypeReference, TypeReference> CreateSerializer { get; set; }

        private CecilGenericSerializerFactory(Type genericSerializableType, Func<TypeReference, TypeReference> createSerializer)
        {
            this.genericSerializableType = genericSerializableType;
            CreateSerializer = createSerializer;
            checkInterfaces = genericSerializableType.IsInterface;
        }

        public CecilGenericSerializerFactory(Type genericSerializableType, TypeReference genericSerializerType)
            : this(genericSerializableType, (type => genericSerializerType.MakeGenericType(((GenericInstanceType)type).GenericArguments.ToArray())))
        {
            if (genericSerializerType == null)
                throw new ArgumentNullException("genericSerializerType");
            if (genericSerializableType == null)
                throw new ArgumentNullException("genericSerializableType");
        }

        #region IDataSerializerFactory Members

        public virtual TypeReference GetSerializer(TypeReference objectType)
        {
            // Check if objectType matches genericSerializableType.
            // Note: Not perfectly valid but hopefully it should be fast enough.
            if (objectType.IsGenericInstance && checkInterfaces)
            {
                if (objectType.GetElementType().Resolve().Interfaces.Any(x => x.InterfaceType.IsGenericInstance && x.InterfaceType.GetElementType().FullName == genericSerializableType.FullName))
                    return CreateSerializer(objectType);
            }
            if (objectType.IsGenericInstance && objectType.GetElementType().FullName == genericSerializableType.FullName)
                return CreateSerializer(objectType);

            return null;
        }

        #endregion
    }
}
