using System;
using System.IO;

namespace BestBrains.IETest
{
	class CodeGenerator
	{
		[STAThread]
		static void Main(string[] args)
		{
			string IETestClassName = args[0];
			string HTMLClassName = args[1];
			string BasePath = new DirectoryInfo(System.Environment.CurrentDirectory).Parent.FullName;
			string TemplateFileName = BasePath + @"\CodeGenerator\Template.txt";
			string CodeFileName = BasePath + @"\IETest\" + IETestClassName + "Collection.cs";
			string Template = new StreamReader(TemplateFileName).ReadToEnd();
			string Code = Template.Replace("%IETestClassName%", IETestClassName).Replace("%HTMLClassName%", HTMLClassName);
			using(StreamWriter sw = new StreamWriter(CodeFileName)) 
			{
				sw.Write(Code);
			}
		}
	}
}
