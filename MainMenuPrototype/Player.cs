namespace MainMenuPrototype
{
    /// <summary>
    /// Simple data model that mirrors the 'Players' table in the database
    /// </summary>
    public class Player
    {
        public string Username { get; set; }
        public int BestScore { get; set; }
        public int TotalScore { get; set; }

        public Player (string username, int best_score, int total_score)
        {
            Username = username;
            BestScore = best_score;
            TotalScore = total_score;
        }
    }
}
