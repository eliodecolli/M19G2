using Castle.DynamicProxy;
using M19G2.Common.Exceptions;
using M19G2.Common.Logging;
using System;

namespace M19G2.Common
{
    public class Interceptor : IInterceptor
    {

        public void Intercept(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();
            }
            catch (Exception e)
            {
                CustomLogger.LogError(e.Message, e);
                throw new BaseException(666, e.Message, e);
            }
        }
    }
}
