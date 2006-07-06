using System;
using System.Collections;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Reflection;
using System.Windows.Forms;
using NUnit.Framework;
using mshtml;
using SHDocVw;

namespace BestBrains.IETest
{
	public class Element
	{
		private IEDriver ie;
		protected object element;
		public Element(IEDriver ie, object element) 
		{
			this.ie = ie;
			this.element = element;
		}
		public string Id { get { return htmlElement.id; } }
		public string InnerText { get { return htmlElement.innerText; } }
		public string InnerHtml { get { return htmlElement.innerHTML; } }
		public string OuterText { get { return htmlElement.outerText; } }
		public string OuterHtml { get { return htmlElement.outerHTML; } }
		public string TagName { get { return htmlElement.tagName; } }
		public string Title { get { return htmlElement.title; } }
		public Element NextSibling { get { return new Element(ie, domNode.nextSibling); } }
		public Element Parent { get { return new Element(ie, domNode.parentNode); } }
        public Element FirstChild { get { return new Element(ie, domNode.firstChild); } }
        public void Click() { dispHtmlBaseElement.click(); ie.WaitForComplete(); }
		public void Focus() { dispHtmlBaseElement.focus(); FireEvent("onfocus"); }
		public void DoubleClick() { FireEvent("ondblclick"); }
		public void MouseEnter() { FireEvent("onmouseenter"); }
		public void Change() { FireEvent("onchange"); }
		public void FireEvent(string eventName) { ie.FireEvent(dispHtmlBaseElement, eventName); ie.WaitForComplete(); }
		public TableSectionCollection TBODY { get { return new TableSectionCollection(ie, children); } }
		public TableRowCollection TR { get { return new TableRowCollection(ie, children); } }
		public TableCellCollection TD { get { return new TableCellCollection(ie, children); } }
		public SpanCollection SPAN { get { return new SpanCollection(ie, children); } }
		public DivCollection DIV { get { return new DivCollection(ie, children); } }
		public InputCollection INPUT { get { return new InputCollection(ie, children); } }
		public ImgCollection IMG { get { return new ImgCollection(ie, children); } }
		private IHTMLElement htmlElement { get { return (IHTMLElement)element; } }
		private IHTMLDOMNode domNode { get { return (IHTMLDOMNode)element; } }
		private DispHTMLBaseElement dispHtmlBaseElement { get { return (DispHTMLBaseElement)element; } }
		private IHTMLDOMChildrenCollection children { get { return (IHTMLDOMChildrenCollection)(domNode.childNodes); } } // or: use IHTMLDomNode.childNodes
	}
	public class TableSection : Element
	{
		public TableSection(IEDriver ie, HTMLTableSection htmlTableSection) : base(ie, (IHTMLElement)htmlTableSection)
		{
		}
	}
	public class TableRow : Element
	{
		public TableRow(IEDriver ie, HTMLTableRow htmlTableRow) : base(ie, (IHTMLElement)htmlTableRow)
		{
		}
	}
	public class TableCell : Element
	{
		public TableCell(IEDriver ie, HTMLTableCell htmlTableCell) : base(ie, (IHTMLElement)htmlTableCell)
		{
		}
	}
	public class Div : Element
	{
		public Div(IEDriver ie, HTMLDivElement HTMLDivElement) : base(ie, (IHTMLElement)HTMLDivElement)
		{
		}
	}
	public class Input : Element
	{
		public Input(IEDriver ie, HTMLInputElement HTMLInputElement) : base(ie, (IHTMLElement)HTMLInputElement)
		{
		}
		public string Value { 
			get { return ((HTMLInputElement)element).value; } 
			set { ((HTMLInputElement)element).value = value; } 
		}
	}
	public class InputButton : Element
	{
		public InputButton(IEDriver ie, HTMLInputButtonElement inputButtonElement) : base(ie, (IHTMLElement)inputButtonElement)
		{
		}
	}
	public class Img : Element
	{
		public Img(IEDriver ie, HTMLImg HTMLImg) : base(ie, (IHTMLElement)HTMLImg)
		{
		}
	}
	public class Table : Element 
	{
		public Table(IEDriver ie, HTMLTable htmlTable) : base(ie, (IHTMLElement)htmlTable)
		{
		}
	}
	public class Form : Element 
	{
		public Form(IEDriver ie, HTMLFormElement htmlFormElement) : base(ie, (IHTMLElement)htmlFormElement)
		{
		}
	}
	public class Span : Element
	{
		public Span(IEDriver ie, HTMLSpanElement HTMLSpanElement) : base(ie, (IHTMLElement)HTMLSpanElement)
		{
		}
	}
	public class Select : Element
	{
		private IEDriver ie;
		private HTMLSelectElement selectElement;
		public Select(IEDriver ie, HTMLSelectElement selectElement) : base(ie, (IHTMLElement)selectElement)
		{
			this.ie = ie;
			this.selectElement = selectElement;
		}
		
