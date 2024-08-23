using System;
using System.Collections.Generic;

namespace PS3Lib
{
    public delegate void OnError(ErrorCodes code);

    public static class APISingleton
    {
        public static CCAPI CC_API;
        public static TMAPI TM_API;
        public static MAPI M_API;
    }

    public class PS3API
    {
        public static string TargetName { get; internal set; }
        public static string TargetIp { get; internal set; }

        public static OnError OnError;

        public bool IsAttached
        {
            get;
            internal set;
        }

        internal SelectAPI currentAPI_internal;

        public SelectAPI CurrentAPI
        {
            get { return currentAPI_internal; }
            set
            {
                currentAPI_internal = value;
                MakeInstanceAPI(currentAPI_internal);
            }
        }

        public PS3API(SelectAPI API = SelectAPI.TargetManager)
        {
            CurrentAPI = API;
        }

        internal bool isConnected()
        {
            if (CurrentAPI == SelectAPI.ControlConsole)
            {
                int status = CCAPI.GetConnectionStatus();
                return true;
            }
            else if (CurrentAPI == SelectAPI.ManagerAPI)
            {
                return MAPI.IsConnected;
            }
            else
            {
                return TMAPI.SCE.GetStatus() == ConnectStatus.Connected;
            }
        }

        public bool IsConnected
        {
            get => isConnected();
        }

        private void MakeInstanceAPI(SelectAPI API)
        {
            if (API == SelectAPI.TargetManager)
            {
                APISingleton.TM_API = new TMAPI();
            }
            else if (API == SelectAPI.ControlConsole)
            {
                APISingleton.CC_API = new CCAPI();
            }
            else
            {
                APISingleton.M_API = new MAPI();
            }
        }

        /// <summary>init again the connection if you use a Thread or a Timer.</summary>
        public void InitTarget()
        {
            if (CurrentAPI == SelectAPI.TargetManager)
                APISingleton.TM_API.InitComms();
            else
                OnError?.Invoke(ErrorCodes.TMAPI_NOT_SELECTED);
        }

        public List<CCAPI.ConsoleInfo> GetCCAPIConsoleList()
        {
            if (CurrentAPI == SelectAPI.ControlConsole)
                return APISingleton.CC_API.GetConsoleList();
            return null;
        }

        /// <summary>Connect your console with selected API.</summary>
        public bool ConnectTarget(int target = 0)
        {
            // We'll check again if the instance has been done, else you'll got an exception error.
            MakeInstanceAPI(CurrentAPI);

            if (CurrentAPI == SelectAPI.TargetManager)
                return APISingleton.TM_API.ConnectTarget(target);
            else if (CurrentAPI == SelectAPI.ControlConsole)
            {
                var consoleList = GetCCAPIConsoleList();
                if (ConnectTarget(consoleList[target].Ip))
                {
                    TargetName = consoleList[target].Name;
                    return true;
                }
            }
            else
                OnError?.Invoke(ErrorCodes.NOT_SUPPORTED);

            return false;
        }

        /// <summary>Connect your console with CCAPI.</summary>
        public bool ConnectTarget(string ip)
        {
            if (CurrentAPI == SelectAPI.ControlConsole)
            {
                if (APISingleton.CC_API.SUCCESS(APISingleton.CC_API.ConnectTarget(ip)))
                {
                    TargetIp = ip;
                    return true;
                }
            }
            else if (CurrentAPI == SelectAPI.ManagerAPI)
            {
                if (APISingleton.M_API.ConnectTarget(ip))
                {
                    TargetIp = ip;
                    return true;
                }
            }

            return false;
        }

        /// <summary>Disconnect Target with selected API.</summary>
        public void DisconnectTarget()
        {
            if (CurrentAPI == SelectAPI.TargetManager)
                APISingleton.TM_API.DisconnectTarget();
            else if (CurrentAPI == SelectAPI.ControlConsole)
                APISingleton.CC_API.DisconnectTarget();
            else
                APISingleton.M_API.DisconnectTarget();
        }

