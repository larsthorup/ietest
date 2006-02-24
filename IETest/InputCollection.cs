using System;
using System.Collections;
using mshtml;

namespace BestBrains.IETest
{
	public class InputCollection : IEnumerable
	{
		ArrayList children;
		
		public InputCollection(IEDriver ie, IHTMLDOMChildrenCollection children) 
		{
			this.children = new ArrayList();
			for(int i = 0; i != children.length; ++i)
			{
				try
				{
					Input v = new Input(ie, (HTMLInputElement)children.item(i));
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

		public Input this[int index] { get { return (Input)children[index]; } }

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

			public Input Current 
			{
				get 
				{
					return (Input)children[index];
				}
			}

			object IEnumerator.Current { get { return Current; } }
		}
	}
}