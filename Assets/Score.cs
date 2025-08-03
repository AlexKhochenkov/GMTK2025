using System;
using System.Globalization;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class Score : MonoBehaviour
{
    private float value;
    [SerializeField] private TMP_Text score;
    [SerializeField] private TMP_Text finalScore;
    [SerializeField] private TMP_Text bestScore;
    [SerializeField] private Lideboard lideboard;
    [SerializeField] private TMP_Text looping;

    public void AddScore(float addValue)
    {
        value += addValue;
        SetUiValue();
    }

    public void ResetScore()
    {
        value = 0;
        SetUiValue();
    }
    public void SetBestScore()
    {
        var name = PlayerPrefs.GetString("PlayerName");
        var result = Task.Run(() => WebFetcher.WebFetcher.AddRecord(name, value)).Result;
        var bestResult = Task.Run(() => WebFetcher.WebFetcher.GetRecordByName(name)).Result;
        var best = 0f;
        if (bestResult is not null)
            best = bestResult.best_time;

        lideboard.UpdateLideboard();

        bestScore.text = Math.Round(best,2).ToString(CultureInfo.InvariantCulture);

        looping.text = $"You've been looping for {bestResult.total_time} in {bestResult.runs} runs";
    }

    private void SetUiValue()
    {
        score.text = Math.Round(value,2).ToString(CultureInfo.InvariantCulture);
        finalScore.text =  Math.Round(value,2).ToString(CultureInfo.InvariantCulture);
    }
}
