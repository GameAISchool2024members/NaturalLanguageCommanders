using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
// using DotNetEnv;
using UnityEngine;
using OpenAI_API.Chat;
using System;
using OpenAI_API;
using System.Text;
using System.Collections.Specialized;
using OpenAI_API.Moderation;
using OpenAI_API.Models;

public class GPTController : MonoBehaviour
{
    /*  Link to the documentation here.
     *  https://www.nuget.org/packages/OpenAI/
     */
    [SerializeField] GameObject agent;
    [SerializeField] AgentPrompts agentPrompt;


    private Vector3 agentDirection = Vector3.zero;



    void Start()
    {   

        // Env.Load(".env");
        // string privateKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
        string privateKey = "";
        var api = new OpenAIAPI(privateKey);

        var chat = api.Chat.CreateConversation();
        chat.Model = Model.ChatGPTTurbo;
        chat.RequestParameters.Temperature = 0;

        /// give instruction as System
        string inputMessage = @$"You are a {agentPrompt.Name} who {agentPrompt.Description}.
        

        foreach(var l in MapLabelController.Instance.GetLabels()){
            inputMessage += $"{l}\n";
        }

        Debug.Log(inputMessage);

        // now let's ask it a question

        inputMessage += "You will be given a Chatlog:\n";
        // inputMessage += ChatLogController.Instance.TextLog;
        inputMessage += "\n---";
        inputMessage += @"give the agents instructions on where to go based on the information of the chatlog.
        Limit yourself to 10 words.";
        chat.AppendSystemMessage(inputMessage);

        addUserInput(chat, ChatLogController.Instance.TextLog);

    }

    async void addUserInput(Conversation chat, String message) {
        chat.AppendUserInput(message);
        string response = await chat.GetResponseFromChatbotAsync();
        Debug.Log(response);
        ChatLogController.Instance.AddText(response);
        // agentCommands = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);
        // Debug.Log(agentCommands["agent1"]);
    }    

}
