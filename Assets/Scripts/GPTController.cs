using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenAI_API.Chat;
using System;
using OpenAI_API;

public class GPTController : MonoBehaviour
{
    /*  Link to the documentation here.
     *  https://www.nuget.org/packages/OpenAI/
     */
    void Start()
    {
        var api = new OpenAI_API.OpenAIAPI("YOUR_API_KEY");
        var result = api.Chat.CreateChatCompletionAsync("Hello world!");
        Console.WriteLine(result);
    }
}
