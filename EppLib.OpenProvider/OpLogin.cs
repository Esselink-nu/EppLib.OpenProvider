using EppLib.Entities;
using System.Collections.Generic;

namespace EppLib.OpenProvider
{

    public class OpLogin : Entities.Login
    {
        public OpLogin(string clientId, string password)
            : base(clientId, password)
        {
            Init();
        }

        public OpLogin(string clientId, string password, string newPassword)
            : base(clientId, password, newPassword)
        {
            Init();
        }

        protected void Init()
        {
            Options = new Options();
            Options.MLang = OpOptions.MLang;
            Options.MVersion = OpOptions.MVersion;
            
            Services = new Services();
            Services.ObjURIs.Add(OpObjURIs.Domain);
            Services.ObjURIs.Add(OpObjURIs.Contact);

            Services.Extensions = new List<string>();
            Services.Extensions.Add(OpExtensions.RGP);
            Services.Extensions.Add(OpExtensions.OpProv);
        }
    }
}