		public Option getSelectedOption() 
		{ 
			IHTMLElementCollection elements = selectElement.getElementsByTagName("OPTION");
			IEnumerator enumerator = elements.GetEnumerator();
			while(enumerator.MoveNext()) 
			{
				HTMLOptionElement o = (HTMLOptionElement)enumerator.Current;
				if(o.selected)
				{
					System.Diagnostics.Debug.WriteLine("Selected found");
					return new Option(ie, o);
				}
			}
			throw new Exception("Could not find selected Option.");
		}
		public OptionCollection OPTION{ get { return new OptionCollection(ie, (IHTMLElementCollection)this.selectElement.getElementsByTagName("OPTION")); } }
	}
	public class Option : Element
	{
		private IEDriver ie;
		private HTMLOptionElement optionElement;

		public Option(IEDriver ie, HTMLOptionElement optionElement) : base(ie, (IHTMLOptionElement)optionElement)
		{
			this.ie = ie;
			this.optionElement = optionElement;
		}

		public bool Selected
		{
			get { return this.optionElement.selected; }
			set { this.optionElement.selected = value; }
		}
	}
	public class Anchor : Element
	{
		public Anchor(IEDriver ie, HTMLAnchorElement anchorElement) : base(ie, (IHTMLElement)anchorElement)
		{
		}
	}
	public class Document : Element
	{
		private IEDriver ie;
		public HTMLDocument htmlDocument;
		public Document(IEDriver ie, HTMLDocument htmlDocument) : base(ie, htmlDocument)
		{
			this.ie = ie;
			this.htmlDocument = htmlDocument;
		}
		public void Close() 
		{
			ie = null;
			htmlDocument = null;
		}
		public Document FindFRAMEDocument(string elementId)
		{
			return new Document(ie, (HTMLDocument)((HTMLIFrame)GetElementById(elementId)).contentWindow.document);
		}
        public string[] Frames
        {
            get
            {
                ArrayList frames = new ArrayList();
                foreach (IHTMLElement e in htmlDocument.all)
                {
                    if (e.tagName.ToLower() == "iframe")
                    {
                        frames.Add(e.id);
                    }
                }
                return (string[])frames.ToArray(typeof(string));
            }
        }
        public TableRow FindTR(string elementId)
		{
			return new TableRow(ie, (HTMLTableRow)GetElementById(elementId));
		}
		public Span FindSPAN(string elementId)
		{
			return new Span(ie, (HTMLSpanElement)GetElementById(elementId));
		}
		public Anchor FindANCHOR(string elementId)
		{
			return new Anchor(ie, (HTMLAnchorElement)GetElementById(elementId));
		}
		public Select FindSELECT(string elementId)
		{
			return new Select(ie, (HTMLSelectElement)GetElementById(elementId));
		}
		public Input FindINPUT(string elementId)
		{
			return new Input(ie, (HTMLInputElement)GetElementById(elementId));
		}
        public Table FindTABLE(string elementId)
		{
			return new Table(ie, (HTMLTable)GetElementById(elementId));
		}
        public Div FindDIV(string elementId)
        {
            return new Div(ie, (HTMLDivElement)GetElementById(elementId));
        }
        public Form FindFORM(string elementId)
		{
			return new Form(ie, (HTMLFormElement)GetElementById(elementId));
		}
		public InputButton FindINPUT_BUTTON(string elementId)
		{
			return new InputButton(ie, (HTMLInputButtonElement)GetElementById(elementId));
		}
		public TableCell FindTD(string elementId)
		{
			return new TableCell(ie, (HTMLTableCell)GetElementById(elementId));
		}
        public IHTMLElement GetElementById(string elementId)
        {
            IHTMLElement htmlElement = htmlDocument.getElementById(elementId);
            if(htmlElement == null)
            {
                throw new ArgumentException(string.Format("Element '{0}' not found; must be one of: {1}", elementId, GetElementListText()));
            }
            return htmlElement;
        }

