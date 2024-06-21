using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
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


    private Vector3 agentDirection = Vector3.zero;



    void Start()
    {

        var api = new OpenAIAPI("##ADD KEY HERE##");

        var chat = api.Chat.CreateConversation();
        chat.Model = Model.ChatGPTTurbo;
        chat.RequestParameters.Temperature = 0;

        /// give instruction as System
        string inputMessage = @"You are a commander who gives commands in a dramatic,
          game-entertaining way to agents.
          Agents can be used to catch the player. Here are the legends for the places the agents can look for the player.
          You can only instruct the agents using the legends:\n";

        foreach(var l in MapLabelController.Instance.GetLabels()){
            inputMessage += $"{l}\n";
        }


        // now let's ask it a question

        inputMessage += "Here is an example of a Chatlog:\n";
        inputMessage += ChatLogController.Instance.TextLog;
        inputMessage += "\n---";
        inputMessage += @"give the agents instructions on where to go based on the information of the chatlog.
        Limit yourself to 10 words.";

        Debug.Log(inputMessage);
        chat.AppendSystemMessage(inputMessage);

        addUserInput(chat, ChatLogController.Instance.TextLog);

    }

    async void addUserInput(Conversation chat, String message) {
        chat.AppendUserInput(message);
        string response = await chat.GetResponseFromChatbotAsync();
        Debug.Log(response);
        // agentCommands = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);
        // Debug.Log(agentCommands["agent1"]);
    }    

}
