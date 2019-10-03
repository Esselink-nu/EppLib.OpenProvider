using System;
using System.Diagnostics;

namespace EppLib.OpenProvider.ImplTests
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var op = new OpIntegrationTests("username", "password", "epp.cte.openprovider.eu", 443);

                //op.Hello();
                //op.DomainCheck();
                //op.ReqAck();
                op.CreateDomain();

            }
            catch(Exception ex)
            {
                Debugger.Break();
            }
        }
    }

}
