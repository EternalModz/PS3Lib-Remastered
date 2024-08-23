using System;
using System.Runtime.InteropServices;
using System.Text;

namespace PS3Lib.NET
{
    public class PS3TMAPI
    {
        private class ScopedGlobalHeapPtr
        {
            private IntPtr m_intPtr = IntPtr.Zero;

            public ScopedGlobalHeapPtr(IntPtr intPtr)
            {
                m_intPtr = intPtr;
            }

            ~ScopedGlobalHeapPtr()
            {
                if (m_intPtr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(m_intPtr);
                }
            }

            public IntPtr Get()
            {
                return m_intPtr;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public class TCPIPConnectProperties
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0xff)]
            public string IPAddress;
            public uint Port;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct TargetInfoPriv
        {
            public TargetInfoFlag Flags;
            public int Target;
            public IntPtr Name;
            public IntPtr Type;
            public IntPtr Info;
            public IntPtr HomeDir;
            public IntPtr FSDir;
            public BootParameter Boot;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct TargetInfo
        {
            public TargetInfoFlag Flags;
            public int Target;
            public string Name;
            public string Type;
            public string Info;
            public string HomeDir;
            public string FSDir;
            public BootParameter Boot;
        }

        private static bool Is32Bit()
        {
            return (IntPtr.Size == 4);
        }

        public static bool FAILED(SNRESULT res)
        {
            return !SUCCEEDED(res);
        }

        public static bool SUCCEEDED(SNRESULT res)
        {
            return (res >= SNRESULT.SN_S_OK);
        }

        private static IntPtr AllocUtf8FromString(string wcharString)
        {
            if (wcharString == null)
                return IntPtr.Zero;
            byte[] bytes = Encoding.UTF8.GetBytes(wcharString);
            IntPtr destination = Marshal.AllocHGlobal((int)(bytes.Length + 1));
            Marshal.Copy(bytes, 0, destination, bytes.Length);
            Marshal.WriteByte((IntPtr)(destination.ToInt64() + bytes.Length), 0);
            return destination;
        }

        public static string Utf8ToString(IntPtr utf8, uint maxLength)
        {
            int len = Native.MultiByteToWideChar(65001, 0, utf8, -1, null, 0);
            if (len == 0)
                throw new System.ComponentModel.Win32Exception();
            var buf = new StringBuilder(len);
            len = Native.MultiByteToWideChar(65001, 0, utf8, -1, buf, len);
            return buf.ToString();
        }

        private static IntPtr ReadDataFromUnmanagedIncPtr<T>(IntPtr unmanagedBuf, ref T storage)
        {
            storage = (T)Marshal.PtrToStructure(unmanagedBuf, typeof(T));
            return new IntPtr(unmanagedBuf.ToInt64() + Marshal.SizeOf((T)storage));
        }

        public static SNRESULT InitTargetComms()
        {
            return !Is32Bit() ? Native.InitTargetCommsX64()
                              : Native.InitTargetCommsX86();
        }

        public static SNRESULT Connect(int target, string application)
        {
            return !Is32Bit() ? Native.ConnectX64(target, application)
                              : Native.ConnectX86(target, application);
        }

        public static SNRESULT PowerOn(int target)
        {
            return !Is32Bit() ? Native.PowerOnX64(target)
                              : Native.PowerOnX86(target);
        }

        public static SNRESULT PowerOff(int target, bool bForce)
        {
            uint force = bForce ? (uint)1 : 0;
            return !Is32Bit() ? Native.PowerOffX64(target, force)
                              : Native.PowerOffX86(target, force);
        }

        public static SNRESULT GetProcessList(int target, out uint[] processIDs)
        {
            processIDs = null;
            uint count = 0;
            SNRESULT res = Is32Bit() ? Native.GetProcessListX86(target, ref count, IntPtr.Zero)
                                     : Native.GetProcessListX64(target, ref count, IntPtr.Zero);
            if (!FAILED(res))
            {
                ScopedGlobalHeapPtr ptr = new ScopedGlobalHeapPtr(Marshal.AllocHGlobal((int)(4 * count)));
                res = Is32Bit() ? Native.GetProcessListX86(target, ref count, ptr.Get())
                                : Native.GetProcessListX64(target, ref count, ptr.Get());
                if (FAILED(res))
                {
                    return res;
                }
                IntPtr unmanagedBuf = ptr.Get();
                processIDs = new uint[count];
                for (uint i = 0; i < count; i++)
                {
                    unmanagedBuf = ReadDataFromUnmanagedIncPtr<uint>(unmanagedBuf, ref processIDs[i]);
                }
            }
            return res;
        }

        public static SNRESULT ProcessAttach(int target, UnitType unit, uint processID)
        {
            return !Is32Bit() ? Native.ProcessAttachX64(target, (uint)unit, processID)
                              : Native.ProcessAttachX86(target, (uint)unit, processID);
        }

        public static SNRESULT ProcessContinue(int target, uint processID)
        {
            return !Is32Bit() ? Native.ProcessContinueX64(target, processID)
                              : Native.ProcessContinueX86(target, processID);
        }

        public static SNRESULT GetTargetInfo(ref TargetInfo targetInfo)
        {
            TargetInfoPriv targetInfoPriv = new TargetInfoPriv
            {
                Flags = targetInfo.Flags,
                Target = targetInfo.Target
            };
            SNRESULT res = Is32Bit() ? Native.GetTargetInfoX86(ref targetInfoPriv)
                                     : Native.GetTargetInfoX64(ref targetInfoPriv);
            if (!FAILED(res))
            {
                targetInfo.Flags = targetInfoPriv.Flags;
                targetInfo.Target = targetInfoPriv.Target;
                targetInfo.Name = Utf8ToString(targetInfoPriv.Name, uint.MaxValue);
                targetInfo.Type = Utf8ToString(targetInfoPriv.Type, uint.MaxValue);
                targetInfo.Info = Utf8ToString(targetInfoPriv.Info, uint.MaxValue);
                targetInfo.HomeDir = Utf8ToString(targetInfoPriv.HomeDir, uint.MaxValue);
                targetInfo.FSDir = Utf8ToString(targetInfoPriv.FSDir, uint.MaxValue);
                targetInfo.Boot = targetInfoPriv.Boot;
            }
            return res;
        }

        public static SNRESULT GetTargetFromName(string name, out int target)
        {
            ScopedGlobalHeapPtr ptr = new ScopedGlobalHeapPtr(AllocUtf8FromString(name));
            return !Is32Bit() ? Native.GetTargetFromNameX64(ptr.Get(), out target)
                              : Native.GetTargetFromNameX86(ptr.Get(), out target);
        }

        public static SNRESULT GetConnectionInfo(int target, out TCPIPConnectProperties connectProperties)
        {
            connectProperties = null;
            ScopedGlobalHeapPtr ptr = new ScopedGlobalHeapPtr(Marshal.AllocHGlobal(Marshal.SizeOf(typeof(TCPIPConnectProperties))));
            SNRESULT res = Is32Bit() ? Native.GetConnectionInfoX86(target, ptr.Get())
                                     : Native.GetConnectionInfoX64(target, ptr.Get());
            if (SUCCEEDED(res))
            {
                connectProperties = new TCPIPConnectProperties();
                Marshal.PtrToStructure(ptr.Get(), connectProperties);
            }
            return res;
        }

        public static SNRESULT GetConnectStatus(int target, out ConnectStatus status, out string usage)
        {
            IntPtr ptr;
            uint num;
            SNRESULT snresult = Is32Bit() ? Native.GetConnectStatusX86(target, out num, out ptr)
                                          : Native.GetConnectStatusX64(target, out num, out ptr);
            status = (ConnectStatus)num;
            usage = Utf8ToString(ptr, uint.MaxValue);
            return snresult;
        }

        public static SNRESULT Reset(int target, ResetParameter resetParameter)
        {
            return !Is32Bit() ? Native.ResetX64(target, (ulong)resetParameter)
                              : Native.ResetX86(target, (ulong)resetParameter);
        }

        public static SNRESULT ProcessGetMemory(int target, UnitType unit, uint processID, ulong threadID, ulong address, ref byte[] buffer)
        {
            return !Is32Bit() ? Native.ProcessGetMemoryX64(target, unit, processID, threadID, address, buffer.Length, buffer)
                              : Native.ProcessGetMemoryX86(target, unit, processID, threadID, address, buffer.Length, buffer);
        }

        public static SNRESULT ProcessSetMemory(int target, UnitType unit, uint processID, ulong threadID, ulong address, byte[] buffer)
        {
            return !Is32Bit() ? Native.ProcessSetMemoryX64(target, unit, processID, threadID, address, buffer.Length, buffer)
                              : Native.ProcessSetMemoryX86(target, unit, processID, threadID, address, buffer.Length, buffer);
        }

        public static SNRESULT Disconnect(int target)
        {
            return !Is32Bit() ? Native.DisconnectX64(target)
                              : Native.DisconnectX86(target);
        }
    }
}