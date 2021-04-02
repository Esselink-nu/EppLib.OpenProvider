using EppLib.Entities;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace EppLib.OpenProvider
{

    public class OpDomainTransfer : DomainTransfer
    {
        public OpDomainTransfer(string domainName)
            : base(domainName)
        { }

        public OpDomainTransfer(string domainName, string registrantContactId, string adminContactId, string techContactId)
            : base(domainName, registrantContactId, adminContactId, techContactId)
        { }


        public string PromoCode { get; set; }
        public bool? UseDomicile { get; set; }

        public string AutoRenew { get; set; }

        public IEnumerable<string> NameServers { get; }

        public string NsGroup { get; set; }

        public string NsTemplateName { get; set; }

        public string Comments { get; set; }


        //protected override XmlElement BuildCommandElement(XmlDocument doc, XmlElement commandRootElement)
        //{
        //    var domainCreate = BuildCommandElement(doc, "create", commandRootElement);

        //    AddXmlElement(doc, domainCreate, "domain:name", DomainName, namespaceUri);

        //    if (Period != null)
        //    {
        //        var period = AddXmlElement(doc, domainCreate, "domain:period", Period.Value.ToString(CultureInfo.InvariantCulture), namespaceUri);

        //        period.SetAttribute("unit", Period.Unit);
        //    }

        //    if (NameServers != null && NameServers.Count > 0)
        //    {
        //        domainCreate.AppendChild(CreateNameServerElement(doc, NameServers));
        //    }

        //    if (RegistrantContactId != null)
        //    {
        //        AddXmlElement(doc, domainCreate, "domain:registrant", RegistrantContactId, namespaceUri);
        //    }

        //    foreach (var contact in DomainContacts)
        //    {
        //        var contact_element = AddXmlElement(doc, domainCreate, "domain:contact", contact.Id, namespaceUri);

        //        contact_element.SetAttribute("type", contact.Type);
        //    }


        //    // We have to set the authInfo and PW elements even though they must be empty or the command will fail.
        //    var authInfo = AddXmlElement(doc, domainCreate, "domain:authInfo", string.Empty, namespaceUri);

        //    AddXmlElement(doc, authInfo, "domain:pw", string.Empty, namespaceUri);

        //    return domainCreate;
        //}

        public override XmlDocument ToXml()
        {
            var opCreate = new OpProvDomainTransferEppExtension
            {
                PromoCode = PromoCode,
                UseDomicile = UseDomicile,
                AutoRenew = AutoRenew,
                NsGroup = NsGroup,
                NsTemplateName = NsTemplateName,
                Comments = Comments,
            };

            Extensions.Clear();
            Extensions.Add(opCreate);

            return base.ToXml();
        }
    }
}