        private string GetElementListText()
        {
            StringBuilder result = new StringBuilder();
            foreach (IHTMLElement e in htmlDocument.all)
            {
                if (result.Length > 0) result.Append(", ");
                result.Append(e.id);
            }
            return string.Format("[{0}]", result);
        }
		public TableCell FindTD(string elementId, int occurence)
		{
			int nr = -1;
			foreach(IHTMLElement e in htmlDocument.all)
			{
				if(e.id == elementId)
				{
					++nr;
					if(nr == occurence) 
					{
						return new TableCell(ie, (HTMLTableCell)e);
					}
				}
			}
            throw new ArgumentException(string.Format("Occurence {0} of element with id '{1}' not found in '{2}'", occurence, elementId, GetElementListText()));
		}

		public IHTMLElement FindTagFromText(string text, string tagname)
		{
			IHTMLElementCollection elements = htmlDocument.getElementsByTagName(tagname);
			IEnumerator enumerator = elements.GetEnumerator();

			while(enumerator.MoveNext()) 
			{
				IHTMLElement e = (IHTMLElement)enumerator.Current;
				if(e.innerText == text)
				{
					if(e.id != null)
					{
						System.Diagnostics.Debug.WriteLine("Found a " + tagname + " tag from the text: " + text + ". The ID, which you should probably use instead, is: " + e.id);
					}
					return e;
				}
			}
			throw new Exception("Could not find a " + tagname + " tag containing the text: " + text);
		}

		public IHTMLElement FindTagFromName(string name, string tagname)
		{
			IHTMLElementCollection elements = htmlDocument.getElementsByName(name);
			IEnumerator enumerator = elements.GetEnumerator();

			while(enumerator.MoveNext()) 
			{
				IHTMLElement e = (IHTMLElement)enumerator.Current;
				if(e.tagName == tagname)
				{
					if(e.id != null)
					{
						System.Diagnostics.Debug.WriteLine("Found a " + tagname + " tag from the name: " + name + ". The ID, which you should probably use instead, is: " + e.id);
					}
					return e;
				}
			}
			throw new Exception("Could not find a " + tagname + " tag containing the name: " + name);
		}
		public TableRow FindTRFromText(string text)
		{
			return new TableRow(ie, (HTMLTableRow)FindTagFromText(text, "TR"));
		}

		public Span FindSPANFromText(string text)
		{
			return new Span(ie, (HTMLSpanElement)FindTagFromText(text, "SPAN"));
		}
			
		public Anchor FindANCHORFromText(string text)
		{
			return new Anchor(ie, (HTMLAnchorElement)FindTagFromText(text, "A"));
		}
		
		public Select FindSELECTFromText(string text)
		{
			return new Select(ie, (HTMLSelectElement)FindTagFromText(text, "SELECT"));
		}
		
		public Input FindINPUTFromText(string text)
		{
			return new Input(ie, (HTMLInputElement)FindTagFromText(text, "INPUT"));
		}
		public Input FindINPUTFromName(string name)
		{
			return new Input(ie, (HTMLInputElement)FindTagFromName(name, "INPUT"));
		}
		public Table FindTABLEFromText(string text)
		{
			return new Table(ie, (HTMLTable)FindTagFromText(text, "TABLE"));
		}
		public Form FindFORMFromText(string text)
		{
			return new Form(ie, (HTMLFormElement)FindTagFromText(text, "FORM"));
		}
		public TableCell FindTDFromText(string text)
		{
			return new TableCell(ie, (HTMLTableCell)FindTagFromText(text, "TD"));
		}
		
		

