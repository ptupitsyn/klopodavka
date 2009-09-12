using System;
using System.Collections;

namespace Clops_
{
	/// <summary>
	/// Summary description for priority_queue.
	/// </summary>
	public class Priority_queue : ArrayList
	{
		public Priority_queue()
		{
			// TODO: Add constructor logic here
		}

		public object Pop()
			//Get lowest element
		{
			IEnumerator e = this.GetEnumerator();
			IComparable o; //lowest object in queue
			IComparable o1;
			e.Reset();
			e.MoveNext();
			o=(IComparable)e.Current;
			while (e.MoveNext())
			{
				o1=(IComparable)e.Current;
				if (o1.CompareTo(o)<0)
					o=o1;
			}
			this.Remove(o);
			return (object)o;
		}
	}
}
