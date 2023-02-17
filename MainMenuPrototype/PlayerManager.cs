using System.Data.SQLite;

namespace MainMenuPrototype
{
    /// <summary>
    /// Interfaces with the DB and manages the 'Players' table (insert and select)
    /// </summary>
    internal class PlayerManager
    {
        private SQLiteConnection connection;
        public PlayerManager(SQLiteConnection conn)
        {
            connection = conn;
        }
        public Player FindOrCreatePlayer(string usernameToSearch)
        {
            Player player;

            SQLiteParameter usernameParam = new SQLiteParameter("username", usernameToSearch);

            SQLiteCommand findPlayerCommand = new SQLiteCommand(
                "SELECT * FROM Players WHERE Username=@username", connection);
            SQLiteCommand createPlayerCommand = new SQLiteCommand(
                "INSERT INTO Players (Username, BestScore, TotalScore) " +
                "VALUES" +
                "(@username,0,0);", connection);
            findPlayerCommand.Parameters.Add(usernameParam);
            createPlayerCommand.Parameters.Add(usernameParam);

            using (SQLiteDataReader reader = findPlayerCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    string username = reader.GetString(reader.GetOrdinal("Username"));
                    int best_score = reader.GetInt32(reader.GetOrdinal("BestScore"));
                    int total_score = reader.GetInt32(reader.GetOrdinal("TotalScore"));
                    if (username == usernameToSearch)
                    {
                        player = new Player(username, best_score, total_score);
                        return player;
                    }
                }
            }

            // The code block below executes ONLY when the Player is NOT found
            createPlayerCommand.ExecuteNonQuery();
            player = new Player(usernameToSearch, 0, 0);
            return player;
        }
    }
}
