using System;
using System.IO;

namespace BestBrains.IETest
{
	class CodeGenerator
	{
		[STAThread]
		static void Main(string[] args)
		{
            if (args.Length != 3)
            {
                Console.WriteLine("usage: CodeGenerator.exe <ietest class name> <mshtml class name> <output directory>");
                Console.WriteLine("example: CodeGenerator.exe Table HTMLTable C:\\Temp");

                return;
            }

			string IETestClassName = args[0];
			string HTMLClassName = args[1];
			//string BasePath = new DirectoryInfo(System.Environment.CurrentDirectory).Parent.FullName;
			string TemplateFileName = "Template.txt";
			string CodeFileName = Path.Combine(args[2], IETestClassName + "Collection.cs");
			string Template = new StreamReader(TemplateFileName).ReadToEnd();
			string Code = Template.Replace("%IETestClassName%", IETestClassName).Replace("%HTMLClassName%", HTMLClassName);
			using(StreamWriter sw = new StreamWriter(CodeFileName)) 
			{
				sw.Write(Code);
			}
		}
	}
}
