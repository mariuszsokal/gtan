using System;

using GTANetworkServer;
using GTANetworkShared;

namespace Core
{
    /// <summary>
    /// Main resource script.
    /// </summary>
    public class Main : Script, IDisposable
    {
        /// <summary>
        /// The instance of the player manager.
        /// </summary>
        private PlayerManager       playerManager = null;

        /// <summary>
        /// The database context.
        /// </summary>
        private Database.Context    db = null;

        /// <summary>
        /// The spawn point. (at the moment rotation is 0,0,0)
        /// </summary>
        private static Vector3 SpawnPoint = new Vector3(-1041.671, -2743.54, 21.3594);

        /// <summary>
        /// The main script constructor.
        /// </summary>
        public Main() {
            API.consoleOutput("Initializing RP Core");

            API.consoleOutput("Connecting to database..");

            db = new Database.Context();

            API.consoleOutput("Connected!");

            API.consoleOutput("Creating player manager..");

            playerManager = new PlayerManager();

            API.consoleOutput("Player manager created.");



            setupCallbacks();

            API.consoleOutput("RP Core started");

            API.delay(60 * 1000, false, () => {
                syncTime();
            });
            syncTime();
<<<<<<< HEAD
        }

        private void syncTime() {
            DateTime Now = DateTime.Now;
            API.setTime(Now.Hour, Now.Minute);

            API.consoleOutput("Synchronizing time: " + Now.Hour + ":" + Now.Minute);
        }

=======
        }

        private void syncTime() {
            DateTime Now = DateTime.Now;
            API.setTime(Now.Hour, Now.Minute);

            API.consoleOutput("Synchronizing time: " + Now.Hour + ":" + Now.Minute);
        }

>>>>>>> refs/remotes/RootKiller/master

        /// <summary>
        /// Delete all sub-objects.
        /// </summary>
        public void Dispose() {
            playerManager.Dispose();

            db.Dispose();
        }

        /// <summary>
        /// Setup all callbacks.
        /// </summary>
        private void setupCallbacks() {
            API.onPlayerConnected += onPlayerConnected;
            API.onPlayerDisconnected += onPlayerDisconnected;
            API.onChatMessage += onChatMessage;
            API.onClientEventTrigger += onClientEventTrigger;
            API.onResourceStop += onResourceStop;
        }

        /// <summary>
        /// Handle moment when this resource stops.
        /// </summary>
        private void onResourceStop() {
            Dispose();
        }

        /// <summary>
        /// Handle events triggered by client.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventName"></param>
        /// <param name="arguments"></param>
        private void onClientEventTrigger(Client sender, string eventName, params object[] arguments) {
            Player player = playerManager.findPlayerByHandle(sender);
            if (player == null) {
                API.consoleOutput("Player " + sender.name + " triggered event however was not registered in player manager.");
                API.kickPlayer(sender);
                return;
            }

            if (eventName == "doLogin") {
                if (arguments.Length != 2) {
                    API.consoleOutput(sender.name + " send invalid doLogin event");
                    return;
                }

                string username = ((String) arguments[0]).ToLower();
                string password = (string) arguments[1];

                Database.Data.User user = db.findUserByName(username);

                if (user == null) {
                    API.triggerClientEvent(sender, "loginResult", false, "Invalid user name");
                    return;
                }

                if (playerManager.findPlayerByAccountId(user.id) != null) {
                    API.triggerClientEvent(sender, "loginResult", false, "There is some player using this account on the server.");
                    return;
                }

                if (user.password != Utils.sha256(password)) {
                    API.triggerClientEvent(sender, "loginResult", false, "Invalid password");
                    return;
                }

                API.triggerClientEvent(sender, "loginResult", true);
                API.sendNativeToPlayer(sender, Hash.DO_SCREEN_FADE_OUT, 300);

                API.delay(900, true, () => {
                    API.setEntityPositionFrozen(sender, false);
                    player.InitAuth(user.id);
                    Character.LoadPlayerCharacter(player);
                    API.sendNativeToPlayer(sender, Hash.DO_SCREEN_FADE_IN, 300);

                    API.triggerClientEvent(sender, "playerSpawn");
                });
            }
        }

        /// <summary>
        /// Handle chat message.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        /// <param name="cancel"></param>
        private void onChatMessage(Client sender, string message, CancelEventArgs cancel) {
            Player player = playerManager.findPlayerByHandle(sender);
            if (player == null || player.CurrentCharacter == null) {
                return;
            }

            string finalMessage = player.CurrentCharacter.GetFullName() + " mówi: " + message;
            foreach (Client client in API.getPlayersInRadiusOfPlayer(Commands.LOCAL_CHAT_RADIUS, sender)) {
                client.sendChatMessage(finalMessage);
            }

            cancel.Cancel = true;
        }

        /// <summary>
        /// Handle player connection.
        /// </summary>
        /// <param name="player"></param>
        private void onPlayerConnected(Client player)
        {
            playerManager.registerPlayer(player);

            API.setPlayerNametagVisible(player, false);

            player.sendChatMessage("Witaj na serwerze deweloperskim ~g~LSS-RP.pl");
            player.sendChatMessage("Pamiętaj że aktualna wersja oprogramowania serwera jest wersją");
            player.sendChatMessage("bardzo wczesną i nie reprezentuje ona finalnego produktu.");
            player.sendChatMessage("Miłej zabawy - ekipa LSS-RP.pl");

            // Set player dimension to player handle + 10000. It should be suffficiently unique.
            API.setEntityDimension(player, player.handle.Value + 10000);
            API.setEntityPositionFrozen(player, true);

            API.freezePlayerTime(player, true);
        }

        /// <summary>
        /// Handle player disconnect.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="reason"></param>
        private void onPlayerDisconnected(Client player, string reason) {
            playerManager.unregisterPlayer(player);
        }
    }
}
