// Copyright (c) 2012-2023 FuryLion Group. All Rights Reserved.

using Newtonsoft.Json;

namespace Task3
{
    /// <summary>
    /// Информация о погоде 
    /// </summary>
    [Serializable]
    public class WeatherData
    {
        /// <summary>
        /// Массив информации с подробном описанием погоды
        /// </summary>
        [JsonProperty("main")]
        public WeatherDataDetails WeatherDataDetails { get; set; }

        /// <summary>
        /// Название города
        /// </summary>
        [JsonProperty("name")]
        public string? CityName { get; set; }

        /// <summary>
        /// Описание типа погоды
        /// </summary>
        [JsonProperty("weather")]
        public WeatherDescription[] WeatherDescription { get; set; }

        /// <summary>
        /// Описание даты
        /// </summary>
        [JsonProperty("dt_txt")]
        public DateTime Date { get; set; }
    }
}