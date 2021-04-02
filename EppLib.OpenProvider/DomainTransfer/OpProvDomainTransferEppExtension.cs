using EppLib.Entities;
using System.Collections.Generic;
using System.Xml;

namespace EppLib.OpenProvider
{
    public class OpProvDomainTransferEppExtension : OpProvEppExtension
    {
        public string RegistrantContactId { get; set; }

        public IList<DomainContact> DomainContacts { get; } = new List<DomainContact>();

        public IList<NameServer> NameServers { get; } = new List<NameServer>();

        /// <summary>
        /// Accepted values: on, off, default
        /// </summary>
        public string AutoRenew { get; set; }

        public string NsGroup { get; set; }

        public string PromoCode { get; set; }
        public bool? UseDomicile { get; set; }

        /// <summary>
        /// Can be used if <op:nsGroup> is dns-openprovider
        /// </summary>
        public string NsTemplateName { get; set; }

        public string Comments { get; set; }

        public override XmlNode ToXml(XmlDocument doc)
        {
            var root = CreateElement(doc, "op:ext");
            var transfer = AddXmlElement(doc, root, "op:transfer", "");
            var domain = AddXmlElement(doc, transfer, "op:domain", "");

            if (RegistrantContactId != null)
                AddXmlElement(doc, domain, "op:registrant", RegistrantContactId);

            foreach (var contact in DomainContacts)
            {
                var contact_element = AddXmlElement(doc, domain, "op:contact", contact.Id);

                contact_element.SetAttribute("type", contact.Type);
            }

            if(NameServers != null && NameServers.Count > 0)
            {
                var nameServerElement = AddXmlElement(doc, domain, "op:ns", "");

                foreach (var serverName in NameServers)
                {
                    var hostAttr = AddXmlElement(doc, nameServerElement, "domain:hostAttr", "", "urn:ietf:params:xml:ns:domain-1.0");

                    AddXmlElement(doc, hostAttr, "domain:hostName", serverName.HostName, "urn:ietf:params:xml:ns:domain-1.0");

                    foreach (var address in serverName.HostAddresses)
                    {
                        var hostAddr = AddXmlElement(doc, hostAttr, "domain:hostAddr", address.IPAddress, "urn:ietf:params:xml:ns:domain-1.0");

                        hostAddr.SetAttribute("ip", address.IPVersion);
                    }
                }
            }

            if (PromoCode != null)
                AddXmlElement(doc, domain, "op:promoCode", PromoCode);

            if (UseDomicile != null)
                AddXmlElement(doc, domain, "op:useDomicile", UseDomicile.ToString());

            // on, off or default
            if (AutoRenew != null)
                AddXmlElement(doc, domain, "op:autorenew", AutoRenew);

            if (NsGroup != null)
                AddXmlElement(doc, domain, "op:nsGroup", NsGroup);

            if (NsTemplateName != null)
                AddXmlElement(doc, domain, "op:nsTemplateName", NsTemplateName);

            if (Comments != null)
                AddXmlElement(doc, domain, "op:comments", Comments);

            return root;
        }
    }
}
