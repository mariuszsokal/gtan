using System;

using GTANetworkServer;
using GTANetworkShared;

namespace Core {
    /// <summary>
    /// Sub-script used to handle commands.
    /// </summary>
    class Commands : Script {
        public const float LOCAL_CHAT_RADIUS = 10.0f;
        public const float WISPER_CHAT_RADIUS = 5.0f;
        public const float SCREAM_CHAT_RADIUS = 20.0f;

        [Command("tpdo", "~y~Użycie: ~w~/tpdo [gracz]", Alias = "tpto")]
        public void TeleportTo(Client invoker, Client customer) {
            Player player = PlayerManager.Instance.findPlayerByHandle(invoker);
            if (player == null || player.CurrentCharacter == null) {
                return;
            }

            Player target = PlayerManager.Instance.findPlayerByHandle(customer);
            if (target == null || target.CurrentCharacter == null || player == target) {
                API.sendNotificationToPlayer(player.client, "~r~Błąd » ~w~ Nie ma takiego gracza.");
                return;
            }

            Utils.showPlayerInfo(player, "Teleportowano się do gracza " + target.CurrentCharacter.GetFullName() + ".", true);
            Utils.showAdminMessage(target, player, "teleportował się do Ciebie.");
            API.setEntityPosition(player.client, API.getEntityPosition(target.client));
        }  

        [Command("tptutaj", "~y~Użycie: ~w~/tptptutaj [gracz]", Alias = "getthere,gt")]
        public void TeleportToMe(Client invoker, Client customer) {
            Player player = PlayerManager.Instance.findPlayerByHandle(invoker);
            if (player == null || player.CurrentCharacter == null) {
                return;
            }

            Player target = PlayerManager.Instance.findPlayerByHandle(customer);
            if (target == null || target.CurrentCharacter == null || player == target) {
                API.sendNotificationToPlayer(player.client, "~r~Błąd » ~w~ Nie ma takiego gracza.");
                return;
            }

            Utils.showPlayerInfo(player, "Teleportowano gracza " + target.CurrentCharacter.GetFullName() + " do Ciebie.", true);
            Utils.showAdminMessage(target, player, "teleportował Ciebie do siebie.");
            API.setEntityPosition(target.client, API.getEntityPosition(player.client));
        }  

        [Command("me", "~y~Użyj: ~w~/me [akcja]", Alias = "ja", GreedyArg = true)]
        public void Me(Client invoker, String Parameters) {
            Player player = PlayerManager.Instance.findPlayerByHandle(invoker);
            if (player == null || player.CurrentCharacter == null) {
                return;
            }
<<<<<<< HEAD
            string message = "~#C2A2DA~" + player.CurrentCharacter.GetFullName() + " " + Parameters;

=======

            string message = "~b~" + player.CurrentCharacter.GetFullName() + " " + Parameters;
>>>>>>> refs/remotes/RootKiller/master
            foreach (Client client in API.getPlayersInRadiusOfPlayer(LOCAL_CHAT_RADIUS, invoker)) {
                invoker.sendChatMessage(message);
            }
        }

        [Command("do", "~y~Użyj: ~w~/do [akcja]", GreedyArg = true)]
        public void Do(Client invoker, String Parameters) {
            Player player = PlayerManager.Instance.findPlayerByHandle(invoker);
            if (player == null || player.CurrentCharacter == null) {
                return;
            }
<<<<<<< HEAD
            string message = "~#C2A2DA~" + Parameters + " ((" + player.CurrentCharacter.GetFullName() + "))";

=======

            string message = "~b~" + Parameters + " ((" + player.CurrentCharacter.GetFullName() + "))";
            foreach (Client client in API.getPlayersInRadiusOfPlayer(LOCAL_CHAT_RADIUS, invoker)) {
                invoker.sendChatMessage(message);
            }
        }

        [Command("krzyk", "~y~Użyj: ~w~/krzyk [wiadomość]", Alias = "k", GreedyArg = true)]
        public void Scream(Client invoker, String Parameters) {
            Player player = PlayerManager.Instance.findPlayerByHandle(invoker);
            if (player == null || player.CurrentCharacter == null) {
                return;
            }

            string message = player.CurrentCharacter.GetFullName() + " krzyczy: " + Parameters;
            foreach (Client client in API.getPlayersInRadiusOfPlayer(SCREAM_CHAT_RADIUS, invoker)) {
                invoker.sendChatMessage(message);
            }
        }

