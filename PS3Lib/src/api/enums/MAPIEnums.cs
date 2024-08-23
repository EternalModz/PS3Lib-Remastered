namespace PS3Lib
{
    public enum Syscall8Mode
    {
        Enabled = 0,
        Only_CobraMambaAndPS3MAPI_Enabled = 1,
        Only_PS3MAPI_Enabled = 2,
        FakeDisabled = 3,
        Disabled = 4
    }

    internal enum PS3MAPI_ResponseCode
    {
        DataConnectionAlreadyOpen = 125,
        MemoryStatusOK = 150,
        CommandOK = 200,
        RequestSuccessful = 226,
        EnteringPassiveMode = 227,
        PS3MAPIConnected = 220,
        PS3MAPIConnectedOK = 230,
        MemoryActionCompleted = 250,
        MemoryActionPended = 350
    }

    public enum PowerFlags
    {
        ShutDown,
        QuickReboot,
        SoftReboot,
        HardReboot
    }
}