namespace Draughts
{
    public class ResultEntry
    {
        public string Nickname { get; }
        public int Result { get; }
        public int ScorePlayerOne { get; }
        public int ScorePlayerTwo { get; }

        public ResultEntry (string nickname, int result, int scorePlayerOne, int scorePlayerTwo)
        {
            Nickname = nickname;
            Result = result;
            ScorePlayerOne = scorePlayerOne;
            ScorePlayerTwo = scorePlayerTwo;
        }
    }
}
