using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnterPlayerName : MonoBehaviour
{
    [SerializeField] private TMP_InputField playerNameInput;
    [SerializeField] private TMP_Text fiveLettersEntry;
    [SerializeField] private GameObject inGameInterface;
    [SerializeField] private TMP_Text playerName;

    private void Start()
    {
        // playerNameInput = gameObject.GetComponent<InputField>();
        var name = PlayerPrefs.GetString("PlayerName");
        if (name is not null && name != string.Empty)
        {
            inGameInterface.SetActive(true);
            playerName.text = name;
            Hide();
        }
    }

    public void Enter()
    {
        var name = playerNameInput.text;
        if (name.Length < 5)
        {
            fiveLettersEntry.color = Color.red;
            return;
        }
        inGameInterface.SetActive(true);
        playerName.text = name;
        PlayerPrefs.SetString("PlayerName", name);
        Hide();
    }

    public void Reset()
    {
        PlayerPrefs.DeleteKey("PlayerName");
        fiveLettersEntry.color = Color.yellow;
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
