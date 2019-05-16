using System;
using System.Collections;
using System.Reflection;

using MySql.Data.MySqlClient;

namespace Database {
    /// <summary>
    /// Database context allowing easy queries to the server database.
    /// </summary>
    public class Context : IDisposable {
        /// <summary>
        /// Static instance to the context for better accessibility.
        /// </summary>
        public static Context Instance = null;

        /// <summary>
        /// The database connection.
        /// </summary>
        private MySqlConnection dbConnection = null;

        /// <summary>
        /// Database context constructor.
        /// </summary>
        public Context() {
            dbConnection = new MySqlConnection("server=127.0.0.1;port=3306;uid=lssrp;pwd=lssrp123;database=lssrp");
            dbConnection.Open();

            Instance = this;
        }

        /// <summary>
        /// Destroy object resources.
        /// </summary>
        public void Dispose() {
            dbConnection.Close();
            dbConnection.Dispose();
            dbConnection = null;

            Instance = null;
        }


        /// <summary>
        /// Internal helper used to deserialize SQL data into the data object.
        /// </summary>
        /// <param name="Obj">The object where to deserialize data into</param>
        /// <param name="Reader">The SQL reader used to read data</param>
        private void deserializeData(object Obj, MySqlDataReader Reader) {
            foreach (FieldInfo field in Obj.GetType().GetFields()) {
                DBField dbField = field.GetCustomAttribute<DBField>();
                if (dbField == null) {
                    continue;
                }

                field.SetValue(Obj, Reader[dbField.name]);
            }
        }

        /// <summary>
        /// Internal helper used to build an update query from the object.
        /// </summary>
        /// <remarks>
        /// The update queries are quite limited at the moment and support
        /// only one update key however it is sufficient for now.
        ///
        /// The method may throw exception in case object is somehow broken.
        /// </remarks>
        /// <param name="Obj">The object to build update query for.</param>
        /// <returns>Npgsql command with generated query.</returns>
        private MySqlCommand buildUpdateCommand(object Obj) {
            Type type = Obj.GetType();

            DBTable tableData = type.GetCustomAttribute<DBTable>();
            if (tableData == null) {
                throw new Exception("Tried to build update command using the object without DBTable attribute defined. Type: " + type.AssemblyQualifiedName);
            }

            if (tableData.name.Length == 0) {
                throw new Exception("Tried to build update command using the object which table name is zero length. Type: " + type.AssemblyQualifiedName);
            }

            string where = "";

            MySqlCommand command = new MySqlCommand("", dbConnection);

            command.CommandText = "UPDATE " + tableData.name + " ";

            bool isFirstField = true;
            foreach (FieldInfo field in type.GetFields()) {
                DBField dbField = field.GetCustomAttribute<DBField>();
                if (dbField == null) {
                    continue;
                }

                if (dbField.skipUpdate) {
                    continue;
                }

                string fieldName = dbField.name;
                string paramName = "@" + fieldName;
                object value = field.GetValue(Obj);

                command.Parameters.AddWithValue(paramName, value);

                if (dbField.isUpdateKey) {
                    if (where.Length > 0) {
                        throw new Exception("Unsupported duplicate update key detected name = " + fieldName + ", where value = " + where);
                    }
                    else {
                        where = " WHERE " + fieldName + " = " + paramName;
                    }
                    continue;
                }

                if (isFirstField) {
                    command.CommandText += "SET ";
                    isFirstField = false;
                }
                else {
                    command.CommandText += ", ";
                }

                command.CommandText += dbField.name + " = " + paramName;
            }

            command.CommandText += where;
            return command;
        }

        /// <summary>
        /// Find user by username.
        /// </summary>
        /// <param name="username">The username used to find user (should be lower case)</param>
        /// <returns>The database data object of the user - null in case no user was found</returns>
        public Data.User findUserByName(string username) {
            MySqlCommand command = new MySqlCommand("SELECT id, username, password FROM users WHERE username=@username", dbConnection);
            command.Parameters.AddWithValue("@username", username);

            MySqlDataReader reader = command.ExecuteReader();
            if (!reader.Read()) {
                reader.Close();
                return null;
            }

            Data.User user = new Data.User();
            deserializeData(user, reader);
            reader.Close();
            return user;
        }

