using System;

namespace Clops.Ifaces
{
    public interface IClopNode : IComparable
    {
        IClopNode Parent { get; set; }
        double gdist { get; set; }
        double hdist { get; set; }
        double cost { get; set; }
        bool inpath { get; set; }
        bool visited { get; set; }
        int px { get; set; }
        int py { get; set; }
        double fval { get; }

        IClopNode Clone();
    }

    public interface IClopCell : IClopNode
    {
        int Owner { get; set; }
        int State { get; set; }
        bool Flag { get; set; }
        bool Avail { get; set; }
        double Value { get; set; }
    }
}