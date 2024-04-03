// Copyright (c) 2012-2023 FuryLion Group. All Rights Reserved.

using System.Runtime.Serialization.Formatters.Binary;

namespace Task3
{
    /// <summary>
    /// Расширение для типа object
    /// </summary>
    public static class ObjectExtension
    {
        /// <summary>
        /// Сериализует объект в поток байтов
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Obsolete("Obsolete")]
        public static byte[] GetBytes(this object data)
        {
            using var memoryStream = new MemoryStream();
            var formatter = new BinaryFormatter();
            formatter.Serialize(memoryStream, data);
            return memoryStream.ToArray();
        }

        /// <summary>
        /// Десериализует массив байтов
        /// </summary>
        /// <param name="bytes"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [Obsolete("Obsolete")]
        public static T GetObject<T>(this byte[] bytes)
        {
            using var memoryStream = new MemoryStream(bytes);
            var formatter = new BinaryFormatter();
            return (T) formatter.Deserialize(memoryStream);
        }
    }
}