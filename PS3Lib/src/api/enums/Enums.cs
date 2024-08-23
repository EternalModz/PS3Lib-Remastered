namespace PS3Lib
{
    public enum ErrorCodes
    {
        CCAPI_LOAD_FAILED,
        CCAPI_NOT_FOUND,
        CCAPI_NOT_INSTALLED,

        MAPI_LOAD_FAILED,
        MAPI_NOT_FOUND,

        TMAPI_NOT_FOUND,
        TMAPI_NOT_SELECTED,

        CID_NULL,
        PSID_NULL,

        NOT_SUPPORTED,
    }

    public enum SelectAPI
    {
        ControlConsole,
        TargetManager,
        ManagerAPI,
    }
}