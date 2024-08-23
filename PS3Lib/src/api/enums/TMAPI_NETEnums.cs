using System;

namespace PS3Lib
{
    public enum ConnectStatus
    {
        Connected,
        Connecting,
        NotConnected,
        InUse,
        Unavailable
    }

    [Flags]
    public enum TargetInfoFlag : uint
    {
        Boot = 0x20,
        FileServingDir = 0x10,
        HomeDir = 8,
        Info = 4,
        Name = 2,
        TargetID = 1
    }

    [Flags]
    public enum BootParameter : ulong
    {
        BluRayEmuOff = 4L,
        BluRayEmuUSB = 0x20L,
        DebugMode = 0x10L,
        Default = 0L,
        DualNIC = 0x80L,
        HDDSpeedBluRayEmu = 8L,
        HostFSTarget = 0x40L,
        MemSizeConsole = 2L,
        ReleaseMode = 1L,
        SystemMode = 0x11L
    }
}