        /// <summary>Attach the current process (current Game) with selected API.</summary>
        public bool AttachProcess()
        {
            if (CurrentAPI == SelectAPI.TargetManager)
                return IsAttached = APISingleton.TM_API.AttachProcess();
            else if (CurrentAPI == SelectAPI.ControlConsole)
                return IsAttached = APISingleton.CC_API.SUCCESS(APISingleton.CC_API.AttachProcess());
            else
                return IsAttached = APISingleton.M_API.AttachProcess();
        }

        public string GetConsoleName()
        {
            if (CurrentAPI == SelectAPI.TargetManager)
                return APISingleton.TM_API.SCE.GetTargetName();
            else if (CurrentAPI == SelectAPI.ControlConsole)
            {
                if (TargetName != string.Empty)
                    return TargetName;

                if (TargetIp != string.Empty)
                {
                    List<CCAPI.ConsoleInfo> Data = APISingleton.CC_API.GetConsoleList();
                    if (Data.Count > 0)
                    {
                        for (int i = 0; i < Data.Count; i++)
                            if (Data[i].Ip == TargetIp)
                                return Data[i].Name;
                    }
                }
                return TargetIp;
            }

            return null;
        }

        /// <summary>Set memory to offset with selected API.</summary>
        public void SetMemory(uint offset, byte[] buffer)
        {
            if (CurrentAPI == SelectAPI.TargetManager)
                APISingleton.TM_API.SetMemory(offset, buffer);
            else if (CurrentAPI == SelectAPI.ControlConsole)
                APISingleton.CC_API.SetMemory(offset, buffer);
            else
                APISingleton.M_API.Process.Memory.Set(APISingleton.M_API.Process.Process_Pid, offset, buffer);
        }

        /// <summary>Get memory from offset using the Selected API.</summary>
        public void GetMemory(uint offset, byte[] buffer)
        {
            if (CurrentAPI == SelectAPI.TargetManager)
                APISingleton.TM_API.GetMemory(offset, buffer);
            else if (CurrentAPI == SelectAPI.ControlConsole)
                APISingleton.CC_API.GetMemory(offset, buffer);
            else
                APISingleton.M_API.Process.Memory.Get(APISingleton.M_API.Process.Process_Pid, offset, buffer);
        }

        /// <summary>Get memory from offset with a length using the Selected API.</summary>
        public byte[] GetBytes(uint offset, int length)
        {
            byte[] buffer = new byte[length];
            if (CurrentAPI == SelectAPI.TargetManager)
                APISingleton.TM_API.GetMemory(offset, buffer);
            else if (CurrentAPI == SelectAPI.ControlConsole)
                APISingleton.CC_API.GetMemory(offset, buffer);
            else
                APISingleton.M_API.Process.Memory.Get(APISingleton.M_API.Process.Process_Pid, offset, buffer);
            return buffer;
        }

        /// <summary>Return selected API into string format.</summary>
        public string GetCurrentAPIName()
        {
            if (CurrentAPI == SelectAPI.TargetManager)
                return Enum.GetName(typeof(SelectAPI), SelectAPI.TargetManager).Replace("Manager", " Manager");
            else if (CurrentAPI == SelectAPI.ControlConsole)
                return Enum.GetName(typeof(SelectAPI), SelectAPI.ControlConsole).Replace("Console", " Console");
            else
                return Enum.GetName(typeof(SelectAPI), SelectAPI.ManagerAPI).Replace("API", " API");
        }

        /// <summary>Use the extension class with your selected API.</summary>
        public Extension Extension
        {
            get
            {
                return new Extension(CurrentAPI);
            }
        }

        public CCAPI CCAPI
        {
            get
            {
                return APISingleton.CC_API;
            }
        }

        public TMAPI TMAPI
        {
            get
            {
                return APISingleton.TM_API;
            }
        }

        public MAPI MAPI
        {
            get
            {
                return APISingleton.M_API;
            }
        }
    }
}