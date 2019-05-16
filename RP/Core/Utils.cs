using System;
using System.Text;
<<<<<<< HEAD
using GTANetworkServer;

=======

using GTANetworkServer;
>>>>>>> refs/remotes/RootKiller/master

namespace Core {
    /// <summary>
    /// Set of utitiles used by gamemode.
    /// </summary>
    class Utils {
        /// <summary>
        /// Encodes string using SHA256.
        /// </summary>
        /// <param name="StringToEncode">The string to encode</param>
        /// <returns>Encoded string</returns>
        public static string sha256(string StringToEncode) {
            System.Security.Cryptography.SHA256Managed crypt = new System.Security.Cryptography.SHA256Managed();
            System.Text.StringBuilder hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(StringToEncode), 0, Encoding.UTF8.GetByteCount(StringToEncode));
            foreach (byte theByte in crypto) {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }

        /// <summary>
        /// Show information for player.
        /// </summary>
        /// <param name="player">The player to show information for.</param>
        /// <param name="text">The information text.</param>
        /// <param name="Notification">Show the information as notification or chat message.</param>
        public static void showPlayerInfo(Player player, String text, bool Notification) {
            if (Notification) {
                API.shared.sendNotificationToPlayer(player, "~b~Informacja » ~w~" + text);
            }
            else {
                player.client.sendChatMessage("~b~Informacja » ~w~" + text);
            }
        }

        /// <summary>
        /// Show tip for player.
        /// </summary>
        /// <param name="player">The player to show tip for.</param>
        /// <param name="text">The tip text.</param>
        /// <param name="Notification">Show the tip as notification or chat message.</param>
        public static void showPlayerTip(Player player, String text, bool Notification) {
            if (Notification) {
                API.shared.sendNotificationToPlayer(player, "~b~Wskazówka » ~w~" + text);
            }
            else {
                player.client.sendChatMessage("~b~Wskazówka » ~w~" + text);
            }
        }

        /// <summary>
        /// Show error for player.
        /// </summary>
        /// <param name="player">The player to show error for.</param>
        /// <param name="text">The error text.</param>
        /// <param name="Notification">Show the error as notification or chat message.</param>
        public static void showPlayerError(Player player, String text, bool Notification) {
            if (Notification) {
                API.shared.sendNotificationToPlayer(player, "~r~Błąd » ~w~" + text);
            }
            else {
                player.client.sendChatMessage("~r~Błąd » ~w~" + text);
            }
        }

        /// <summary>
        /// Send admin message to player.
        /// </summary>
        /// <param name="player">The player to show admin message .</param>
        /// <param name="admin">The admin who sends message to player.</param>
        /// <param name="message">The message to show to player.</param>
        public static void showAdminMessage(Player player, Player admin, String message) {
<<<<<<< HEAD
            int adminLevel = API.shared.getEntityData(admin, "ADMIN_LEVEL");
            string finalMessage = "";
            if (adminLevel == 1) {
                finalMessage = "~#C52F1A~Gamemaster " + admin.CurrentCharacter.GetFullName() + " " + message;
            }
            else if(adminLevel == 2) {
                finalMessage = "~#C52F1A~Administrator " + admin.CurrentCharacter.GetFullName() + " " + message;
=======
            string finalMessage = "";
            if (admin.AdminLevel == AdminLevels.Moderator) {
                finalMessage = "~g~Moderator " + admin.CurrentCharacter.GetFullName() + " " + message;
            }
            else if(admin.AdminLevel == AdminLevels.Admin) {
                finalMessage = "~r~Administrator " + admin.CurrentCharacter.GetFullName() + " " + message;
>>>>>>> refs/remotes/RootKiller/master
            }

            if (finalMessage.Length > 0) {
                API.shared.sendChatMessageToPlayer(player, finalMessage);
            }
        }
    }
}
