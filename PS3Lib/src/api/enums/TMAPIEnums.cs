using System;

namespace PS3Lib
{
    public enum SNRESULT
    {
        SN_E_BAD_ALIGN = -28,
        SN_E_BAD_MEMSPACE = -18,
        SN_E_BAD_PARAM = -21,
        SN_E_BAD_TARGET = -3,
        SN_E_BAD_UNIT = -11,
        SN_E_BUSY = -22,
        SN_E_CHECK_TARGET_CONFIGURATION = -33,
        SN_E_COMMAND_CANCELLED = -36,
        SN_E_COMMS_ERR = -5,
        SN_E_COMMS_EVENT_MISMATCHED_ERR = -39,
        SN_E_CONNECT_TO_GAMEPORT_FAILED = -35,
        SN_E_CONNECTED = -38,
        SN_E_DATA_TOO_LONG = -26,
        SN_E_DECI_ERROR = -23,
        SN_E_DEPRECATED = -27,
        SN_E_DLL_NOT_INITIALISED = -15,
        SN_E_ERROR = -2147483648,
        SN_E_EXISTING_CALLBACK = -24,
        SN_E_FILE_ERROR = -29,
        SN_E_HOST_NOT_FOUND = -8,
        SN_E_INSUFFICIENT_DATA = -25,
        SN_E_LICENSE_ERROR = -32,
        SN_E_LOAD_ELF_FAILED = -10,
        SN_E_LOAD_MODULE_FAILED = -31,
        SN_E_MODULE_NOT_FOUND = -34,
        SN_E_NO_SEL = -20,
        SN_E_NO_TARGETS = -19,
        SN_E_NOT_CONNECTED = -4,
        SN_E_NOT_IMPL = -1,
        SN_E_NOT_LISTED = -13,
        SN_E_NOT_SUPPORTED_IN_SDK_VERSION = -30,
        SN_E_OUT_OF_MEM = -12,
        SN_E_PROTOCOL_ALREADY_REGISTERED = -37,
        SN_E_TARGET_IN_USE = -9,
        SN_E_TARGET_RUNNING = -17,
        SN_E_TIMEOUT = -7,
        SN_E_TM_COMMS_ERR = -6,
        SN_E_TM_NOT_RUNNING = -2,
        SN_E_TM_VERSION = -14,
        SN_S_NO_ACTION = 6,
        SN_S_NO_MSG = 3,
        SN_S_OK = 0,
        SN_S_PENDING = 1,
        SN_S_REPLACED = 5,
        SN_S_TARGET_STILL_REGISTERED = 7,
        SN_S_TM_VERSION = 4
    }

    public enum UnitType
    {
        PPU,
        SPU,
        SPURAW
    }

    [Flags]
    public enum ResetParameter : ulong
    {
        Hard = 1L,
        Quick = 2L,
        ResetEx = 9223372036854775808L,
        Soft = 0L
    }

    /// <summary>Enum of flag reset.</summary>
    public enum ResetTarget
    {
        Hard,
        Quick,
        ResetEx,
        Soft
    }
}