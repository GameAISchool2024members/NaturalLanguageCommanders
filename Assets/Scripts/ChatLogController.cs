using TMPro;
using UnityEngine;

public class ChatLogController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ChatLog;
    [SerializeField] TMP_InputField TextInput;
    [SerializeField] AudioSource walkieSound;
    //[SerializeField] TextToSpeech textToSpeech;

    public static ChatLogController Instance { get; private set; }
    public string TextLog { get; private set; }

    private void Awake()
    { 
        Instance = this;
        AddText("### Chat Log Started ###");
    }

    public void AddText(string text)
    {
        walkieSound?.Play();
        //textToSpeech?.Speak(text.Split(':', 2)[1]);
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
