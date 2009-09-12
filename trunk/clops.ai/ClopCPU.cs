/////////////////////////////
/// code by TPS
/// tps0@hotmail.com
/////////////////////////////

using System;
using System.Collections;
using Clops.Ai.Algo;
using Clops.Ifaces;

namespace Clops.Ai
{
    /// <summary>
    /// Acts as CPU player
    /// Artificial intelligence
    /// Interacts with <see cref="ClopWar"/> class
    /// </summary>
    public class ClopCPU
    {
        #region Class header

        //Strategy file
        public string StrategyFile { get; set;}
        //Options
        public bool AdvancedDefence { get; set; }
        public bool AdvancedPath { get; set; }
        //Vars
        private readonly ClopWar _clpWar;
        private int _side; //Side
        private int _xside; //Opposite side
        private int _bx; //Base coords
        private int _by;
        private int _ebx; //Enemy base
        private int _eby;
        private ArrayList _bestpath; //Store best path sequence
        private bool _beginning; //Indicates state of the game

        //Const
        private const int TURN_EMPTY = 100; //Цена хода в пустую клетку
        private const int TURN_EAT = 35; //Цена хода в занятую клетку
        private const int TURN_EATENEMYBASE = 8; //Цена съедания клопа около вражеской базы
        private const int TURN_EATOWNBASE = 5; //Цена съедания клопа около своей базы
        private const int TURN_NEARBASE = 11000; //Цена хода около своей базы
        private const int TURN_BLOCKED = 1000000; //Цена хода в запрещенную клетку (->inf)
        //Same for values (not costs)
        private const int TURN_EAT2 = -100; //Цена хода в занятую клетку
        private const int TURN_EATENEMYBASE2 = -1000; //Цена съедания клопа около вражеской базы
        private const int TURN_EATOWNBASE2 = -1100; //Цена съедания клопа около своей базы
        private const int TURN_NEARBASE2 = 1000; //Цена хода около своей базы

        //Constructor
        public ClopCPU(ClopWar cw)
        {
            _clpWar = cw;
            AdvancedPath = true; //??

            //Prepare pathfinder
            //PathFinder.Dist = dist;
            //PathFinder.NodeXY = nodexy;
        }

        #endregion

        private void evalCells()
        {
            int i;
            int j;
            double v;
            IClopCell c;

            //Find price for every cell
            for (i = 0; i < ClopWar.FieldW; i++)
                for (j = 0; j < ClopWar.FieldH; j++)
                {
                    //A*
                    _clpWar.Field[i, j].cost = TURN_EMPTY;
                    _clpWar.Field[i, j].Parent = null;
                    _clpWar.Field[i, j].inpath = false;
                    _clpWar.Field[i, j].visited = false;

                    if (_clpWar.Field[i, j].Owner == _side)
                    {
                        v = Double.MaxValue;
                        //A*
                        _clpWar.Field[i, j].cost = 0; //Ход в свою клетку бесплатный
                    }
                    else
                    {
                        c = _clpWar.Field[i, j];
                        v = Distance(_ebx, _eby, i, j);
                        if (c.Owner == _xside)
                        {
                            v += TURN_EAT2;
                        }
                        //A*
                        if (c.Owner == _xside)
                            _clpWar.Field[i, j].cost = TURN_EAT;
                        if ((c.Owner == _xside) & ((c.State == Cell.DEAD) | (c.State == Cell.BASE)))
                            _clpWar.Field[i, j].cost = TURN_BLOCKED;
                    }

                    _clpWar.Field[i, j].Value = v;
                }

            //Check cells near enemy base - eat everything near it.
            int[,] carr =
                new int[8,2]
                    {
                        {_ebx - 1, _eby - 1}, {_ebx - 1, _eby}, {_ebx - 1, _eby + 1}, {_ebx, _eby - 1}, {_ebx, _eby + 1},
                        {_ebx + 1, _eby - 1}, {_ebx + 1, _eby + 1}, {_ebx + 1, _eby}
                    };
            for (int k = 0; k < 8; k++)
            {
                i = carr[k, 0];
                j = carr[k, 1];
                if (_clpWar.Field[i, j].Owner == _xside)
                    _clpWar.Field[i, j].Value = TURN_EATENEMYBASE2; //Best turn except of eating near own base
                else
                    _clpWar.Field[i, j].Value = TURN_NEARBASE2; //NEVER move here - it only helps your opponent

                //A*
                if ((_clpWar.Field[i, j].Owner == _xside) & (_clpWar.Field[i, j].State != Cell.DEAD))
                    _clpWar.Field[i, j].cost = TURN_EATENEMYBASE; //Best turn except of eating near own base
                else
                {
                    if (_clpWar.Field[i, j].Owner == Cell.EMPTY)
                        _clpWar.Field[i, j].cost = TURN_NEARBASE; //NEVER move here - it only helps your opponent
                }
            }
            //----

            //Try NOT to move to any cell near own base
            int[,] carr2 =
                new int[8,2]
                    {
                        {_bx - 1, _by - 1}, {_bx - 1, _by}, {_bx - 1, _by + 1}, {_bx, _by - 1}, {_bx, _by + 1}, {_bx + 1, _by - 1},
                        {_bx + 1, _by + 1}, {_bx + 1, _by}
                    };
            for (int k = 0; k < 8; k++)
            {
                i = carr2[k, 0];
                j = carr2[k, 1];
                if (_clpWar.Field[i, j].Owner == _xside)
                    _clpWar.Field[i, j].Value = TURN_EATOWNBASE2; //Really the Best turn
                else
                    _clpWar.Field[i, j].Value = TURN_NEARBASE2; //NEVER move here except of when there is nowhere to move

                //A*
                if ((_clpWar.Field[i, j].Owner == _xside) & (_clpWar.Field[i, j].State != Cell.DEAD))
                    _clpWar.Field[i, j].cost = TURN_EATOWNBASE; //Really the Best turn
                else
                {
                    if (_clpWar.Field[i, j].Owner == Cell.EMPTY)
                        _clpWar.Field[i, j].cost = TURN_NEARBASE;
                    //NEVER move here except of when there is nowhere to move
                }
            }
            //----
        }

