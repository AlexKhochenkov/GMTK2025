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
        _best_time.text = time.ToString();
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
