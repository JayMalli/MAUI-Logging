using System.Diagnostics;
using System.Reflection;
using System.Runtime.ExceptionServices;
namespace logs;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
		AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;
	}

	private void OnCounterClicked(object sender, EventArgs e)
	{
		throw new Exception("Counter Button Clicked!");
	}

	private void OnGetFilesClicked(object sender, EventArgs e)
	{
		try
		{
			string path = "/demo_logs";
			Directory.GetFiles(path);
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
		}
	}

	private void CurrentDomain_FirstChanceException(object sender, FirstChanceExceptionEventArgs e)
	{
		if (e.Exception is Exception ex)
		{
			LogException(ex);
		}
	}

	private void LogException(Exception ex)
	{
		string logsFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app_logs.txt");
		try
		{
			StackFrame sf = new StackFrame(1);
			MethodBase method = sf.GetMethod();
			int line = sf.GetFileLineNumber();
			string exMsg = Environment.CurrentManagedThreadId + " ~ " + DateTime.Now.ToString() + method.DeclaringType.FullName + "::" + line + "-" + GetErrorMessage(ex);
			File.AppendAllText(logsFilePath, exMsg + Environment.NewLine + Environment.NewLine);
		}
		catch (Exception exe)
		{
			Console.WriteLine(exe.Message);
		}
	}

	private string GetErrorMessage(Exception ex)
	{
		if (ex != null)
		{
			string message = ex.StackTrace + " " + ex.Message;
			if (ex.InnerException != null)
			{
				message += "~ Caused By" + GetErrorMessage(ex.InnerException);
			}
			return message;
		}

		return string.Empty;
	}

}

