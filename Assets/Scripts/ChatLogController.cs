using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ChatLogController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ChatLog;
    [SerializeField] TMP_InputField TextInput;

    public static ChatLogController Instance { get; private set; }
    public string TextLog { get; private set; }

    private void Awake()
    { 
        Instance = this;
        AddText("### Chat Log Started ###");
    }

    public void AddText(string text)
    {
        TextLog = $"{text}\n{TextLog}";
        ChatLog.text = TextLog;
    }

    public void PlayerMessage()
    {
        if (!string.IsNullOrEmpty(TextInput.text))
            AddText($"Player: {TextInput.text}");
        TextInput.text = "";
    }
}
