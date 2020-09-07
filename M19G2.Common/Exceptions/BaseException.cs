using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M19G2.Common.Exceptions
{
    public class BaseException : Exception
    {
        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public BaseException(int _errorCode, string _errorMessage, Exception e) : base(_errorMessage, e)
        {
            ErrorCode = _errorCode;
            ErrorMessage = _errorMessage;
        }
    }
}
