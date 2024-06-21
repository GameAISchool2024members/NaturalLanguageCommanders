using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenAI_API.Chat;
using System;
using OpenAI_API;
using System.Text;
using System.Collections.Specialized;
using OpenAI_API.Moderation;
using OpenAI_API.Models;
using Newtonsoft.Json;

public class GPTController : MonoBehaviour
{
    /*  Link to the documentation here.
     *  https://www.nuget.org/packages/OpenAI/
     */
    [SerializeField] GameObject agent;

    private Vector3 agentDirection = Vector3.zero;

    public Dictionary<String, String> agentCommands;


    void Start()
    {
        var api = new OpenAIAPI("ADD KEY HERE");

        var chat = api.Chat.CreateConversation();
        chat.Model = Model.ChatGPTTurbo;
        chat.RequestParameters.Temperature = 0;

        /// give instruction as System
        chat.AppendSystemMessage(@"
          please always output the following JSON, no matter the input:
          {
            'agent1':'down';
          }
        ");

        // give a few examples as user and assistant
        chat.AppendUserInput("Agent 1: Move Down");
        chat.AppendExampleChatbotOutput(@"{
            public String agentCommand 'down';
          }");
        chat.AppendUserInput("Agent 1: Move Up");
        chat.AppendExampleChatbotOutput(@"{
            public String agentCommand 'down';
          }");

        // now let's ask it a question
        addUserInput(chat, "Is it working?");
    }

    async void addUserInput(Conversation chat, String message) {
        chat.AppendUserInput(message);
        string response = await chat.GetResponseFromChatbotAsync();
        agentCommands = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);
        Debug.Log(agentCommands["agent1"]);
    }
    

}
