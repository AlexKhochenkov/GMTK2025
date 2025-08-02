using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace WebFetcher
{
    [Serializable]
    class Record
    {
        public string nickname;

        public float best_time;

        public int runs;

        public float total_time;

        public override string ToString()
        {
            return $"nickname: {nickname}\nbest_run: {best_time}\n runs: {runs}\ntotal_time: {total_time}";
        }
    }

    // Обертка для десериализации массива
    [System.Serializable]
    class RecordListWrapper
    {
        public Record[] records;
    }
    
    static class WebFetcher
    {
        const string ipString = "62.84.121.113";
        const int port = 8000;
        static readonly HttpClient client = new HttpClient();
        static readonly string baseUrl = $"http://{ipString}:{port}";

        // Генерация контрольной суммы (MD5(nickname:time))
        private static string GenerateChecksum(string nickname, float time)
        {
            // Форматируем время с 2 знаками после запятой
            string data = $"{nickname}:{Math.Round(time + 2435.4558, 4).ToString().Replace(",", ".")}";

            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(data);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }

        public static async Task<List<Record>> GetRecords(int topN)
        {
            try
            {
                string url = $"{baseUrl}/top_records?n={topN}";
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                Debug.Log(responseBody);
                string wrappedJson = $"{{\"records\": {responseBody}}}";
                return JsonUtility.FromJson<RecordListWrapper>(wrappedJson).records.ToList();
            }
            catch (HttpRequestException e)
            {
                Debug.LogError($"Error getting records: {e.Message}");
                return new List<Record>();
            }
        }

        public static async Task<Record> GetRecordByName(string nickname)
        {
            try
            {
                string url = $"{baseUrl}/record/{Uri.EscapeDataString(nickname)}";
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.StatusCode == HttpStatusCode.NotFound)
                    return null;

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonUtility.FromJson<Record>(responseBody);
            }
            catch (HttpRequestException e)
            {
                Debug.LogError($"Error getting record: {e.Message}");
                return null;
            }
        }

        public static async Task<bool> AddRecord(string nickname, float time)
        {
            try
            {
                string checksum = GenerateChecksum(nickname, time);
                string url = $"{baseUrl}/add_record?nickname={Uri.EscapeDataString(nickname)}&time={time.ToString().Replace(",", ".")}&checksum={checksum}";

                HttpResponseMessage response = await client.PostAsync(url, null);
                response.EnsureSuccessStatusCode();

                return true;
            }
            catch (HttpRequestException e)
            {
                Debug.LogError($"Error adding record: {e.Message}");
                return false;
            }
        }
    }
}