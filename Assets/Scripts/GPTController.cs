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
    [SerializeField] AgentPrompts agentPrompts;

    private Vector3 agentDirection = Vector3.zero;

    public void RoundTick()
    {   

        // Env.Load(".env");
        // string privateKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
        string privateKey = "";
        var api = new OpenAIAPI(privateKey);

        var chat = api.Chat.CreateConversation();
        chat.Model = Model.ChatGPTTurbo;
        chat.RequestParameters.Temperature = 0;

        /// give instruction as System
        string inputMessage = @$"You are {agentPrompts.name}. {agentPrompts.Description}
          Here are the legends for the places the agents can look for the player. You can only instruct the agents using the legends:\n";

        foreach(var l in MapLabelController.Instance.GetLabels()){
            inputMessage += $"{l}\n";
        }

        // Debug.Log(inputMessage);
         
        // now let's ask it a question

        inputMessage += "Here is an example of a Chatlog:\n";
        inputMessage += ChatLogController.Instance.TextLog;
        inputMessage += "\n---";
        inputMessage += agentPrompts.Instructions;
        inputMessage += @$"Limit yourself to 10 words. 
                        Youre response should be strickly formattet as following: 
                        <Legend>|<You're name>:<Walkie talkie chatter>
                        Here are an example of a correct output:
                        Wood 1|{agentPrompts.name}: Enemy spottet search Wood 1!";
        chat.AppendSystemMessage(inputMessage);

        this.addUserInput(chat, inputMessage);
    }

    async void addUserInput(Conversation chat, String message) {
        chat.AppendUserInput(message);
        string response = await chat.GetResponseFromChatbotAsync();
        Debug.Log(response);
        var messages = response.Split('|');
        ChatLogController.Instance.AddText(messages[1]);
        var target = MapLabelController.Instance.ObjectByLabel(messages[0]);
        if (agent != null)
        {
            agent.GetComponent<AStarSearch>().Go(target);

        }

        // agentCommands = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);
        // Debug.Log(agentCommands["agent1"]);
    }    

}
