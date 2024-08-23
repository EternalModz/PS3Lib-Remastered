using System;
using System.Runtime.InteropServices;
using System.Text;
using static PS3Lib.NET.PS3TMAPI;

namespace PS3Lib
{
    public static class Native
    {
        #region Win32
        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string dllName);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int MultiByteToWideChar(int codepage, int flags, IntPtr utf8, int utf8len, StringBuilder buffer, int buflen);
        #endregion

        #region TMAPI
        [DllImport("PS3TMAPIX64.dll", EntryPoint = "SNPS3InitTargetComms", CallingConvention = CallingConvention.Cdecl)]
        public static extern SNRESULT InitTargetCommsX64();

        [DllImport("PS3TMAPI.dll", EntryPoint = "SNPS3InitTargetComms", CallingConvention = CallingConvention.Cdecl)]
        public static extern SNRESULT InitTargetCommsX86();

        [DllImport("PS3TMAPIX64.dll", EntryPoint = "SNPS3PowerOn", CallingConvention = CallingConvention.Cdecl)]
        public static extern SNRESULT PowerOnX64(int target);

        [DllImport("PS3TMAPI.dll", EntryPoint = "SNPS3PowerOn", CallingConvention = CallingConvention.Cdecl)]
        public static extern SNRESULT PowerOnX86(int target);

        [DllImport("PS3TMAPIX64.dll", EntryPoint = "SNPS3PowerOff", CallingConvention = CallingConvention.Cdecl)]
        public static extern SNRESULT PowerOffX64(int target, uint force);

        [DllImport("PS3TMAPI.dll", EntryPoint = "SNPS3PowerOff", CallingConvention = CallingConvention.Cdecl)]
        public static extern SNRESULT PowerOffX86(int target, uint force);

        [DllImport("PS3TMAPIX64.dll", EntryPoint = "SNPS3Connect", CallingConvention = CallingConvention.Cdecl)]
        public static extern SNRESULT ConnectX64(int target, string application);

        [DllImport("PS3TMAPI.dll", EntryPoint = "SNPS3Connect", CallingConvention = CallingConvention.Cdecl)]
        public static extern SNRESULT ConnectX86(int target, string application);

        [DllImport("PS3TMAPIX64.dll", EntryPoint = "SNPS3GetConnectionInfo", CallingConvention = CallingConvention.Cdecl)]
        public static extern SNRESULT GetConnectionInfoX64(int target, IntPtr connectProperties);

        [DllImport("PS3TMAPI.dll", EntryPoint = "SNPS3GetConnectionInfo", CallingConvention = CallingConvention.Cdecl)]
        public static extern SNRESULT GetConnectionInfoX86(int target, IntPtr connectProperties);

        [DllImport("PS3TMAPIX64.dll", EntryPoint = "SNPS3GetConnectStatus", CallingConvention = CallingConvention.Cdecl)]
        public static extern SNRESULT GetConnectStatusX64(int target, out uint status, out IntPtr usage);

        [DllImport("PS3TMAPI.dll", EntryPoint = "SNPS3GetConnectStatus", CallingConvention = CallingConvention.Cdecl)]
        public static extern SNRESULT GetConnectStatusX86(int target, out uint status, out IntPtr usage);

        [DllImport("PS3TMAPIX64.dll", EntryPoint = "SNPS3ProcessList", CallingConvention = CallingConvention.Cdecl)]
        public static extern SNRESULT GetProcessListX64(int target, ref uint count, IntPtr processIdArray);

        [DllImport("PS3TMAPI.dll", EntryPoint = "SNPS3ProcessList", CallingConvention = CallingConvention.Cdecl)]
        public static extern SNRESULT GetProcessListX86(int target, ref uint count, IntPtr processIdArray);

        [DllImport("PS3TMAPIX64.dll", EntryPoint = "SNPS3ProcessContinue", CallingConvention = CallingConvention.Cdecl)]
        public static extern SNRESULT ProcessContinueX64(int target, uint processId);

