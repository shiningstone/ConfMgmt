using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Utils
{
    public class CmdRecord
    {
        public string CmdId;
        public DateTime Time;
        public string Value;

        public bool IsNeedReSend(string value = null)
        {
            if (value != null)/* set */
            {
                if (Value == null || Time == new DateTime() ||
                    Value != value || DateTime.Now.Subtract(Time).TotalMinutes > 1)
                {
                    return true;
                }
            }
            else/*read*/
            {
                if (Value != "NaN" || Time == new DateTime() || DateTime.Now.Subtract(Time).TotalMinutes > 1)
                {
                    return true;
                }
            }

            return false;
        }
        public void Update(string value = null)
        {
            Value = value;
            Time = DateTime.Now;
        }
    }
    public class CmdController
    {
        public ErrInfo Add(CmdRecord r, Func<string, ErrInfo> setCmd)
        {
            return new ErrInfo(ErrCode.Ok);
        }
    }
}