        /// <summary>
        /// Get characters array by owner.
        /// </summary>
        /// <param name="OwnerAccountId">The owner account id</param>
        /// <returns>The array list containing database data objects of the characters.</returns>
        public ArrayList getCharactersByOwner(long OwnerAccountId) {
<<<<<<< HEAD
            NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM characters WHERE owner=:owner", dbConnection);
            command.Parameters.AddWithValue(":owner", OwnerAccountId);
=======
            MySqlCommand command = new MySqlCommand("SELECT * FROM characters WHERE owner=@owner", dbConnection);
            command.Parameters.AddWithValue("@owner", OwnerAccountId);
>>>>>>> refs/remotes/RootKiller/master

            ArrayList characters = new ArrayList();

            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read()) {
                Data.Character character = new Data.Character();
                deserializeData(character, reader);
                characters.Add(character);
            }
            reader.Close();
            return characters;
        }


        /// <summary>
        /// Create character in database.
        /// </summary>
<<<<<<< HEAD
        /// <param name="Name">The name of the character to create</param>
        /// <param name="Surname">The surname of the character to create</param>
        /// <param name="OwnerAccountId">The id of the owner account</param>
        /// <returns>The unique id of the created character or DBData.Character.INVALID_ID in case insert failed</returns>
        public long createCharacter(string Name, string Surname, long OwnerAccountId) {
            NpgsqlCommand command = new NpgsqlCommand("INSERT INTO characters (name,surname,owner) VALUES(:name,:surname,:ownerAccountId) RETURNING id", dbConnection);
            command.Parameters.AddWithValue(":name", Name);
            command.Parameters.AddWithValue(":surname", Surname);
            command.Parameters.AddWithValue(":ownerAccountId", OwnerAccountId);
=======
        /// <param name="CharacterData">The data of character to create</param>
        /// <returns>Returns true and sets the id to id of the created entry if character was created returns false otherwise.</returns>
        public bool createCharacter(ref Data.Character CharacterData) {
            if (CharacterData.id != Data.Character.INVALID_ID) {
                throw new Exception("Tried to create character from data of already exising character.");
            }

            MySqlCommand command = new MySqlCommand("INSERT INTO characters (name,surname,owner,is_male,skin) VALUES(@name,@surname,@owner,@is_male,@skin) RETURNING id", dbConnection);
            command.Parameters.AddWithValue("@name", CharacterData.name);
            command.Parameters.AddWithValue("@surname", CharacterData.surname);
            command.Parameters.AddWithValue("@owner", CharacterData.owner);
            command.Parameters.AddWithValue("@is_male", CharacterData.is_male);
            command.Parameters.AddWithValue("@skin", CharacterData.skin);
>>>>>>> refs/remotes/RootKiller/master

            object id = command.ExecuteScalar();
            if (id == null) {
                return false;
            }

            CharacterData.id = (long)id;
            return true;
        }


        /// <summary>
        /// Update character state in database.
        /// </summary>
        /// <param name="data">The data of the character to update</param>
        /// <returns>true if character was updated false otherwise</returns>
        public bool updateCharacter(Data.Character data) {
<<<<<<< HEAD
            NpgsqlCommand command = new NpgsqlCommand("", dbConnection);

            command.CommandText = "UPDATE characters SET ";

            command.CommandText += "is_male= :is_male";
            command.Parameters.AddWithValue(":is_male", data.is_male);

            command.CommandText += ", skin = :skin";
            command.Parameters.AddWithValue(":skin", data.skin);

            command.CommandText += ", health = :health";
            command.Parameters.AddWithValue(":health", data.health);

            command.CommandText += ", x = :x";
            command.Parameters.AddWithValue(":x", data.x);

            command.CommandText += ", y = :y";
            command.Parameters.AddWithValue(":y", data.y);

            command.CommandText += ", z = :z";
            command.Parameters.AddWithValue(":z", data.z);

            command.CommandText += ", rz = :rz";
            command.Parameters.AddWithValue(":rz", data.rz);

            command.CommandText += ", dimension = :dimension";
            command.Parameters.AddWithValue(":dimension", data.dimension);

            command.CommandText += " WHERE id = :id";
            command.Parameters.AddWithValue(":id", data.id);



=======
            MySqlCommand command = buildUpdateCommand(data);
>>>>>>> refs/remotes/RootKiller/master
            return command.ExecuteNonQuery() == 1;
        }
    }
}
