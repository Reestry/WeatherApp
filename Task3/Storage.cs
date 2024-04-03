// Copyright (c) 2012-2023 FuryLion Group. All Rights Reserved.

namespace Task3
{
    /// <summary>
    /// Сохранение и загрузка данных
    /// </summary>
    public static class Storage
    {
        /// <summary>
        /// Сохранение массива данных
        /// </summary>
        /// <param name="data"></param>
        /// <param name="filePath"></param>
        /// <typeparam name="T"></typeparam>
        public static void Save<T>(T data, string filePath)
        {
            var bytes = data.GetBytes();
            File.WriteAllBytes(filePath, bytes);
        }

        /// <summary>
        /// Загрузка массива данных
        /// </summary>
        /// <param name="filePath"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Load<T>(string filePath)
        {
            var bytes = File.ReadAllBytes(filePath);
            return bytes.GetObject<T>();
        }
    }
}