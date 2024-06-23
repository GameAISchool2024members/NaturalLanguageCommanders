using UnityEngine;
using OpenAI_API.Chat;
using System;
using OpenAI_API;
using OpenAI_API.Models;

public class GPTController : MonoBehaviour
{
    /*  Link to the documentation here.
     *  https://www.nuget.org/packages/OpenAI/
     */
    [SerializeField] GameObject agent;
    [SerializeField] AgentPrompts agentPrompts;
    [SerializeField] Screendumper AIVision;
    [SerializeField] Tilemap2Text textView;
    public bool PlayerSpottet = false;

    private Vector3 agentDirection = Vector3.zero;

    public void RoundTick()
    {

        // Env.Load(".env");
        // string privateKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
        string privateKey = "";
        //string privateKey = "";
        var api = new OpenAIAPI(privateKey);

        var chat = api.Chat.CreateConversation();
        chat.Model = Model.ChatGPTTurbo;
        //chat.Model = new Model("gpt-4o") { OwnedBy = "openai" };
        chat.RequestParameters.Temperature = 0;

        /// give instruction as System
        string inputMessage = @$"You are {agentPrompts.name}. {agentPrompts.Description}
          Here are the legends for the places the agents can look for the player. You can only instruct the agents using the legends:\n";

        foreach(var l in MapLabelController.Instance.GetLabels()){
            inputMessage += $"{l}\n";
        }

        // Debug.Log(inputMessage);
         
        // now let's ask it a question

        inputMessage += "Here is an example of a Chatlog, you pay extra attention to the players messages (and respond to them):\n";
        inputMessage += ChatLogController.Instance.TextLog;
        if (textView != null)
        {
            inputMessage += $"\n---\n Here's a representation of the map:\n {textView.ConvertTilemap()}";
        }
        inputMessage += "\n---";
        inputMessage += agentPrompts.Instructions;
        if (agent != null)
            inputMessage += $"You are currently near {MapLabelController.Instance.ClosestLabel(agent.transform.position)}. Please select a new legend.";
        inputMessage += PlayerSpottet ? "You see the player in front of you!\n" : "No player in sight.\n";
        inputMessage += @$"Respond with one line, limit yourself to 10 words. 
                        The format dictates that you start with a legend from the list, followed by your name and a message.                        
                        It's very important that the response adheres to the following format: 
                        <Legend>|<Youre name>: <Walkie talkie message>
                        Here are examples of correctly formattet respondses:
                        Wood 1|{agentPrompts.name}: Castle is clear, heading to Wood 1.
                        Sign 1|{agentPrompts.name}: Spread out! You'll search Wood 1, I'll search Sign 1!
                        Church|{agentPrompts.name}: I'll guard the church, someone check the graveyard!";

        chat.AppendSystemMessage(inputMessage);
        
        if (AIVision != null)
        {
            var screen = AIVision.Capture().EncodeToPNG();
            var image = ChatMessage.ImageInput.FromImageBytes(screen);
            chat.AppendUserInput("Picture of the map", image);
        }

        addUserInput(chat, inputMessage);
    }

    async void addUserInput(Conversation chat, String message) {
        chat.AppendUserInput(message);
        string response = await chat.GetResponseFromChatbotAsync();
        Debug.Log(response);
        
        if (response.Contains('|'))
        {
            var messages = response.Split('|');
            ChatLogController.Instance.AddText(messages[1]);
            var target = MapLabelController.Instance.ObjectByLabel(messages[0]);
            if (agent != null)
            {
                agent.GetComponent<AStarSearch>().Go(target);
                agent.GetComponent<GridMovement>().UpdateTarget();
            }
        }
        // agentCommands = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);
        // Debug.Log(agentCommands["agent1"]);
    }    

}
