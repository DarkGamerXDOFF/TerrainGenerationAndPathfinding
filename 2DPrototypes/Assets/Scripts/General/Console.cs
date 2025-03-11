using UnityEngine;
using TMPro;
public class Console : MonoBehaviour
{
    public static Console I;

    [SerializeField] private GameObject messagePrefab;
    [SerializeField] private Transform messageBox;

    [SerializeField] private Color normalColor;
    [SerializeField] private Color errorColor;

    private void Awake()
    {
        if (I == null)
            I = this;
        else
            Destroy(this);
    }

    public void Log(string message)
    {
        TMP_Text text = CreateMessage();
        text.text = message;
        text.color = normalColor;
    }
    public void LogError(string message)
    {
        TMP_Text text = CreateMessage();
        text.text = message;
        text.color = errorColor;
    }

    private TMP_Text CreateMessage()
    {
        TMP_Text text = Instantiate(messagePrefab, messageBox).GetComponent<TMP_Text>();

        return text;
    }
}
