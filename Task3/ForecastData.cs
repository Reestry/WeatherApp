// Copyright (c) 2012-2023 FuryLion Group. All Rights Reserved.

using Newtonsoft.Json;

namespace Task3
{
    /// <summary>
    /// Информация о погоде за 5 дней 
    /// </summary>
    [Serializable]
    public class ForecastData
    {
        /// <summary>
        /// Вывод информации за 5 дней о погоде
        /// </summary>
        [JsonProperty("list")]
        public List<WeatherData> ForecastDetailsList { get; set; }
    }
}