        #region AuxFnc

        ////Functions required for A_STAR
        //private double dist(node n1, node n2)
        //{
        //    return Distance(n1.px, n1.py, n2.px, n2.py);
        //}

        //private IClopNode nodexy(int x, int y)
        //{
        //    if ((x >= 0) & (x < ClopWar.FieldW) & (y >= 0) & (y < ClopWar.FieldH))
        //        return _clpWar.Field[x, y];
        //    else
        //        return null;
        //}

        private bool checkNeighbours(int x, int y, int depth)
        {
            for (int i = x - depth; i <= x + depth; i++)
                for (int j = y - depth; j <= y + depth; j++)
                    if (_clpWar.CheckBound(i, j))
                        if (_clpWar.Field[i, j].Owner == _xside)
                            return true;
            return false;
        }

        public static double Distance(int x1, int y1, int x2, int y2)
        {
            int dx = x1 - x2;
            int dy = y1 - y2;
            return Math.Sqrt(dx*dx + dy*dy);
        }

        #endregion

        #region PathFinder

        //Find path using A-Star Pathfinder
        private void findPath()
        {
            //GO!
            var finder = new ClopPathFinder(_clpWar.Field);
            IClopNode bestn = finder.FindPath(_ebx, _eby, _bx, _by);

            //Write found path into bestpath
            _bestpath = new ArrayList();
            IClopNode n = bestn;
            while (n != null)
            {
                n.inpath = true;
                _bestpath.Add(n);
                n = n.Parent;
            }
        }

        private double findPathLength()
        {
            //GO!
            var finder = new ClopPathFinder(_clpWar.Field);
            IClopNode bestn = finder.FindPath(_ebx, _eby, _bx, _by);

            //Count path length
            //int len = 0;
            IClopNode n = bestn;
            /*while (n!=null)
			{
				n=n.Parent;
				len++;
			}*/

            return n.gdist;
        }

        #endregion

        public void Stage1Array()
        {
            #region Pre-battle array forming

            _beginning = true;
            while ((_clpWar.turn == _side) & _beginning)
            {
                //Analyse Field: what's happening (beginning of the game or battle)			
                _beginning = true;
                for (int i = 0; (i < ClopWar.FieldW); i++)
                    for (int j = 0; (j < ClopWar.FieldH); j++)
                    {
                        //if (((_clpWar.Field[i,j].state==cell.DEAD) & (_clpWar.Field[i,j].owner!=cell.EMPTY)) | ((_clpWar.Field[i,j].avail)&(_clpWar.Field[i,j].owner==_xside)))
                        if ((_clpWar.Field[i, j].Owner == _side) & checkNeighbours(i, j, _clpWar.clopNum/3))
                        {
                            _beginning = false;
                        }
                    }

                //Prepare pre-battle array
                if (_beginning)
                {
                    if (StrategyFile != null)
                    {
                        if (_clpWar.PlayRecord(StrategyFile, 1) == 0) break;
                    }
                    else break;
                }

                /**/
            }

            #endregion
        }

