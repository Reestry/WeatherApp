// Copyright (c) 2012-2023 FuryLion Group. All Rights Reserved.

using System.Net;
using Newtonsoft.Json;

namespace Task3
{
    /// <summary>
    /// Точка входа в программу 
    /// </summary>
    public static class WeatherApp
    {
        private const string WeatherUrl =
            "http://api.openweathermap.org/data/2.5/weather?units=metric&appid={0}&lang=ru&q={1}";

        private const string ForecastUrl =
            "http://api.openweathermap.org/data/2.5/forecast?units=metric&cnt=0&appid={0}&lang=ru&q={1}";

        private const string ApiKey = "318434a39cc33916b8b135a49bb5ce12";

        /// <summary>
        /// Получение информации о погоде на один день
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="Exception"></exception>
        private static async Task<WeatherData> GetWeatherDataAsync(string? city)
        {
            try
            {
                using var client = new HttpClient();
                var url = string.Format(WeatherUrl, ApiKey, city);

                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var forecastData = JsonConvert.DeserializeObject<WeatherData>(json);
                    if (forecastData != null)
                        return forecastData;
                }

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new ArgumentException($"Город {city} не найден.");
                }
                else
                {
                    throw new Exception($"Не удалось получить данные о погоде. Код состояния: {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Не удалось подключиться к метеослужбе.", ex);
            }
            catch (JsonException ex)
            {
                throw new Exception("Не удалось десериализовать данные о погоде.", ex);
            }
        }

