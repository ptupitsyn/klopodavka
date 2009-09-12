using Clops.Ai;
using clops.drawing.wpf;

namespace clops.ui.wpf
{
  /// <summary>
  /// Interaction logic for ClopWarWpf.xaml
  /// </summary>
  public partial class ClopWarWpf
  {
    private readonly ClopWar _clopWar;              //Model
    private readonly WpfClopDrawer _clopDrawer;     //View
    private readonly ClopCPU _clopCPU;              //Controller

    public ClopWarWpf()
    {
      InitializeComponent();

      _clopDrawer = new WpfClopDrawer(cnvBattle);
      _clopWar = new ClopWar(_clopDrawer);
      _clopDrawer.InitForm(_clopWar);
      _clopCPU = new ClopCPU(_clopWar);
    }
  }
}