		public Document GetPopupDocument() 
		{
			object script = htmlDocument.Script;
			Type scriptType = script.GetType();
			HTMLWindow2 popupWindow = (HTMLWindow2)scriptType.InvokeMember(
				"GetPopupWindow", 
				BindingFlags.Default | BindingFlags.InvokeMethod,
				null,
				script,
				new object[]{});
			Document popupDoc = new Document(ie, (HTMLDocument)popupWindow.document);
			ie.AddWatch(popupDoc);
			return popupDoc;
		}
		public void dumpElements() 
		{
			System.Diagnostics.Debug.WriteLine("Dump:");
			foreach(IHTMLElement e in htmlDocument.all)
			{
				System.Diagnostics.Debug.WriteLine("id = " + e.id);
			}		
		}
		public void dumpElementsElab() 
		{
			System.Diagnostics.Debug.WriteLine("Dump:==================================================");
			foreach(IHTMLElement e in htmlDocument.all)
			{
				System.Diagnostics.Debug.WriteLine("------------------------- " + e.id);
				System.Diagnostics.Debug.WriteLine(e.outerHTML);
			}		
		}
	}



	public class NativeWIN32
	{
		public const int WM_SYSCOMMAND = 0x0112;
		public const int WM_CLOSE = 0x0010;
		public const int SC_CLOSE = 0xF060;

		public delegate bool EnumThreadProc(IntPtr hwnd, IntPtr lParam);

		[DllImport("user32.dll", CharSet=CharSet.Auto)]
		public static extern bool EnumThreadWindows(int threadId, EnumThreadProc pfnEnum, IntPtr lParam);

		// used for an output LPCTSTR parameter on a method call
		[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
		public struct STRINGBUFFER
		{
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst=256)]
			public string szText;
		}

		[DllImport("user32.dll", CharSet=CharSet.Auto)]
		public static extern int GetWindowText(IntPtr hWnd, out STRINGBUFFER ClassName, int nMaxCount);

		[DllImport("user32.dll", CharSet=CharSet.Auto)]
		public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
		