        [DllImport("PS3TMAPI.dll", EntryPoint = "SNPS3ProcessContinue", CallingConvention = CallingConvention.Cdecl)]
        public static extern SNRESULT ProcessContinueX86(int target, uint processId);

        [DllImport("PS3TMAPIX64.dll", EntryPoint = "SNPS3ProcessAttach", CallingConvention = CallingConvention.Cdecl)]
        public static extern SNRESULT ProcessAttachX64(int target, uint unitId, uint processId);

        [DllImport("PS3TMAPI.dll", EntryPoint = "SNPS3ProcessAttach", CallingConvention = CallingConvention.Cdecl)]
        public static extern SNRESULT ProcessAttachX86(int target, uint unitId, uint processId);

        [DllImport("PS3TMAPIX64.dll", EntryPoint = "SNPS3ProcessGetMemory", CallingConvention = CallingConvention.Cdecl)]
        public static extern SNRESULT ProcessGetMemoryX64(int target, UnitType unit, uint processId, ulong threadId, ulong address, int count, byte[] buffer);

        [DllImport("PS3TMAPI.dll", EntryPoint = "SNPS3ProcessGetMemory", CallingConvention = CallingConvention.Cdecl)]
        public static extern SNRESULT ProcessGetMemoryX86(int target, UnitType unit, uint processId, ulong threadId, ulong address, int count, byte[] buffer);

        [DllImport("PS3TMAPIX64.dll", EntryPoint = "SNPS3GetTargetFromName", CallingConvention = CallingConvention.Cdecl)]
        public static extern SNRESULT GetTargetFromNameX64(IntPtr name, out int target);

        [DllImport("PS3TMAPI.dll", EntryPoint = "SNPS3GetTargetFromName", CallingConvention = CallingConvention.Cdecl)]
        public static extern SNRESULT GetTargetFromNameX86(IntPtr name, out int target);

        [DllImport("PS3TMAPIX64.dll", EntryPoint = "SNPS3Reset", CallingConvention = CallingConvention.Cdecl)]
        public static extern SNRESULT ResetX64(int target, ulong resetParameter);

        [DllImport("PS3TMAPI.dll", EntryPoint = "SNPS3Reset", CallingConvention = CallingConvention.Cdecl)]
        public static extern SNRESULT ResetX86(int target, ulong resetParameter);

        [DllImport("PS3TMAPIX64.dll", EntryPoint = "SNPS3ProcessSetMemory", CallingConvention = CallingConvention.Cdecl)]
        public static extern SNRESULT ProcessSetMemoryX64(int target, UnitType unit, uint processId, ulong threadId, ulong address, int count, byte[] buffer);

        [DllImport("PS3TMAPI.dll", EntryPoint = "SNPS3ProcessSetMemory", CallingConvention = CallingConvention.Cdecl)]
        public static extern SNRESULT ProcessSetMemoryX86(int target, UnitType unit, uint processId, ulong threadId, ulong address, int count, byte[] buffer);

        [DllImport("PS3TMAPIX64.dll", EntryPoint = "SNPS3GetTargetInfo", CallingConvention = CallingConvention.Cdecl)]
        public static extern SNRESULT GetTargetInfoX64(ref TargetInfoPriv targetInfoPriv);

        [DllImport("PS3TMAPI.dll", EntryPoint = "SNPS3GetTargetInfo", CallingConvention = CallingConvention.Cdecl)]
        public static extern SNRESULT GetTargetInfoX86(ref TargetInfoPriv targetInfoPriv);

        [DllImport("PS3TMAPIX64.dll", EntryPoint = "SNPS3Disconnect", CallingConvention = CallingConvention.Cdecl)]
        public static extern SNRESULT DisconnectX64(int target);

        [DllImport("PS3TMAPI.dll", EntryPoint = "SNPS3Disconnect", CallingConvention = CallingConvention.Cdecl)]
        public static extern SNRESULT DisconnectX86(int target);
        #endregion
    }
}