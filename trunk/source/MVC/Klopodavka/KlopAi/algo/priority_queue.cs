using System.Collections;

namespace Clops.Ai.Algo
{
    /// <summary>
    /// Summary description for priority_queue.
    /// </summary>
    public class Priority_queue : Hashtable
    {
        public Node Pop() //0.07 ms
            //Get lowest element
        {
            Node o = new Node(); //lowest object in queue
            double d = double.MaxValue;
            foreach (Node n in Keys)
                if (n.fval < d)
                {
                    o = n;
                    d = o.fval;
                }
            Remove(o);
            return o;
        }

        public Node Pop1() //0.06 ms
            //Get lowest element
        {
            Node[] ar = new Node[Keys.Count];
            Keys.CopyTo(ar, 0);
            int m = 0;
            double d = ar[m].fval;
            for (int i = 1; i < ar.Length; i++ )
                if (ar[i].fval < d)
                {
                    m = i;
                    d = ar[m].fval;
                }
            Node ret = ar[m];
            Remove(ar[m]);
            return ret;
        }
    }
}