using System;

namespace Utils
{
    public enum ErrCode
    {
        Ok,
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
        #region burn-in check
        /* burn-in check */
        SpecViolation,

        CouponColdWarn,
        CouponHotWarn,
        CouponColdError,
        CouponHotError,

        CurrentLowWarn,
        CurrentHighWarn,
        CurrentLowError,
        CurrentHighError,

        CouponDiffOverRange,
        CouponTempTooHighToWater,
        WaterTempTooHighToCoupon,
        CouponTempRiseTooFast,
        CouponTempUnstable,
        WaterTempUnstable,

        TooHighError,
        TooHighWarn,
        TooLowWarn,
        TooLowError,
        #endregion

        InvalidValue,
        UndefinedError,
    };
    public class ErrInfo
    {
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
