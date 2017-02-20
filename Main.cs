using System;
using System.Linq;
using Rocket.Core.Plugins;
using Rocket.Unturned.Chat;
using Rocket.Core.Commands;
using Rocket.API;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Logger = Rocket.Core.Logging.Logger;
using System.Net;
using System.Diagnostics;

namespace Plugin4U.SayAs
{
    public class Main : RocketPlugin
    {
        public static Main Instance;

        protected override void Load()
        {
            WebClient clt = new WebClient();
            Instance = this;
            base.Load();
            Logger.Log("[Plugin4U] SayAs Loaded!");
            string a = clt.DownloadString("http://Plugin4U.cf/SayAs_Version.txt");
            if(!(a == "1.0.0.0")) { Logger.LogWarning("Update! NEW VERSION : " + a); }
        }


        [RocketCommand("SayAs", "", "", AllowedCaller.Both)]
        public void Execute(IRocketPlayer caller, params string[] command)
        {
            try
            {
                if (command.Length <= 1)
                {
                    UnturnedChat.Say(caller, "/sayas <player> <message>");
                    return;
                }
                UnturnedPlayer player = UnturnedPlayer.FromName(command[0]);
                if (player.IsAdmin) return;
                if (player.HasPermission("SayAs.Block")) return;
                ChatManager.instance.askChat(player.CSteamID, (byte)EChatMode.GLOBAL, string.Join(" ", command.Skip(1).ToArray()));
                UnturnedChat.Say(caller, "Successfully said as [" + player.DisplayName + "] (" + string.Join(" ", command.Skip(1).ToArray()) + ")");
            }
            catch (Exception ex)
            {
                Logger.LogError("[SayAs] " + ex.ToString());
            }
        }
    }
}
