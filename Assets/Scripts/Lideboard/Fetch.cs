using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

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
            return $"nickname: {nickname}\nbest_run: {best_time}\nruns: {runs}\ntotal_time: {total_time}";
        }
    }


    [Serializable]

    class RecordListWrapper
    {
        public Record[] records;
    }

    static class WebFetcher
    {
        const string ipString = "62.84.121.113";
        const int port = 8000;
        static readonly string baseUrl = $"http://{ipString}:{port}";

        private static string GenerateChecksum(string nickname, float time)
        {
            string data = $"{nickname}:{Math.Round(time + 2435.4558, 4).ToString().Replace(",", ".")}";
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(data);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes) sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }

        public static async Task<List<Record>> GetRecords(int topN)
        {
            string url = $"{baseUrl}/top_records?n={topN}";
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                await request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Error getting records: {request.error}");
                    return new List<Record>();
                }

                string responseBody = request.downloadHandler.text;
                Debug.Log(responseBody);
                string wrappedJson = $"{{\"records\": {responseBody}}}";
                return JsonUtility.FromJson<RecordListWrapper>(wrappedJson).records.ToList();
            }
        }

        public static async Task<Record> GetRecordByName(string nickname)
        {
            string url = $"{baseUrl}/record/{UnityWebRequest.EscapeURL(nickname)}";
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                await request.SendWebRequest();

                if (request.responseCode == 404)
                    return null;

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Error getting record: {request.error}");
                    return null;
                }

                string responseBody = request.downloadHandler.text;
                return JsonUtility.FromJson<Record>(responseBody);
            }
        }

        public static async Task<bool> AddRecord(string nickname, float time)
        {
            string checksum = GenerateChecksum(nickname, time);
            string url = $"{baseUrl}/add_record?nickname={UnityWebRequest.EscapeURL(nickname)}" +
                         $"&time={time.ToString().Replace(",", ".")}" +
                         $"&checksum={checksum}";

            using (UnityWebRequest request = UnityWebRequest.PostWwwForm(url, ""))
            {
                await request.SendWebRequest();


                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Error adding record: {request.error}");
                    return false;
                }
                return true;
            }
        }
    }
}