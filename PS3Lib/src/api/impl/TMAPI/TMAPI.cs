using PS3Lib.NET;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PS3Lib
{
    public class TMAPI
    {
        public static int Target = 0xFF;
        public static bool AssemblyLoaded = true;
        public static ResetParameter resetParameter;

        public TMAPI()
        {
        }

        public Extension Extension
        {
            get
            {
                return new Extension(SelectAPI.TargetManager);
            }
        }

        public class SCECMD
        {
            /// <summary>Get the target status and return the string value.</summary>
            public string SNRESULT()
            {
                return Parameters.snresult;
            }

            /// <summary>Get the target name.</summary>
            public string GetTargetName()
            {
                if (Parameters.ConsoleName == null || Parameters.ConsoleName == string.Empty)
                {
                    PS3TMAPI.InitTargetComms();
                    PS3TMAPI.TargetInfo TargetInfo = new PS3TMAPI.TargetInfo();
                    TargetInfo.Flags = TargetInfoFlag.TargetID;
                    TargetInfo.Target = Target;
                    PS3TMAPI.GetTargetInfo(ref TargetInfo);
                    Parameters.ConsoleName = TargetInfo.Name;
                }
                return Parameters.ConsoleName;
            }

            /// <summary>Get the target status and return the string value.</summary>
            public ConnectStatus GetStatus()
            {
                if (AssemblyLoaded)
                    return ConnectStatus.NotConnected;
                Parameters.connectStatus = new ConnectStatus();
                PS3TMAPI.GetConnectStatus(Target, out Parameters.connectStatus, out Parameters.usage);
                Parameters.Status = Parameters.connectStatus.ToString();
                return Parameters.connectStatus;
            }

            /// <summary>Get the ProcessID by the current process.</summary>
            public uint ProcessID()
            {
                return Parameters.ProcessID;
            }

            /// <summary>Get an array of processID's.</summary>
            public uint[] ProcessIDs()
            {
                return Parameters.processIDs;
            }

            /// <summary>Get some details from your target.</summary>
            public ConnectStatus DetailStatus()
            {
                return Parameters.connectStatus;
            }
        }

        public SCECMD SCE
        {
            get
            {
                return new SCECMD();
            }
        }

        public class Parameters
        {
            public static string
                usage,
                info,
                snresult,
                Status,
                MemStatus,
                ConsoleName;
            public static uint
                ProcessID;
            public static uint[]
                processIDs;
            public static byte[]
                Retour;
            public static ConnectStatus
                connectStatus;
        }

        public void InitComms()
        {
            PS3TMAPI.InitTargetComms();
        }

        /// <summary>Connect the default target and initialize the dll. Possible to put an int as arugment for determine which target to connect.</summary>
        public bool ConnectTarget(int TargetIndex = 0)
        {
            if (AssemblyLoaded)
                PS3TMAPI_NET();
            AssemblyLoaded = false;
            Target = TargetIndex;
            bool result = PS3TMAPI.SUCCEEDED(PS3TMAPI.InitTargetComms());
            result = PS3TMAPI.SUCCEEDED(PS3TMAPI.Connect(TargetIndex, null));
            return result;
        }

        /// <summary>Connect the target by is name.</summary>
        public bool ConnectTarget(string TargetName)
        {
            if (AssemblyLoaded)
                PS3TMAPI_NET();
            AssemblyLoaded = false;
            bool result = PS3TMAPI.SUCCEEDED(PS3TMAPI.InitTargetComms());
            if (result)
            {
                result = PS3TMAPI.SUCCEEDED(PS3TMAPI.GetTargetFromName(TargetName, out Target));
                result = PS3TMAPI.SUCCEEDED(PS3TMAPI.Connect(Target, null));
            }
            return result;
        }

        /// <summary>Disconnect the target.</summary>
        public void DisconnectTarget()
        {
            PS3TMAPI.Disconnect(Target);
        }

        /// <summary>Power on selected target.</summary>
        public void PowerOn(int numTarget = 0)
        {
            if (Target != 0xFF)
                numTarget = Target;
            PS3TMAPI.PowerOn(numTarget);
        }

        /// <summary>Power off selected target.</summary>
        public void PowerOff(bool Force)
        {
            PS3TMAPI.PowerOff(Target, Force);
        }

        /// <summary>Attach and continue the current process from the target.</summary>
        public bool AttachProcess()
        {
            PS3TMAPI.GetProcessList(Target, out Parameters.processIDs);
            bool isOK;
            if (Parameters.processIDs.Length > 0)
                isOK = true;
            else isOK = false;
            if (isOK)
            {
                ulong uProcess = Parameters.processIDs[0];
                Parameters.ProcessID = Convert.ToUInt32(uProcess);
                PS3TMAPI.ProcessAttach(Target, UnitType.PPU, Parameters.ProcessID);
                PS3TMAPI.ProcessContinue(Target, Parameters.ProcessID);
                Parameters.info = "The Process 0x" + Parameters.ProcessID.ToString("X8") + " Has Been Attached !";
            }
            return isOK;
        }

        /// <summary>Set memory to the target (byte[]).</summary>
        public void SetMemory(uint Address, byte[] Bytes)
        {
            PS3TMAPI.ProcessSetMemory(Target, UnitType.PPU, Parameters.ProcessID, 0, Address, Bytes);
        }

        /// <summary>Set memory to the address (byte[]).</summary>
        public void SetMemory(uint Address, ulong value)
        {
            byte[] b = BitConverter.GetBytes(value);
            Array.Reverse(b);
            PS3TMAPI.ProcessSetMemory(Target, UnitType.PPU, Parameters.ProcessID, 0, Address, b);
        }

        /// <summary>Set memory with value as string hexadecimal to the address (string).</summary>
        public void SetMemory(uint Address, string hexadecimal, EndianType Type = EndianType.BigEndian)
        {
            byte[] Entry = StringToByteArray(hexadecimal);
            if (Type == EndianType.LittleEndian)
                Array.Reverse(Entry);
            PS3TMAPI.ProcessSetMemory(Target, UnitType.PPU, Parameters.ProcessID, 0, Address, Entry);
        }

        /// <summary>Get memory from the address.</summary>
        public void GetMemory(uint Address, byte[] Bytes)
        {
            PS3TMAPI.ProcessGetMemory(Target, UnitType.PPU, Parameters.ProcessID, 0, Address, ref Bytes);
        }

        /// <summary>Get a bytes array with the length input.</summary>
        public byte[] GetBytes(uint Address, uint lengthByte)
        {
            byte[] Longueur = new byte[lengthByte];
            PS3TMAPI.ProcessGetMemory(Target, UnitType.PPU, Parameters.ProcessID, 0, Address, ref Longueur);
            return Longueur;
        }

        /// <summary>Get a string with the length input.</summary>
        public string GetString(uint Address, uint lengthString)
        {
            byte[] Longueur = new byte[lengthString];
            PS3TMAPI.ProcessGetMemory(Target, UnitType.PPU, Parameters.ProcessID, 0, Address, ref Longueur);
            string StringResult = Hex2Ascii(ReplaceString(Longueur));
            return StringResult;
        }

        internal static string Hex2Ascii(string iMCSxString)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i <= (iMCSxString.Length - 2); i += 2)
            {
                builder.Append(Convert.ToString(Convert.ToChar(int.Parse(iMCSxString.Substring(i, 2), NumberStyles.HexNumber))));
            }
            return builder.ToString();
        }

        internal static byte[] StringToByteArray(string hex)
        {
            string replace = hex.Replace("0x", "");
            string Stringz = replace.Insert(replace.Length - 1, "0");

            try
            {
                return Enumerable.Range(0, replace.Length)
                 .Where(x => x % 2 == 0)
                 .Select(x => Convert.ToByte(replace.Length % 2 == 0 ? replace.Substring(x, 2) : Stringz.Substring(x, 2), 16))
                 .ToArray();
            }
            catch { throw new ArgumentException("Value not possible.", "Byte Array"); }
        }

        internal static string ReplaceString(byte[] bytes)
        {
            string PSNString = BitConverter.ToString(bytes);
            PSNString = PSNString.Replace("00", string.Empty);
            PSNString = PSNString.Replace("-", string.Empty);
            for (int i = 0; i < 10; i++)
                PSNString = PSNString.Replace("^" + i.ToString(), string.Empty);
            return PSNString;
        }

        /// <summary>Reset target to XMB , Sometimes the target restart quickly.</summary>
        public void ResetToXMB(ResetTarget flag)
        {
            if (flag == ResetTarget.Hard)
                resetParameter = ResetParameter.Hard;
            else if (flag == ResetTarget.Quick)
                resetParameter = ResetParameter.Quick;
            else if (flag == ResetTarget.ResetEx)
                resetParameter = ResetParameter.ResetEx;
            else if (flag == ResetTarget.Soft)
                resetParameter = ResetParameter.Soft;
            PS3TMAPI.Reset(Target, resetParameter);
        }

        internal static Assembly LoadApi;
        ///<summary>Load the PS3 API for use with your Application .NET.</summary>
        public Assembly PS3TMAPI_NET()
        {
            AppDomain.CurrentDomain.AssemblyResolve += (s, e) =>
            {
                var filename = new AssemblyName(e.Name).Name;
                var x = string.Format(@"C:\Program Files\SN Systems\PS3\bin\ps3tmapi_net.dll", filename);
                var x64 = string.Format(@"C:\Program Files (x64)\SN Systems\PS3\bin\ps3tmapi_net.dll", filename);
                var x86 = string.Format(@"C:\Program Files (x86)\SN Systems\PS3\bin\ps3tmapi_net.dll", filename);
                if (File.Exists(x))
                    LoadApi = Assembly.LoadFile(x);
                else
                {
                    if (File.Exists(x64))
                        LoadApi = Assembly.LoadFile(x64);

                    else
                    {
                        if (File.Exists(x86))
                            LoadApi = Assembly.LoadFile(x86);
                        else
                            PS3API.OnError?.Invoke(ErrorCodes.TMAPI_NOT_FOUND);
                    }
                }
                return LoadApi;
            };
            return LoadApi;
        }
    }
}