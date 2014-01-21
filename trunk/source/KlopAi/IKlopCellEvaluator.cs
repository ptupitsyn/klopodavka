using System;
using KlopIfaces;

namespace KlopAi.DefaultRules
{
    public interface IKlopCellEvaluator
    {
        /// <summary>
        /// Calculates cells Cost for specified player.
        /// </summary>
        /// <param name="klopPlayer">The klop player.</param>
        /// <param name="callback">Callback for evaluation result.</param>
        void EvaluateCells(IKlopPlayer klopPlayer, Action<IKlopCell, double> callback);
    }
}