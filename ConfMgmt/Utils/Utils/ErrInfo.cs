using System;
using System.Windows.Forms;

namespace Utils
{
    public enum ErrCode
    {
        Ok,
        Fail,

        UserCanceled,
        EventIgnored,
        ErrorParamValue,
        #region database query
        ErrorFormat,
        DataNotFound,
        RecipeNotMatch,
        UnknownParam,
        DatabaseFail,
        #endregion
        #region hardware access
        HardwareError,
        PowerSupplyLostCommunication,
        CylinderLostCommunication,
        TemperatureLostCommunication,
        #endregion
        #region check connection
        WaterTempOverRange,
        TemperatureOverRange,
        #endregion
        #region spec check
        SpecViolation,
        TooHighError,
        TooHighWarn,
        TooLowWarn,
        TooLowError,
        #endregion

        Abort,
        Blocked,
        InvalidValue,

        UndefinedError,
    };
    public class ErrInfo
    {
        public static ErrInfo Ok = new ErrInfo(ErrCode.Ok);

        public static string NullString = "";
        public static string FailString = "N/A";

        public ErrCode Code;
        public string Info;
        public ErrInfo(ErrCode code, object info = null)
        {
            Code = code;
            if (info != null)
            {
                Info = info as string;
            }
        }
        public ErrInfo(ErrCode code, object info, BIException biex)
        {
            Code = code;
            if (info != null)
            {
                Info = (info as string) + ": " + biex.mMsg;
            }
        }
        public ErrInfo(ErrCode code, string info, string exMessage)
        {
            Code = code;
            Info = (info as string) + ": " + exMessage;
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", Code, Info);
        }

        public ErrInfo ShowAnyAbnormal()
        {
            if (Code != ErrCode.Ok)
            {
                MessageBox.Show($"{Info}");
            }

            return this;
        }
    }
    public class BIException : Exception
    {
        public ErrCode mCode { get; set; }
        public string mMsg { get; set; }
        public BIException(ErrCode code, string msg = "Ok")
            : base(code.ToString())
        {
            mCode = code;
            mMsg = msg;
        }
        public BIException(ErrCode code, string msg, BIException ex)
        {
            mCode = code;
            mMsg = msg + ": " + ex.mMsg;
        }
    }
}
