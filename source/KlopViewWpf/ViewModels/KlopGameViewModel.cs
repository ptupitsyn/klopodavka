using System.Collections.Generic;
using System.Linq;
using Common.Commands;
using KlopIfaces;
using KlopViewWpf.Controls;

namespace KlopViewWpf.ViewModels
{
    public class KlopGameViewModel
    {
        public KlopGameViewModel(int fieldWidth, int fieldHeight, IEnumerable<IKlopPlayer> players, int turnLength)
        {
            //_klopModel = new KlopModel.KlopModelAllowDisconnected(fieldWidth, fieldHeight, players, turnLength);
            _klopModel = new KlopModel.KlopModel(fieldWidth, fieldHeight, players, turnLength);
        }

        public IKlopCell ActiveCell
        {
            set { PathHighlighter.HighlightPath(value); }
        }

        public DelegateCommand<IKlopCell> MakeTurnCommand
        {
            get { return _makeTurnCommand ?? (_makeTurnCommand = new DelegateCommand<IKlopCell>(MakeTurn)); }
        }

        public IKlopModel Model
        {
            get
            {
                return _klopModel;
            }
        }

        public HintPathHighlighter PathHighlighter
        {
            get { return _pathHighlighter ?? (_pathHighlighter = new HintPathHighlighter(Model)); }
        }

        public DelegateCommand ResetCommand
        {
            get
            {
                return _resetCommand ?? (_resetCommand = new DelegateCommand(() => Model.Reset()));
            }
        }

        public DelegateCommand<IKlopCell> SetActiveCellCommand
        {
            get { return _setCurrentCellCommand ?? (_setCurrentCellCommand = new DelegateCommand<IKlopCell>(c => ActiveCell = c)); }
        }

        public DelegateCommand UndoCommand
        {
            get { return _undoCommand ?? (_undoCommand = new DelegateCommand(() => Model.UndoTurn())); }
        }

        private void MakeTurn(IKlopCell cell)
        {
            if (cell.Available)
            {
                Model.MakeTurn(cell);
            }
            else if (PathHighlighter.IsHighlighted(cell))
            {
                // Cell is highlighted - perform multiple turns:
                while (Model.RemainingKlops > 1) //TODO: Configurable whether leave one clop or not
                {
                    var currentCell = Model.Cells.FirstOrDefault(c => c.Available && PathHighlighter.IsHighlighted(c));
                    if (currentCell == null) break;
                    Model.MakeTurn(currentCell);
                    if (currentCell == cell) break; // Destination reached
                }
            }
        }

        private readonly IKlopModel _klopModel;
        private DelegateCommand<IKlopCell> _makeTurnCommand;
        private HintPathHighlighter _pathHighlighter;
        private DelegateCommand _resetCommand;
        private DelegateCommand<IKlopCell> _setCurrentCellCommand;
        private DelegateCommand _undoCommand;
    }
}