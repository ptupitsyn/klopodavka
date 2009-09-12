using System.Windows.Controls;
using Clops.Ifaces;

namespace clops.drawing.wpf
{
  public class WpfClopDrawer : IClopDrawer
  {
    private readonly Canvas _baseCanvas;
    private IClopWar _clopWar;

    public WpfClopDrawer(Canvas baseCanvas)
    {
      _baseCanvas = baseCanvas;
    }

    public void InitForm(IClopWar clopWar)
    {
      _clopWar = clopWar;
    }

    public void Refresh()
    {
      //_baseCanvas
    }
  }
}
