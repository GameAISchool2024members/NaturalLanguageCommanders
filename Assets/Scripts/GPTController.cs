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

public class GPTController : MonoBehaviour
{
    /*  Link to the documentation here.
     *  https://www.nuget.org/packages/OpenAI/
     */
    void Start()
    {
        var api = new OpenAIAPI("");

        var chat = api.Chat.CreateConversation();
        chat.Model = Model.ChatGPTTurbo;
        chat.RequestParameters.Temperature = 0;

        /// give instruction as System
        chat.AppendSystemMessage("You are a teacher who helps children understand if things are animals or not.  If the user tells you an animal, you say \"yes\".  If the user tells you something that is not an animal, you say \"no\".  You only ever respond with \"yes\" or \"no\".  You do not say anything else.");

        // give a few examples as user and assistant
        chat.AppendUserInput("Is this an animal? Cat");
        chat.AppendExampleChatbotOutput("Yes");
        chat.AppendUserInput("Is this an animal? House");
        chat.AppendExampleChatbotOutput("No");

        // now let's ask it a question
        addUserInput(chat, "Is this an animal? Dog");
        addUserInput(chat, "Is this an animal? Chair");
    }

    async void addUserInput(Conversation chat, String message) {
        chat.AppendUserInput("Is this an animal? Dog");
        string response = await chat.GetResponseFromChatbotAsync();
        // Debug.Log(response);
    }
}
