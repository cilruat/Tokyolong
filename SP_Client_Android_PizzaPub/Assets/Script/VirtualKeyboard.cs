using System;
using System.Diagnostics;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

public class VirtualKeyboard : MonoBehaviour {

	[DllImport("user32")]
	static extern IntPtr FindWindow(String sClassName, String sAppName);

	[DllImport("user32")]
	static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

	private static Process _onScreeKeyboardProcess = null;

	public void ShowTouchKeyboard()
	{
		ExternalCall ("C:\\Program Files\\Common Files\\Microsoft Shared\\ink\\tabtip.exe", null, false);
	}

	public void HideTouchKeyboard()
	{
		uint WM_SYSCOMMAND = 274;
		int SC_CLOSE = 61536;
		IntPtr ptr = FindWindow ("IPTip_Main_Window", null);
		PostMessage (ptr, WM_SYSCOMMAND, SC_CLOSE, 0);
	}

	public void ShowOnScreenKeyboard()
	{
		if (_onScreeKeyboardProcess == null || _onScreeKeyboardProcess.HasExited)
			_onScreeKeyboardProcess = ExternalCall ("OSK", null, false);
	}

	public void HideOnScreenKeyboard()
	{
		if (_onScreeKeyboardProcess != null && _onScreeKeyboardProcess.HasExited == false)
			_onScreeKeyboardProcess.Kill ();
	}

	public void RepositionOnScreenKeyboard(Rect rect)
	{
		ExternalCall ("REG", @"ADD HKCU\Software\Microsoft\OSK /v WindowLeft /t REG_DWORD /d " + (int)rect.x + " /f", true);
		ExternalCall ("REG", @"ADD HKCU\Software\Microsoft\OSK /v WindowTop /t REG_DWORD /d " + (int)rect.y + " /f", true);
		ExternalCall ("REG", @"ADD HKCU\Software\Microsoft\OSK /v WindowWidth /t REG_DWORD /d " + (int)rect.width + " /f", true);
		ExternalCall ("REG", @"ADD HKCU\Software\Microsoft\OSK /v WindowHeight /t REG_DWORD /d " + (int)rect.height + " /f", true);
	}

	private static Process ExternalCall(string filename, string arguments, bool hideWindow)
	{
		ProcessStartInfo startInfo = new ProcessStartInfo ();
		startInfo.FileName = filename;
		startInfo.Arguments = arguments;

		if (hideWindow) {
			startInfo.RedirectStandardOutput = true;
			startInfo.RedirectStandardError = true;
			startInfo.UseShellExecute = false;
			startInfo.CreateNoWindow = true;
		}

		Process process = new Process ();
		process.StartInfo = startInfo;
		process.Start ();

		return process;
	}
}