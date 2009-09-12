namespace Clops.Ifaces
{
    public interface IClopWar
    {
        IClopCell[,] Field { get; set; }

        int clopNum { get; }

        int turn { get; }

        int clopLeft { get; }

        int gameStatus { get; }

        void ResetGame();

        bool CheckBound(int x, int y) //True if (x,y) in Field
            ;

        bool MakeMove(int i, int j);
        void UndoTurn();
        void SwitchTurn();
        void SwitchTurn(bool force, bool setRed, bool setBlue);
        void StartRecording();
        void StopRecording(string fln);
        void RecordTurn(int x, int y);
        int PlayRecord(string Fln, int TurnNum);
        double Distance(int x1, int y1, int x2, int y2);
    }
}
