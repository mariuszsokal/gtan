#pragma warning disable CS0649
namespace Database.Data {
    /// <summary>
    /// User account data.
    /// </summary>
    [DBTable("users")]
    public class User {
        /// <summary>
        /// Invalid account id constant.
        /// </summary>
        public const long INVALID_ACCOUNT_ID = 0;

        /// <summary>
        /// The unique id of the account.
        /// </summary>
        [DBField("id")]
        public long id = INVALID_ACCOUNT_ID;

        /// <summary>
        /// The lower cased name of the account.
        /// </summary>
        [DBField("username")]
        public string username;

        /// <summary>
        /// Encoded password.
        /// </summary>
        [DBField("password")]
        public string password;
    }
}
