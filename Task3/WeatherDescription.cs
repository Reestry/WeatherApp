// Copyright (c) 2012-2023 FuryLion Group. All Rights Reserved.

using Newtonsoft.Json;

namespace Task3
{
    /// <summary>
    /// Подробная информация о погоде 
    /// </summary>
    [Serializable]
    public class WeatherDescription
    {
        /// <summary>
        /// Описание типа погоды
        /// </summary>
        [JsonProperty("description")]
        public string? Description { get; set; }
    }
}