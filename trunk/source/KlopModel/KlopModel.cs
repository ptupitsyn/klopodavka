using System.Collections.Generic;
using KlopIfaces;

namespace KlopModel
{
    /// <summary>
    /// Default implementation of game rules: must be connected to the base.
    /// </summary>
    public class KlopModel : KlopModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KlopModel"/> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="players">The players.</param>
        /// <param name="turnLenght">The turn lenght.</param>
        public KlopModel(int width, int height, IEnumerable<IKlopPlayer> players, int turnLenght)
            : base(width, height, players, turnLenght)
        {
        }
    }
}