		[DllImport("User32.dll", CharSet=CharSet.Auto)]
		public static extern Boolean EnumChildWindows(IntPtr hWndParent, Delegate lpEnumFunc, int lParam);
	}

	public class PopupWatcher
	{
		public delegate int Callback(IntPtr hwnd,int lParam);

		private int iePid;
		private bool keepRunning;

		private System.Collections.Queue alertQueue;
		private string currentAlertMessage;

		public PopupWatcher(int iePid)
		{
			this.iePid = iePid;
			this.keepRunning = true;
			this.alertQueue = new System.Collections.Queue();
		}

		public int alertCount()
		{
			return alertQueue.Count;
		}

		public string popAlert()
		{
			if(alertQueue.Count == 0)
			{
				throw new MissingAlertException();
			}
			
			return (string)alertQueue.Dequeue();
		}

		public string[] alerts 
		{
			get 
			{
				string[] result = new string[alertQueue.Count];
				Array.Copy(alertQueue.ToArray(), result, alertQueue.Count);
				return result;
			}
		}

		public void flushAlerts()
		{
			alertQueue.Clear();
		}

		public void run()
		{
			while(keepRunning)
			{
				Thread.Sleep(1000);
				
				System.Diagnostics.Process p = System.Diagnostics.Process.GetProcessById(iePid);
				foreach(System.Diagnostics.ProcessThread t in p.Threads)
				{
					int threadId = t.Id;
 
					NativeWIN32.EnumThreadProc callbackProc = new NativeWIN32.EnumThreadProc(MyEnumThreadWindowsProc);
					NativeWIN32.EnumThreadWindows(threadId, callbackProc, IntPtr.Zero /*lParam*/);					
				}
			}
		}

		// callback used to enumerate Windows attached to one of the threads
		private bool MyEnumThreadWindowsProc(IntPtr hwnd, IntPtr lParam)
		{
			// get window caption
			NativeWIN32.STRINGBUFFER sLimitedLengthWindowTitle;
			NativeWIN32.GetWindowText(hwnd, out sLimitedLengthWindowTitle, 256);
			String sWindowTitle = sLimitedLengthWindowTitle.szText;

			if (sWindowTitle.Length==0) return true;

			if(sWindowTitle == "Microsoft Internet Explorer")
			{
//				System.Diagnostics.Debug.WriteLine("Popup detected. Shooting it down.");

				Callback myCallBack = new Callback(EnumChildGetValue);
				NativeWIN32.EnumChildWindows(hwnd,myCallBack,0);
				
				alertQueue.Enqueue(currentAlertMessage);
				NativeWIN32.SendMessage(hwnd, NativeWIN32.WM_CLOSE, 0, 0);
			}
						
			return true;
		}

		public int EnumChildGetValue(IntPtr hwnd,int lParam)
		{
			NativeWIN32.STRINGBUFFER sLimitedLengthWindowTitle;
			NativeWIN32.GetWindowText(hwnd, out sLimitedLengthWindowTitle, 256);

			// Why does this work?
			// It works because the label is the last child window of the messagebox.
			// Hence, when iterating through them all, the last caption (i.e. the label)
			// will remain in the currentAlertMessage variable, which is used above.
			currentAlertMessage = sLimitedLengthWindowTitle.szText;

			return 1;
		}

		public void Stop()
		{
			keepRunning = false;
		}
	}

	public class IEDriver : IDisposable
	{
		private HTMLDocument htmlDocument = null;
		private Document mainDocument = null;
		public HTMLDocument HtmlDocument { 
			get {
				if(htmlDocument == null)
				{
					htmlDocument = (HTMLDocument)ie.Document;
				}
				return htmlDocument;
			} 
		}
		public Document MainDocument { 
			get {
				if(mainDocument == null) 
				{
					mainDocument = new Document(this, HtmlDocument);
				}
				return mainDocument;
			} 
		}

		public SHDocVw.InternetExplorer ie;
		private ArrayList watchDocuments;

		private bool autoClose;

		private PopupWatcher popupWatcher;
		private Thread popupWatcherThread;
		
		public IEDriver(string address, bool autoClose)
		{
			this.autoClose = autoClose;
			
			if(autoClose)
			{
				// We assume that when this flag is on, everything should run automatically.
				// Better move the mouse out of the way.
				System.Windows.Forms.Cursor.Position = new System.Drawing.Point(0, 0);
			}
			
			object o = null;

			ie = new SHDocVw.InternetExplorerClass();
			ie.Visible = true;
			ie.Navigate(address, ref o, ref o, ref o, ref o);

			watchDocuments = new ArrayList();
			
//			ie.DocumentComplete += new DWebBrowserEvents2_DocumentCompleteEventHandler(IE_DocumentComplete);
//			ie += new DWebBrowserEvents2_OnStatusBarEventHandler(myhandler);
			
			WaitForComplete();


			int iePid = 0;
			foreach(System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses()) 
			{
				if(p.MainWindowHandle == (System.IntPtr)ie.HWND) 
				{
					iePid = p.Id;
				}
			}

			popupWatcher = new PopupWatcher(iePid);
			popupWatcherThread = new Thread(new ThreadStart(popupWatcher.run));

			// Start the thread.
			popupWatcherThread.Start();
		

//			IHTMLDocument doc = Document as IHTMLDocument;
//			IDispatchEx dex = doc.Script as IDispatchEx;
//
//			dex.GetDispID("myObj", DispatchExConstants.fdexNameCaseInsensitive | DispatchExConstants.fdexNameEnsure, ref dispId);
//			object myObj = new MyObject();
//
//			dex.InvokeEx(dispId, 
//				DispatchExConstants.LOCALE_USER_DEFAULT, 
//				DispatchExConstants.DISPATCH_PROPERTYPUT, 
//				ref dispParams, 
//				null, 
//				ref ei, 
//				BandObjectSite as BandObjectLib._IServiceProvider);

		}

		// TODO: Implement CaptiaDriver.popAlert
		public string popAlert() 
		{
			return popupWatcher.popAlert();			
		}
		
		public void flushAlerts()
		{
			popupWatcher.flushAlerts();
		}

		public void FireEvent(DispHTMLBaseElement element, string eventName) 
		{
			object dummyEvt = "";
			object parentEvt = htmlDocument.CreateEventObject(ref dummyEvt);
			element.FireEvent(eventName, ref parentEvt);
		}

////////////////////////////////////////////////////////////////////////////////////////////////////
//
//			m_Timer = new System.Timers.Timer(); 
//			m_Timer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimerKillPopup);
//			m_Timer.Interval = 3000; 
//			m_Timer.Enabled = true;
//
//		}
//
//		private System.Timers.Timer m_Timer;
//
//		protected void OnTimerKillPopup(Object source, System.Timers.ElapsedEventArgs e)
//		{
//			System.Diagnostics.Debug.WriteLine("Timer");
//		}
//
////////////////////////////////////////////////////////////////////////////////////////////////////
		
		public void AddWatch(Document doc)
		{
			watchDocuments.Add(doc.htmlDocument);
		}

		public void RemoveWatch(Document doc)
		{
			watchDocuments.Remove(doc.htmlDocument);
		}
		
		// TODO: write a test case that demonstrates that IE does not close by itself
		//		HTMLDocument d = (HTMLDocument)this.ie.Document;
		//		d = null;
		//		System.GC.Collect();
		//		System.GC.Collect();
		public virtual void Close()
		{
			popupWatcher.Stop();
			popupWatcherThread.Join();
			
			htmlDocument = null;
			if(mainDocument != null) mainDocument.Close();
			mainDocument = null;
			int iePid = 0;
			foreach(System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses()) 
			{
				if(p.MainWindowHandle == (System.IntPtr)ie.HWND) 
				{
					iePid = p.Id;
				}
			}
			ie.Quit(); // ask IE to close
			ie = null;
			Thread.Sleep(1000); // wait for IE to close by itself
			try 
			{
				System.Diagnostics.Process.GetProcessById(iePid).Kill(); // force IE to close if needed
				System.Diagnostics.Debug.WriteLine("IE didn't close by itself, so we explicitly killed it");
			}
			catch(ArgumentException) 
			{
				// ignore: IE is no longer running
			}
		}

		public void WaitForComplete() 
		{
			

			// Inspiration:
			// C:\Program Files\ruby\lib\ruby\site_ruby\1.8\watir.rb
		
			while(ie.Busy)
			{
				Thread.Sleep(100);
			}

			while(HtmlDocument.readyState != "complete")
			{
				Thread.Sleep(100);
			}

			// System.Diagnostics.Debug.WriteLine("ie: " + ie.LocationURL);

			for(int i = 0; i != HtmlDocument.frames.length; ++i)
			{
				Object o = i;
				IHTMLWindow2 frame = (IHTMLWindow2)HtmlDocument.frames.item(ref o);
				// System.Diagnostics.Debug.WriteLine("Frame: " + frame.location.pathname + ", Readystate: " + frame.document.readyState);
				while(frame.document.readyState != "complete")
				{
					Thread.Sleep(100);
				}
			}
			
			ArrayList removers = new ArrayList();
			
			foreach(HTMLDocument doc in watchDocuments)
			{
				// System.Diagnostics.Debug.WriteLine("Document: " + doc.location.pathname);
				try
				{
					while(doc.readyState != "complete")
					{
						Thread.Sleep(100);
					}
				}
				catch(COMException e)
				{
					System.Diagnostics.Debug.WriteLine("Removing watched document. Exception: " + e.Message);
					removers.Add(doc);
				}
			}
			foreach(HTMLDocument doc in removers)
			{
				watchDocuments.Remove(doc);
			}
		}
		#region IDisposable Members

		public void Dispose()
		{
			if(autoClose)
			{
				Close();
			}

			if(popupWatcher.alertCount() != 0)
			{
				throw new UnanticipatedAlertsException(popupWatcher.alerts);
			}
		}

		#endregion
	}

	// Se her om external: http://www.codeproject.com/csharp/winformiehost.asp
	
	//	[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
	//	interface ICallUIHandler
	//	{
	//		void PopulateWindow(string selectedPageItem);
	//	}
	//
	//	public class PopulateClass:IPopulateWindow
	//	{
	//		SampleEventProgram.Form1 myOwner;
	//
	//		/// <SUMMARY>
	//		/// Requires a handle to the owning form
	//		/// </SUMMARY>
	//		public PopulateClass(SampleEventProgram.Form1 ownerForm)
	//		{
	//			myOwner = ownerForm;
	//		}
	//
	//		/// <SUMMARY>
	//		/// Looks up the string passed and populates the form
	//		/// </SUMMARY>
	//		public void PopulateWindow(string itemSelected)
	//		{
	//			// insert logic here
	//		}
	//	}
	public class MissingAlertException : Exception 
	{
	}
	public class UnanticipatedAlertsException : Exception 
	{
		public UnanticipatedAlertsException(string[] alerts)
		{
			this.alerts = alerts;
		}
		public string[] alerts;
	}
}