        [Command("szept", "~y~Użyj: ~w~/szept [wiadomość]", Alias = "s", GreedyArg = true)]
        public void Wisper(Client invoker, String Parameters) {
            Player player = PlayerManager.Instance.findPlayerByHandle(invoker);
            if (player == null || player.CurrentCharacter == null) {
                return;
            }

            string message = player.CurrentCharacter.GetFullName() + " szepcze: " + Parameters;
            foreach (Client client in API.getPlayersInRadiusOfPlayer(WISPER_CHAT_RADIUS, invoker)) {
                invoker.sendChatMessage(message);
            }
        }

        [Command("ooc", "~y~Użyj: ~w~/ooc [wiadomość]", Alias = "b", GreedyArg = true)]
        public void OOC(Client invoker, String Parameters) {
            Player player = PlayerManager.Instance.findPlayerByHandle(invoker);
            if (player == null || player.CurrentCharacter == null) {
                return;
            }

            string message = "~y~OOC ~w~" + player.CurrentCharacter.GetFullName() + ": " + Parameters;
>>>>>>> refs/remotes/RootKiller/master
            foreach (Client client in API.getPlayersInRadiusOfPlayer(LOCAL_CHAT_RADIUS, invoker)) {
                invoker.sendChatMessage(message);
            }
        }

        [Command("szept", "~y~Użyj: ~w~/szept [wiadomość]", Alias = "s", GreedyArg = true)]
        public void Wisper(Client invoker, String Parameters) {
            Player player = PlayerManager.Instance.findPlayerByHandle(invoker);
            if (player == null || player.CurrentCharacter == null) {
                return;
            }
            string message = player.CurrentCharacter.GetFullName() + " szepcze: " + Parameters;

            foreach (Client client in API.getPlayersInRadiusOfPlayer(WISPER_CHAT_RADIUS, invoker)) {
                invoker.sendChatMessage(message);
            }
        }

        [Command("ooc", "~y~Użyj: ~w~/ooc [wiadomość]", Alias = "b", GreedyArg = true)]
        public void OOC(Client invoker, String Parameters) {
            Player player = PlayerManager.Instance.findPlayerByHandle(invoker);
            if (player == null || player.CurrentCharacter == null) {
                return;
            }

            string message = "(( " + player.CurrentCharacter.GetFullName() + ": " + Parameters + " ))";
            foreach (Client client in API.getPlayersInRadiusOfPlayer(LOCAL_CHAT_RADIUS, invoker)) {
                invoker.sendChatMessage(message);
            }
        }

        [Command("opis", "~y~Użyj: ~w~/opis [treść]", GreedyArg = true)]
        public void Description(Client invoker, string desc) {
            Player player = PlayerManager.Instance.findPlayerByHandle(invoker);
            Vector3 pos = API.getEntityPosition(player);

            var playerDimension = API.getEntityDimension(player);
            var player_desc = API.getEntityData(player, "PLAYER_DESC");


            if (player_desc == null || player_desc == true) {
                API.deleteEntity(player_desc);
            }

            player_desc = API.createTextLabel("~p~~h~" + desc, pos, 15.0f, 0.45f, true);

            API.attachEntityToEntity(player_desc, player, "SKEL_Pelvis", new Vector3(0, 0, 0), new Vector3(0, 0, 0));

            player_desc.dimension = playerDimension;

            API.setEntityData(player, "PLAYER_DESC", player_desc);

            Utils.showPlayerInfo(player, "Opis postaci został pomyślnie utworzony.", true);
        }

        [Command("getpos")]
        public void GetPos(Client invoker) {
            API.consoleOutput(invoker.position.ToString());
        }

        [Command("v")]
        public void Veh(Client invoker) {
            Vehicle veh = API.createVehicle(VehicleHash.Premier, invoker.position, invoker.rotation, 0, 0);

            API.setPlayerIntoVehicle(invoker, veh, -1);
        }
    }
}
