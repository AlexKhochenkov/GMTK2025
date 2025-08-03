using System.Collections.Generic;
using System.Threading.Tasks;
//using UnityEditor.Build.Content;
using UnityEngine;
using WebFetcher;

public class Lideboard : MonoBehaviour
{

    [SerializeField] private RecordEntity _recordPrefab;
    [SerializeField] private Transform _content;
    private List<RecordEntity> currentRecords = new();


    async void Start()
    {
        await UpdateLideboardAsync();
    }

    public void UpdateLideboard()
    {
        var records = Task.Run(() => WebFetcher.WebFetcher.GetRecords(10)).Result;
        DrawRecords(records);
    }

    public async Task UpdateLideboardAsync()
    {

        var records = await WebFetcher.WebFetcher.GetRecords(10);
        DrawRecords(records);
    }

    private void DrawRecords(List<Record> records)
    {
        foreach (var r in currentRecords)
        {
            Destroy(r.gameObject);
        }
        currentRecords.Clear();

        foreach (var r in records)
        {
            Debug.Log(r);
            var obj = Instantiate(_recordPrefab, _content);
            var name = PlayerPrefs.GetString("PlayerName");
            obj.SetValues(r.nickname, r.best_time, r.runs, name is not null && name == r.nickname);
            currentRecords.Add(obj);
        }
    }
}
