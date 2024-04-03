// Copyright (c) 2012-2023 FuryLion Group. All Rights Reserved.

using Newtonsoft.Json;

namespace Task3
{
    /// <summary>
    /// Подробная информация о погоде из JSON
    /// </summary>
    [Serializable]
    public class WeatherDataDetails
    {
        /// <summary>
        /// Информация о температуре
        /// </summary>
        [JsonProperty("temp")]
        public float Temperature { get; set; }

        /// <summary>
        /// Информация о температуре (по ощущению)
        /// </summary>
        [JsonProperty("feels_like")]
        public float FeelsLike { get; set; }

        /// <summary>
        /// Информация о влажности
        /// </summary>
        [JsonProperty("humidity")]
        public float Humidity { get; set; }

        /// <summary>
        /// Информация о давлении
        /// </summary>
        [JsonProperty("pressure")]
        public float Pressure { get; set; }

        /// <summary>
        /// Информация о минимальной температуре
        /// </summary>
        [JsonProperty("temp_min")]
        public float MinTemperature { get; set; }

        /// <summary>
        /// Информация о максимальной температуре
        /// </summary>
        [JsonProperty("temp_max")]
        public float MaxTemperature { get; set; }
    }
}