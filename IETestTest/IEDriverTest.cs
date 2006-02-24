using System;
using System.IO;
using NUnit.Framework;
using BestBrains.IETest;

namespace BestBrains.IETestTest
{
	[TestFixture]
	public class IEDriverTest
	{
		static string TestDataURI = new DirectoryInfo(System.Environment.CurrentDirectory).Parent.Parent.FullName + @"\testdata\";
		static bool CloseIE = true;

		public IEDriverTest()
		{
		}

		[TestFixtureSetUp]
		public void Setup()
		{
			System.Threading.Thread.CurrentThread.ApartmentState = System.Threading.ApartmentState.STA;
			
		}

		[Test]
		public void NUnitGUITest() 
		{
			using(IEDriver ie = new IEDriver(TestDataURI + "main.html", CloseIE)) 
			{
			}
		}

		[Test]
		public void GoogleTest()
		{
			using(IEDriver ie = new IEDriver("http://www.google.com", CloseIE))
			{
				BestBrains.IETest.Input q = ie.MainDocument.FindINPUT("q");
				q.Value = "BestBrains";
				
				BestBrains.IETest.InputButton btnG = ie.MainDocument.FindINPUT_BUTTON("btnG");
				btnG.Click();
			}
		}

		[Test]
		public void ContentTest() 
		{
			using(IEDriver ie = new IEDriver(TestDataURI + "main.html", CloseIE)) 
			{
				Assert.AreEqual("hello", ie.MainDocument.FindINPUT("hello").Value);

				// Collection.length
				Assert.AreEqual(3, ie.MainDocument.FindFORM("form").INPUT.length);

				// Collection[]
				Assert.AreEqual("name", ie.MainDocument.FindFORM("form").INPUT[0].Id);
				Assert.AreEqual("popup", ie.MainDocument.FindFORM("form").INPUT[1].Id);
				Assert.AreEqual("hello", ie.MainDocument.FindFORM("form").INPUT[2].Id);

				// Collection iteration
				int count = ie.MainDocument.FindFORM("form").INPUT.length;

				Assert.AreEqual(3, count);

				Assert.AreEqual(2, ie.MainDocument.FindFORM("collectionTestForm").INPUT.length);
				Assert.AreEqual("first", ie.MainDocument.FindFORM("collectionTestForm").INPUT[0].Value);
				Assert.AreEqual("second", ie.MainDocument.FindFORM("collectionTestForm").INPUT[1].Value);

				// accessing several occurences with equal id
				Assert.AreEqual("a1", ie.MainDocument.FindTD("td1", 0).InnerText);
				Assert.AreEqual("b1", ie.MainDocument.FindTD("td1", 1).InnerText);
			}
		}
		
		[Test]
		public void FindFromTextTest()
		{
			using(IEDriver ie = new IEDriver(TestDataURI + "main.html", CloseIE)) 
			{
				Assert.AreEqual("td1", ie.MainDocument.FindTDFromText("a1").Id);
				Assert.IsNull(ie.MainDocument.FindTDFromText("a3").Id);
				
				try
				{
					ie.MainDocument.FindTDFromText("non-existing tag");
					Assert.Fail("FindTDFromText did not fail during search of non-existing tag.");
				}
				catch(Exception)
				{
				}
			}
		}
		
//		[Test]
		public void ModelessDialogTest() 
		{
			using(IEDriver ie = new IEDriver(TestDataURI + "main.html", CloseIE)) 
			{
				ie.MainDocument.FindINPUT_BUTTON("popup").Click();
				Document dialog = ie.MainDocument.GetPopupDocument();
				Assert.AreEqual("47", dialog.FindINPUT("dims").Value);
			}
		}

//		[Test]
		public void AlertTest() 
		{
			using(IEDriver ie = new IEDriver(TestDataURI + "main.html", CloseIE)) 
			{
				ie.MainDocument.FindINPUT_BUTTON("hello").Click();
				
				// getting alert text
				Assert.AreEqual("hello", ie.popAlert());

				// expected alert was missing
				try 
				{
					ie.popAlert();
					Assert.Fail("Expected MissingAlertException");
				}
				catch(MissingAlertException) 
				{
					// expected; ignore
				}
			}

			// unanticipated alert detected
			try 
			{
				using(IEDriver ie = new IEDriver(TestDataURI + "main.html", CloseIE)) 
				{
					ie.MainDocument.FindINPUT_BUTTON("hello").Click();
				}
				Assert.Fail("Expected UnanticipatedAlertsException");
			}
			catch(UnanticipatedAlertsException e) 
			{
				Assert.AreEqual(1, e.alerts.Length);
				Assert.AreEqual("hello", e.alerts[0]);
			}
		}
	}
}