        /// <summary>
        /// Получение информации о погоде на 5 дней
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="Exception"></exception>
        private static async Task<List<WeatherData>> GetWeatherForecastAsync(string? city)
        {
            try
            {
                using var client = new HttpClient();
                var url = string.Format(ForecastUrl, ApiKey, city);

                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var forecastData = JsonConvert.DeserializeObject<ForecastData>(json);
                    return forecastData!.ForecastDetailsList;
                }

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new ArgumentException($"Город {city} не найден.");
                }
                else
                {
                    throw new Exception($"Не удалось получить данные о погоде. Код состояния: {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Не удалось подключиться к метеослужбе.", ex);
            }
            catch (JsonException ex)
            {
                throw new Exception("Не удалось десериализовать данные о погоде.", ex);
            }
        }

        /// <summary>
        /// Интерфейс программы
        /// </summary>
        public static async Task Run()
        {
            var weatherHistory = new ObservableDictionary<string, WeatherData>();

            weatherHistory.Added += Added;

            while (true)
            {
                Console.WriteLine("1 - Прогноз погоды на сегодня \n" +
                                  "2 - Прогноз погоды на 5 дней \n" +
                                  "3 - История погоды");
                var menuIndex = Console.ReadLine();
                string? cityName;
                switch (menuIndex)
                {
                case "1":
                    DisplayCities();
                    menuIndex = Console.ReadLine();

                    if (menuIndex != null && int.Parse(menuIndex) <= 4 &&
                        Enum.TryParse<Cities>(menuIndex, out var city))
                    {
                        cityName = city.ToString();
                    }
                    else
                    {
                        Console.WriteLine("Введите свой город:");
                        cityName = Console.ReadLine();
                    }

                    try
                    {
                        var weatherData = await GetWeatherDataAsync(cityName);
                        if (cityName != null)
                            weatherHistory.Add(cityName, weatherData);

                        Storage.Save(weatherHistory, "save");

                        PrintWeather(cityName, weatherData);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка: {ex.Message}");
                    }

                    break;
                case "2":
                    DisplayCities();
                    menuIndex = Console.ReadLine();

                    if (menuIndex != null && int.Parse(menuIndex) <= 4 && Enum.TryParse(menuIndex, out city))
                    {
                        cityName = city.ToString().ToLower();
                    }
                    else
                    {
                        Console.WriteLine("Введите свой город:");
                        cityName = Console.ReadLine();
                    }

                    try
                    {
                        var weatherData =
                            await GetWeatherForecastAsync(cityName);
                        PrintForecast(cityName, weatherData);

                        Console.WriteLine();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка: {ex.Message}");
                    }

                    break;

                case "3":
                    DisplayCities();

                    cityName = Console.ReadLine();

                    if (cityName != null && int.Parse(cityName) <= 4 && Enum.TryParse(cityName, out city))
                    {
                        cityName = city.ToString();
                    }

                    if (File.Exists("save"))
                    {
                        var weatherHistoryData = Storage.Load<ObservableDictionary<string, WeatherData>>("save");

                        if (weatherHistoryData.ContainsKey(cityName))
                        {
                            Console.WriteLine("Прошлый прогноз:");
                            Console.WriteLine("Город: " + weatherHistoryData[cityName].CityName);
                            Console.WriteLine("Описание: " +
                                              weatherHistoryData[cityName].WeatherDescription[0].Description);
                            Console.WriteLine("Температура: " +
                                              weatherHistoryData[cityName].WeatherDataDetails.Temperature);
                            Console.WriteLine("Давление: " +
                                              weatherHistoryData[cityName].WeatherDataDetails.Pressure);
                        }
                        else
                        {
                            Console.WriteLine("Такого города не существует в истории запросов.");
                        }
                    }

                    break;
                }
            }

            void Added()
            {
                Storage.Save(weatherHistory, "save");
            }
        }

        /// <summary>
        /// Вывод информации о погоде на 5 дней
        /// </summary>
        /// <param name="cityName"></param>
        /// <param name="weatherData"></param>
        private static void PrintForecast(string? cityName, List<WeatherData> weatherData)
        {
            var currentHour = weatherData[0].Date;
            foreach (var forecastDetails in weatherData)
            {
                if (forecastDetails.Date.Hour != currentHour.Hour)
                    continue;
                Console.WriteLine();
                Console.WriteLine($"Название города: {cityName}");
                Console.WriteLine("-------------------------------");
                Console.WriteLine($"Дата: {forecastDetails.Date}");
                Console.WriteLine($"Температура: {forecastDetails.WeatherDataDetails.Temperature}С°");
                Console.WriteLine(
                    $"Минимальная температура: {forecastDetails.WeatherDataDetails.MinTemperature}С°");
                Console.WriteLine($"Влажность: {forecastDetails.WeatherDataDetails.Humidity}%");
                Console.WriteLine($"Давление: {forecastDetails.WeatherDataDetails.Pressure}мм");
                Console.WriteLine($"Описание: {forecastDetails.WeatherDescription[0].Description}");
            }
        }

        /// <summary>
        /// Вывод информации о погоде на один день
        /// </summary>
        /// <param name="cityName"></param>
        /// <param name="weatherData"></param>
        private static void PrintWeather(string? cityName, WeatherData weatherData)
        {
            Console.WriteLine();
            Console.WriteLine($"Название города: {cityName}");
            Console.WriteLine("-------------------------------");
            Console.WriteLine($"Температура: {weatherData.WeatherDataDetails.Temperature}С°");
            Console.WriteLine(
                $"Минимальная температура: {weatherData.WeatherDataDetails.MinTemperature}С°");
            Console.WriteLine($"Влажность: {weatherData.WeatherDataDetails.Humidity}%");
            Console.WriteLine($"Давление: {weatherData.WeatherDataDetails.Pressure}мм");
            Console.WriteLine($"Описание: {weatherData.WeatherDescription[0].Description}");
        }

        /// <summary>
        /// Вывод списка городов
        /// </summary>
        private static void DisplayCities()
        {
            Console.Clear();
            Console.WriteLine("Выберите город \n" +
                              "0 - Москва\n" +
                              "1 - Минск\n" +
                              "2 - Новополоцк\n" +
                              "3 - Варшава\n" +
                              "4 - Прага \n" +
                              "5 - Свой город");
        }
    }
}