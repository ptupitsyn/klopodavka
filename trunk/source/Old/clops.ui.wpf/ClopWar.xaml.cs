using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Clops.Ai;
using clops.drawing.wpf;

namespace clops.ui.wpf
{
    /// <summary>
    /// Interaction logic for ClopWarWpf.xaml
    /// </summary>
    public partial class ClopWarWpf
    {
        private readonly ClopCPU _clopCPU; //Controller
        private readonly WpfClopDrawer _clopDrawer; //View
        private readonly ClopWar _clopWar; //Model

        public ClopWarWpf()
        {
            InitializeComponent();

            //_clopDrawer = new WpfClopDrawer(cnvBattle);
            //_clopWar = new ClopWar(_clopDrawer);
            //_clopDrawer.InitForm(_clopWar);
            //_clopCPU = new ClopCPU(_clopWar);


            for (int i = 0; i < gridClops.Columns*gridClops.Rows; i++)
            {
                gridClops.Children.Add(new Frame { Source = clopRed.Source });
                //gridClops.Children.Add(new Ellipse {Stroke = Brushes.Black});
            }
        }
    }
}