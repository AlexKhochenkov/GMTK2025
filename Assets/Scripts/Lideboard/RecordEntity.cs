using TMPro;
using UnityEngine;

public class RecordEntity : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nickname;
    [SerializeField] private TextMeshProUGUI _best_time;
    [SerializeField] private TextMeshProUGUI _runs;

    public void SetValues(string nickname, float time, int runs, bool itsMe = false)
    {
        _nickname.text = nickname;
        if (time > 60)
            _best_time.text = $"{(int)(time / 60)}m {(int)(time % 60)}s";
        else
        {
            _best_time.text = $"{(int)(time % 60)}s";
        }
        _runs.text = runs.ToString();
        if (itsMe)
        {
            _nickname.color = Color.yellow;
        }
        else
        {
            _nickname.color = Color.white;
        }
    }
}
