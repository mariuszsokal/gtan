using System;
using System.Diagnostics;
using System.Collections;

using GTANetworkServer;
using GTANetworkShared;

namespace Core {
    /// <summary>
    /// Role play character class.
    /// </summary>
    /// <remarks>Character without owner is prohibited</remarks>
    class Character : IDisposable {
<<<<<<< HEAD
        /// <summary>
        /// The unique id of the character in database.
        /// </summary>
        private long id = Database.Data.Character.INVALID_ID;

        /// <summary>
        /// The character role-play name.
        /// </summary>
        private string name;

        /// <summary>
        /// The character role-play surname.
        /// </summary>
        private string surname;

        /// <summary>
        /// Is this character male?
        /// </summary>
        private bool isMale = true;

        /// <summary>
        /// The character skin.
        /// </summary>
        private int skin = 0;

        /// <summary>
        /// The character dimension.
        /// </summary>
        private int dimension = 0;

        /// <summary>
        /// The last character position.
        /// </summary>
        private Vector3 lastPosition = new Vector3();

        /// <summary>
        /// The last character rotation z-axis.
        /// </summary>
        private float lastZRotation = 0.0f;

        /// <summary>
        /// Character health.
        /// </summary>
        private short health = 100;
=======

        /// <summary>
        /// The character database object.
        /// </summary>
        private Database.Data.Character data = null;
>>>>>>> refs/remotes/RootKiller/master

        /// <summary>
        /// The owner player.
        /// </summary>
        private Player owner;

        /// <summary>
        /// Character constructor.
        /// </summary>
        /// <param name="Owner">The character owner.</param>
        public Character(Player Owner, Database.Data.Character Data = null) {
            Debug.Assert(Owner != null, "Character without owner is prohibited");
            owner = Owner;
            data = Data;
        }

        /// <summary>
        /// Delete all sub-objects.
        /// </summary>
        public void Dispose() {
            // no-op
        }

        /// <summary>
<<<<<<< HEAD
        /// Load cahracter from the database data object.
        /// </summary>
        /// <param name="character">The database data object.</param>
        public void Load(Database.Data.Character character) {
            id = character.id;
            name = character.name;
            surname = character.surname;

            isMale = character.is_male;

            skin = character.skin;
=======
        /// Update character state before saving.
        /// </summary>
        private void UpdateState() {
            Debug.Assert(owner.character == this);
            Debug.Assert(data != null);

            data.skin = API.shared.getEntityModel(owner);
            Vector3 pos = API.shared.getEntityPosition(owner);
            data.x = pos.X;
            data.y = pos.Y;
            data.z = pos.Z;
            data.rz = API.shared.getEntityRotation(owner).Z;
            data.dimension = API.shared.getEntityDimension(owner);
            data.health = (short) API.shared.getPlayerHealth(owner);
        }
>>>>>>> refs/remotes/RootKiller/master

            lastPosition.X = character.x;
            lastPosition.Y = character.y;
            lastPosition.Z = character.z;

            lastZRotation = character.rz;
        }

        /// <summary>
<<<<<<< HEAD
        /// Update character state before saving.
        /// </summary>
        private void updateState() {
            Debug.Assert(owner.character == this);

            skin = API.shared.getEntityModel(owner.client);
            lastPosition = API.shared.getEntityPosition(owner.client);
            lastZRotation = API.shared.getEntityRotation(owner.client).Z;
            dimension = API.shared.getEntityDimension(owner.client);
            health = (short) API.shared.getPlayerHealth(owner.client);
        }


        /// <summary>
=======
>>>>>>> refs/remotes/RootKiller/master
        /// Save or create character in database.
        /// </summary>
        /// <returns>true in case operation succeeds, false otherwise</returns>
        public bool Save() {
<<<<<<< HEAD
            updateState();

            if (id == Database.Data.Character.INVALID_ID) {
                id = Database.Context.Instance.createCharacter(name, surname, this.owner.AccountId);
                if (id == Database.Data.Character.INVALID_ID) {
                    return false;
                }
            }
            else {
                Database.Data.Character data = new Database.Data.Character();
                data.id = id;
                data.name = name;
                data.surname = surname;
                data.is_male = isMale;
                data.skin = skin;
                data.x = lastPosition.X;
                data.y = lastPosition.Y;
                data.z = lastPosition.Z;
                data.rz = lastZRotation;
                data.dimension = dimension;
                data.health = health;
                data.owner = owner.AccountId;
                return Database.Context.Instance.updateCharacter(data);
            }
            return true;
=======
            Debug.Assert(data != null);

            if (data.id == Database.Data.Character.INVALID_ID) {
                // Ensure that owner id is set.
                data.owner = owner.AccountId;

                return Database.Context.Instance.createCharacter(ref data);
            }

            UpdateState();
            return Database.Context.Instance.updateCharacter(data);
        }

        /// <summary>
        /// Spawn the character.
        /// </summary>
        public void Spawn() {
            API.shared.setEntityPosition(owner, new Vector3(data.x, data.y, data.z));
            API.shared.setEntityRotation(owner, new Vector3(0.0, 0.0, data.rz));
            API.shared.setEntityDimension(owner, data.dimension);

            API.shared.setPlayerSkin(owner, (PedHash)data.skin);

            API.shared.setPlayerHealth(owner, data.health);
        }

        /// <summary>
        /// Get character full name.
        /// </summary>
        /// <returns>Character full name.</returns>
        public string GetFullName() {
            return this.data.name + " " + this.data.surname;
>>>>>>> refs/remotes/RootKiller/master
        }

        /// <summary>
        /// Spawn the character.
        /// </summary>
        public void Spawn() {
            Client client = owner.client;

            API.shared.setEntityPosition(client, lastPosition);
            API.shared.setEntityRotation(client, new Vector3(0.0, 0.0, lastZRotation));
            API.shared.setEntityDimension(client, dimension);

            API.shared.setPlayerSkin(client, (PedHash)skin);

            API.shared.setPlayerHealth(client, health);
        }

        /// <summary>
        /// Get character full name.
        /// </summary>
        /// <returns>Character full name.</returns>
        public string GetFullName() {
            return this.name + " " + this.surname;
        }

        /// <summary>
        /// Load and set as current character at 1 slot for the player.
        /// </summary>
        /// <param name="player">The player for which to load and set the character.</param>
        static public void LoadPlayerCharacter(Player player) {
            ArrayList characters = Database.Context.Instance.getCharactersByOwner(player.AccountId);
            if (characters.Count == 0) {
                // No characters :-(
                return;
            }

            Database.Data.Character data = (Database.Data.Character) characters[0];
            Character character = new Character(player, data);
            player.SetCharacter(character);
        }
    }
}
