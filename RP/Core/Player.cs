using System;
using System.Diagnostics;

using GTANetworkServer;
using GTANetworkShared;

namespace Core {

    /// <summary>
    /// The RP player data class.
    /// </summary>
    class Player : IDisposable {
        /// <summary>
        /// The GTA:N client object.
        /// </summary>
        public Client client    = null;

        /// <summary>
        /// Current player character.
        /// </summary>
        public Character character = null;

        /// <summary>
        /// Public getter for current player character.
        /// </summary>
        public Character CurrentCharacter {
            get {
                return character;
            }
        }

        /// <summary>
        /// The RP account id.
        /// </summary>
        private long accountId = Database.Data.User.INVALID_ACCOUNT_ID;

        /// <summary>
        /// The RP account id getter.
        /// </summary>
        public long AccountId {
            get {
                return accountId;
            }
        }

        /// <summary>
        /// The current admin level.
        /// </summary>
        private AdminLevels adminLevel = AdminLevels.NotAdmin;

        /// <summary>
        /// Getter for admin level.
        /// </summary>
        public AdminLevels AdminLevel
        {
            get {
                return adminLevel;
            }
        }

        /// <summary>
        /// Constructor of player object.
        /// </summary>
        /// <param name="Client">The GTA:N client representing the player.</param>
        public Player(Client Client) {
            client = Client;
        }


        /// <summary>
        /// Delete sub-objects.
        /// </summary>
        public void Dispose() {
            if (character != null) {
                character.Dispose();
                character = null;
            }
        }

        /// <summary>
        /// Initialize authentication info for the player.
        /// </summary>
        /// <remarks>
        /// This is not possible to call this method twice for single account.
        /// In case there will be need for logout the logout method needs to be added.
        /// </remarks>
        /// <param name="AccountId">The id of the account player was authenticated to.</param>
        public void InitAuth(long AccountId) {
            Debug.Assert(accountId == Database.Data.User.INVALID_ACCOUNT_ID);
            Debug.Assert(AccountId != Database.Data.User.INVALID_ACCOUNT_ID);

            accountId = AccountId;

            client.setSyncedData("AccountId", accountId);
        }

        /// <summary>
        /// Check if given player is authenticated.
        /// </summary>
        /// <returns>true if player is authenticated false otherwise</returns>
        bool IsAuthenticated() {
            return accountId != Database.Data.User.INVALID_ACCOUNT_ID;
        }

        /// <summary>
        /// Set current player character.
        /// </summary>
        /// <param name="Char">The character to use</param>
        public void SetCharacter(Character Char) {
            Debug.Assert(character == null && Char != null);

            character = Char;

            character.Spawn();
        }

        /// <summary>
        /// Save player data.
        /// </summary>
        public void Save() {
            if (character != null) {
                character.Save();
            }
        }

        /// <summary>
        /// Implicit operator casting Player to Client.
        /// </summary>
        /// <param name="player">Player to cast</param>
        public static implicit operator Client(Player player) {
            return player.client;
        }

        /// <summary>
        /// Implicit operator casting Player to NetHandle of client.
        /// </summary>
        /// <param name="player">Player to cast</param>
        public static implicit operator NetHandle(Player player) {
            return player.client.handle;
        }
<<<<<<< HEAD

        public void createNameLabel(Client player) {
            Player invoker = PlayerManager.Instance.findPlayerByHandle(player);

            var pos = API.shared.getEntityPosition(player);
            var playerDimension = API.shared.getEntityDimension(player);
            var player_name = API.shared.getEntityData(player, "PLAYER_NAME_LABEL");
            if(player_name != null || player_name == true)
            {
                API.shared.deleteEntity(player_name);
            }
            if(API.shared.getEntitySyncedData(player, "PLAYER_ADUTY") == 1)
            {
                player_name = API.shared.createTextLabel("(( " + invoker.CurrentCharacter.GetFullName() + " ))\n(Administrator GTA:N)", new Vector3(pos.X, pos.Y, pos.Z + 3.0), 15.0f, 0.45f, true);
                API.shared.setTextLabelColor(player_name, 220, 20, 60, 255); //Administrator GTA:N
            }
            else
            {
                player_name = API.shared.createTextLabel("(( " + invoker.CurrentCharacter.GetFullName() + " ))\n(wysportowany)", new Vector3(pos.X, pos.Y, pos.Z + 3.0), 15.0f, 0.45f, true);
                //API.setTextLabelColor(player_name, 189, 173, 141, 255); //zwykły
                //API.setTextLabelColor(player_name, 192, 178, 86, 255); //premium
                API.shared.setTextLabelColor(player_name, 252, 232, 96, 255); //premium opis
            }
            API.shared.attachEntityToEntity(player_name, player, "SKEL_Head", new Vector3(0.0, 0.0, 1.1), new Vector3(0, 0, 0));
            player_name.dimension = playerDimension;
            API.shared.setEntityData(player, "PLAYER_NAME_LABEL", player_name);
        }
=======
>>>>>>> refs/remotes/RootKiller/master
    }
}
