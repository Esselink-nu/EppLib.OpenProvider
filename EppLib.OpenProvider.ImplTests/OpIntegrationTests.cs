using System.Collections.Generic;
using EppLib.Entities;
using System;
using System.Diagnostics;


namespace EppLib.OpenProvider.ImplTests
{
    class OpIntegrationTests
    {
        private string _userName;
        private string _password;
        private string _url;
        private int _port;
        private readonly TraceSource _traceSource = new TraceSource("EppLib");

        public Service Service { get; set; }

        public OpIntegrationTests(string username, string password, string url, int port)
        {
            this._userName = username;
            this._password = password;
            this._url = url;
            this._port = port;

            Service = CreateService();
        }

        public void Hello()
        {
            RunCmd(() =>
            {
                var helloCmd = new Hello();
                var helloResp = Service.Execute(helloCmd);

                // Hello does not have a response code
            });
        }


        /// <summary>
        /// Please note that OpenProvider always says the domain name is not available on their common test environment.
        /// </summary>
        public void DomainCheck()
        {
            RunCmd(() =>
            {
                var domainName1 = "dpc-sadfasdf1-qq1.nl";
                var domainName2 = "dpc-sadfasdf1-qq2.nl";
                var domainName3 = "dpc-sadfasdf1-qq3.nl";

                var domainCheck = new DomainCheck(new List<string> { domainName1, domainName2, domainName3 });
                var resp = Service.Execute(domainCheck);
                Ensure.Success(resp);

                // There should be a client transactionId (clTRID) but there is not (domain names not available at the very least).
                // Ensure.IsNotNullOrEmpty(nameof(resp.ClientTransactionId), resp.ClientTransactionId);
                Ensure.IsNotNullOrEmpty(nameof(resp.ServerTransactionId), resp.ServerTransactionId);
            });
        }


        public void CreateDomain()
        {
            RunCmd(() =>
            {
                var createCmd = new OpDomainCreate("dpc-codemonk3.nl", "ED904543-NL");
                createCmd.Period = new DomainPeriod(1, "y");
                createCmd.DomainContacts.Add(new DomainContact("ED904543-NL", "admin"));
                createCmd.DomainContacts.Add(new DomainContact("YN000117-NL", "tech"));
                createCmd.Comments = "Commens, yo!";
                createCmd.AutoRenew = "off";

                var resp = Service.Execute(createCmd);
                Ensure.Success(resp);
            });
        }



        public void ReqAck()
        {
            RunCmd(() =>
            {
                var pollCmd = new Poll();
                pollCmd.Type = PollType.Request;

                var pollResp = Service.Execute(pollCmd);
                Ensure.Success(pollResp);

                var doc = System.Xml.Linq.XDocument.Parse(pollCmd.ToXml().OuterXml);

                var id = pollResp.Id;

                var ackCmd = new Poll();
                ackCmd.Type = PollType.Acknowledge;
                ackCmd.MessageId = id;

                var ackResp = Service.Execute(ackCmd);

                Ensure.Success(ackResp);
            });
        }


        /// <summary>
        /// Run method that contain general command execution flow and verify response codes.
        /// Connect, Login, Execute, Logout and Disconnect.
        /// </summary>
        /// <param name="action"></param>
        protected void RunCmd(Action action)
        {
            Ensure.ArgumentNotNull(nameof(action), action);

            try
            {
                Service.Connect(System.Security.Authentication.SslProtocols.Tls12);

                var loginResp = Service.Execute(CreateLoginCommand());
                Ensure.Success(loginResp);

                action();
            }
            catch(Exception ex)
            {
                _traceSource.TraceEvent(TraceEventType.Error, 0, ex.Message + " " + ex.StackTrace);
                throw;
            }
            finally
            {
                Service?.Execute(CreateLogout());
                Service?.Disconnect();
            }
        }

        protected Service CreateService() => new Service(new WebConnection(_url, _port));

        protected Logout CreateLogout() => new Logout();

        protected OpLogin CreateLoginCommand() => new OpLogin(_userName, _password);


    }
}
