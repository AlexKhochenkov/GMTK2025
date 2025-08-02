using System.Threading.Tasks;
using UnityEditor.Build.Content;
using UnityEngine;
using WebFetcher;

public class Lideboard : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private RecordEntity _recordPrefab;
    [SerializeField] private Transform _content;


    async void Start()
    {
        await UpdateLideboard();
    }

    public async Task UpdateLideboard()
    {
        
        var records = await WebFetcher.WebFetcher.GetRecords(10);
        foreach (var r in records)
        {
            Debug.Log(r);
            var obj = Instantiate(_recordPrefab, _content);
            obj.SetValues(r.nickname, r.best_time, r.runs);
        }
    }
}
