using System;
using System.Net.Http;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;
using TwitchClientObject = TwitchLib.Client.TwitchClient;
namespace WarzoneVoiceController.TwitchClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Bot bot = new Bot();
            Console.ReadLine();
        }
    }

    class Bot
    {
        TwitchClientObject client;

        public Bot()
        {
            var credentials = new ConnectionCredentials(Keys.TwitchAccount, Keys.TwitchToken);
            var clientOptions = new ClientOptions
            {
                MessagesAllowedInPeriod = 750,
                ThrottlingPeriod = TimeSpan.FromSeconds(30)
            };
            var customClient = new WebSocketClient(clientOptions);
            client = new TwitchClientObject(customClient);
            client.Initialize(credentials, Keys.TwitchChannel);

            client.OnLog += Client_OnLog;
            client.OnJoinedChannel += Client_OnJoinedChannel;
            client.OnMessageReceived += Client_OnMessageReceived;
            client.OnWhisperReceived += Client_OnWhisperReceived;
            client.OnNewSubscriber += Client_OnNewSubscriber;
            client.OnConnected += Client_OnConnected;

            client.Connect();
        }

        private void Client_OnLog(object sender, OnLogArgs e)
        {
            Console.WriteLine($"{e.DateTime.ToString()}: {e.BotUsername} - {e.Data}");
        }

        private void Client_OnConnected(object sender, OnConnectedArgs e)
        {
            Console.WriteLine($"Connected to {e.AutoJoinChannel}");
        }

        private void Client_OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            var message = "Hey everyone! I am a bot that lets you shout dark souls commands. Try something like !attack or !roll";
            Console.WriteLine(message);
            client.SendMessage(e.Channel, message);
        }

        private async void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            // Get the message. Validate it is a command with "!". Then send to API to then route
            using (var client = new HttpClient())
            {
                var command = "FallbackIntent";


                if (e.ChatMessage.Message.ToLower().StartsWith("!forward"))
                    command = "MoveForwardIntent";
                if (e.ChatMessage.Message.ToLower().StartsWith("!backwards"))
                    command = "MoveBackwardsIntent";
                if (e.ChatMessage.Message.ToLower().StartsWith("!left"))
                    command = "MoveLeftIntent";
                if (e.ChatMessage.Message.ToLower().StartsWith("!right"))
                    command = "MoveRightIntent";
                if (e.ChatMessage.Message.ToLower().StartsWith("!attack"))
                    command = "RightLightIntent";
                if (e.ChatMessage.Message.ToLower().StartsWith("!leftie") || e.ChatMessage.Message.ToLower().StartsWith("lightning"))
                    command = "LeftLightIntent";
                if (e.ChatMessage.Message.ToLower().StartsWith("!heavy"))
                    command = "RightHeavyIntent";
                if (e.ChatMessage.Message.ToLower().StartsWith("!parry"))
                    command = "LeftHeavyIntent";
                if (e.ChatMessage.Message.ToLower().StartsWith("!roll"))
                    command = "RollIntent";
                if (e.ChatMessage.Message.ToLower().StartsWith("!item")
                    || e.ChatMessage.Message.ToLower().StartsWith("!grenade")
                    || e.ChatMessage.Message.ToLower().StartsWith("!heal")
                    || e.ChatMessage.Message.ToLower().StartsWith("!estus")
                    || e.ChatMessage.Message.ToLower().StartsWith("!yeet"))
                    command = "ItemIntent";
                if (e.ChatMessage.Message.ToLower().StartsWith("!swap"))
                    command = "SwapRightWeaponIntent";
                if (e.ChatMessage.Message.ToLower().StartsWith("!swapleft"))
                    command = "SwapLeftWeaponIntent";

                if (e.ChatMessage.Message.ToLower().StartsWith("!rotateitem"))
                    command = "RotateItemIntent";
                if (e.ChatMessage.Message.ToLower().StartsWith("!rotatespell"))
                    command = "RotateSpellIntent"; 
                if (e.ChatMessage.Message.ToLower().StartsWith("!block"))
                    command = "BlockIntent"; 
                if (e.ChatMessage.Message.ToLower().StartsWith("!run"))
                    command = "RunIntent"; 
                if (e.ChatMessage.Message.ToLower().StartsWith("!jump"))
                    command = "JumpIntent"; 
                if (e.ChatMessage.Message.ToLower().StartsWith("!runjump"))
                    command = "RunJumpIntent"; 
                if (e.ChatMessage.Message.ToLower().StartsWith("!snipe"))
                    command = "SnipeIntent"; 
                if (e.ChatMessage.Message.ToLower().StartsWith("!gitgud"))
                    command = "ParryRepostIntent"; 
                if (e.ChatMessage.Message.ToLower().StartsWith("!interact"))
                    command = "InteractIntent"; 
                if (e.ChatMessage.Message.ToLower().StartsWith("!kick"))
                    command = "KickIntent"; 
                if (e.ChatMessage.Message.ToLower().StartsWith("!lock"))
                    command = "LockIntent";

                var response = await client.PostAsync($"https://darksouls.azurewebsites.net/api/command/{command}", null);
                Console.WriteLine(response);
            }

        }

        private void Client_OnWhisperReceived(object sender, OnWhisperReceivedArgs e)
        {
            if (e.WhisperMessage.Username == "my_friend")
                client.SendWhisper(e.WhisperMessage.Username, "Hey! Whispers are so cool!!");
        }

        private void Client_OnNewSubscriber(object sender, OnNewSubscriberArgs e)
        {
            if (e.Subscriber.SubscriptionPlan == SubscriptionPlan.Prime)
                client.SendMessage(e.Channel, $"Welcome {e.Subscriber.DisplayName} to the substers! You just earned 500 points! So kind of you to use your Twitch Prime on this channel!");
            else
                client.SendMessage(e.Channel, $"Welcome {e.Subscriber.DisplayName} to the substers! You just earned 500 points!");
        }
    }
}
