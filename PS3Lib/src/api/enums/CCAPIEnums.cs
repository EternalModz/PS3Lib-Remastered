namespace PS3Lib
{
    public enum CCAPIFunctions
    {
        ConnectConsole,
        DisconnectConsole,
        GetConnectionStatus,
        GetConsoleInfo,
        GetDllVersion,
        GetFirmwareInfo,
        GetNumberOfConsoles,
        GetProcessList,
        GetMemory,
        GetProcessName,
        GetTemperature,
        VshNotify,
        RingBuzzer,
        SetBootConsoleIds,
        SetConsoleIds,
        SetConsoleLed,
        SetMemory,
        ShutDown
    }

    public enum IdType
    {
        IDPS,
        PSID
    }

    public enum NotifyIcon
    {
        INFO,
        CAUTION,
        FRIEND,
        SLIDER,
        WRONGWAY,
        DIALOG,
        DIALOGSHADOW,
        TEXT,
        POINTER,
        GRAB,
        HAND,
        PEN,
        FINGER,
        ARROW,
        ARROWRIGHT,
        PROGRESS,
        TROPHY1,
        TROPHY2,
        TROPHY3,
        TROPHY4
    }

    public enum ConsoleType
    {
        CEX = 1,
        DEX = 2,
        TOOL = 3
    }

    public enum ProcessType
    {
        VSH,
        SYS_AGENT,
        CURRENTGAME
    }

    public enum RebootFlags
    {
        ShutDown = 1,
        SoftReboot = 2,
        HardReboot = 3
    }

    public enum BuzzerMode
    {
        Single = 1,
        Double,
        Triple
    }

    public enum LedColor
    {
        Green = 1,
        Red = 2,
        Yellow = 3,
    }

    public enum LedMode
    {
        Off,
        On,
        Blink
    }
}