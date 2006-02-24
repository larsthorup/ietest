using System;
using System.Collections;
using mshtml;

namespace BestBrains.IETest
{
	public class DivCollection : IEnumerable
	{
		ArrayList children;
		
		public DivCollection(IEDriver ie, IHTMLDOMChildrenCollection children) 
		{
			this.children = new ArrayList();
			for(int i = 0; i != children.length; ++i)
			{
				try
				{
					Div v = new Div(ie, (HTMLDivElement)children.item(i));
					this.children.Add(v);
				}
				catch(Exception)
				{
					// When an exception is caught, type casting failed so we don't add this element
					// to our list.
				}
			}
		}

		public int length { get { return children.Count; } }

		public Div this[int index] { get { return (Div)children[index]; } }

		public Enumerator GetEnumerator() 
		{
			return new Enumerator(children);
		}

		IEnumerator IEnumerable.GetEnumerator() 
		{
			return GetEnumerator();
		}

		public class Enumerator: IEnumerator 
		{
			ArrayList children;
			int index;
			public Enumerator(ArrayList children) 
			{
				this.children = children;
				Reset();
			}

			public void Reset() 
			{
				index = -1;
			}

			public bool MoveNext() 
			{
				++index;
				return index < children.Count;
			}

			public Div Current 
			{
				get 
				{
					return (Div)children[index];
				}
			}

			object IEnumerator.Current { get { return Current; } }
		}
	}
}