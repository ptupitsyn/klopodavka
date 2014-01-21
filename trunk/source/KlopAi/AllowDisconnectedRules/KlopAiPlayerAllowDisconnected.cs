using System.Linq;
using KlopAi.Extentions;

namespace KlopAi.AllowDisconnectedRules
{
    /// <summary>
    /// AI player for new rules: can make turn from any live cell.
    /// </summary>
    public class KlopAiPlayerAllowDisconnected : KlopAiPlayerBase
    {
        protected override void MakeTurn()
        {
            while (Model.CurrentPlayer == this && Model.Cells.Any(c => c.Available) && !Worker.CancellationPending)
            {
                if (Model.IsFightStarted())
                {
                    // TODO: kill kill kill
                }

                // TODO: Our own cell evaluator
                //Model.MakeTurn(Model.GenerateStartingPattern(this, c => double.MaxValue));
                Model.MakeTurn(Model.Cells.Where(c => c.Available).Random());
            }
        }

    }
}
