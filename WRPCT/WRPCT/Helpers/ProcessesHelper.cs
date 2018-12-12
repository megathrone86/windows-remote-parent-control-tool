using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace WRPCT.Helpers
{
    public class ProcessesHelper
    {
        public static IEnumerable<ProcessInfo> GetProcesses()
        {
            var allProcesses = Process.GetProcesses();
            foreach (var process in allProcesses)
            {
                string filePath = null;
                try
                {
                    filePath = process.MainModule.FileName;
                }
                catch { }
                if (filePath != null)
                    yield return new ProcessInfo(filePath, GetProcessUser(process));
            }
        }

        static string GetProcessUser(Process process)
        {
            IntPtr processHandle = IntPtr.Zero;
            try
            {
                OpenProcessToken(process.Handle, 8, out processHandle);
                WindowsIdentity wi = new WindowsIdentity(processHandle);
                string user = wi.Name;
                return user.Contains(@"\") ? user.Substring(user.IndexOf(@"\") + 1) : user;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (processHandle != IntPtr.Zero)
                {
                    CloseHandle(processHandle);
                }
            }
        }

        [DllImport("advapi32.dll", SetLastError = true)]
        static extern bool OpenProcessToken(IntPtr ProcessHandle, uint DesiredAccess, out IntPtr TokenHandle);
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CloseHandle(IntPtr hObject);
    }

    public class ProcessInfo
    {
        public string FilePath { get; }
        public string FileName { get; }
        public string UserName { get; }

        public ProcessInfo(string filePath, string userName)
        {
            this.FilePath = filePath;
            this.UserName = userName;
            this.FileName = Path.GetFileName(filePath);
        }
    }
}