        public bool Stage2Defence()
        {
            #region Advanced Defence

            bool rs = false;

            if (AdvancedDefence)
            {
                int os = _side;
                int ox = _xside;
                _side = ox;
                _xside = os;

                //Maximum for this phase (to speed up thinking and to leave some clops for next phase
                //int maxmov=(int)(_clpWar.clopNum/2);  

                if ((!_beginning) & (_clpWar.clopLeft >= 1))
                {
                    //maxmov--;

                    double pathl = double.MinValue;
                    int bestx = -1;
                    int besty = -1;
                    double pl;
                    double mpl = double.MaxValue;

                    //int i;
                    //Parallel.For(0, ClopWar.FieldW, i =>
                    {
                        for (int i = 0; (i < ClopWar.FieldW); i++)
                            for (int j = 0; (j < ClopWar.FieldH); j++)
                                if (_clpWar.Field[i, j].Avail && (_clpWar.Field[i, j].Owner == _side))
                                {
                                    _clpWar.Field[i, j].Owner = _xside;
                                    _clpWar.Field[i, j].State = Cell.DEAD;
                                    evalCells();
                                    pl = findPathLength();

                                    if (pl > pathl) //Find max
                                    {
                                        pathl = pl;
                                        bestx = i;
                                        besty = j;
                                    }

                                    if (pl < mpl) //Find min
                                        mpl = pl;

                                    _clpWar.Field[i, j].Owner = _side;
                                    _clpWar.Field[i, j].State = Cell.CLOP;
                                }
                    }//);

                    if (_clpWar.CheckBound(bestx, besty) && ((int) (pathl - mpl) >= (TURN_EAT)))
                    {
                        _clpWar.MakeMove(bestx, besty);
                        rs = true;
                    }
                }

                _side = os;
                _xside = ox;
                evalCells();
            }
            return rs;

            #endregion
        }

        public bool Stage3Path()
        {
            #region Path finding

            bool rs = false;

            if (AdvancedPath /*& (!AdvancedDefence)*/)
            {
                //Find path
                findPath();

                //Make turn according to the path
                IEnumerator e = _bestpath.GetEnumerator();
                e.Reset();
                while ((_clpWar.turn == _side) & (e.MoveNext()) & (rs == false))
                {
                    rs = _clpWar.MakeMove(((Node) e.Current).px, ((Node) e.Current).py);
                }
            }

            return rs;

            #endregion
        }

        public bool Stage4Kill()
        {
            bool rs = false;

            int tx = -1;
            int ty = -1; //Find Best Turn Coords
            double tv;

            //Do other logic
            if (_clpWar.turn == _side)
            {
                tv = double.MaxValue;
                for (int i = 0; (i < ClopWar.FieldW); i++)
                    for (int j = 0; (j < ClopWar.FieldH); j++)
                    {
                        if ((_clpWar.Field[i, j].Value <= tv) & _clpWar.Field[i, j].Avail)
                        {
                            tx = i;
                            ty = j;
                            tv = _clpWar.Field[i, j].Value;
                        }
                    }

                //Make the best found move
                rs = _clpWar.MakeMove(tx, ty);
            }

            return rs;
        }

        //Make turn
        public void Turn()
        {
            _side = _clpWar.turn; //Our side
            _xside = Cell.RED; //Opposite side
            if (_side == Cell.RED) _xside = Cell.BLUE;

            //Find bases coords
            if (_side == Cell.RED)
            {
                _bx = ClopWar.RedBaseX;
                _by = ClopWar.RedBaseY;
                _ebx = ClopWar.BlueBaseX;
                _eby = ClopWar.BlueBaseY;
            }
            else
            {
                _bx = ClopWar.BlueBaseX;
                _by = ClopWar.BlueBaseY;
                _ebx = ClopWar.RedBaseX;
                _eby = ClopWar.RedBaseY;
            }

            //Pre-battle array
            Stage1Array();

            //Evaluate cells
            evalCells();

            //Logic cycle			
            while (_clpWar.turn == _side)
            {
                while (Stage2Defence())
                {
                }
                Stage3Path();
                if (!Stage2Defence())
                    if (!Stage3Path())
                        Stage4Kill();
            }
        }
    }
}
