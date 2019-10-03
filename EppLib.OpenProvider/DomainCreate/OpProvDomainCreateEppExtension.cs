using System.Xml;

namespace EppLib.OpenProvider
{
    public class OpProvDomainCreateEppExtension : OpProvEppExtension
    {

        public string PromoCode { get; set; }
        public bool? UseDomicile { get; set; }

        /// <summary>
        /// Accepted values: on, off, default
        /// </summary>
        public string AutoRenew { get; set; }

        public string NsGroup { get; set; }

        /// <summary>
        /// Can be used if <op:nsGroup> is dns-openprovider
        /// </summary>
        public string NsTemplateName { get; set; }

        public string Comments { get; set; }

        public override XmlNode ToXml(XmlDocument doc)
        {
            var root = CreateElement(doc, "op:ext");
            var create = AddXmlElement(doc, root, "op:create", "");
            var domain = AddXmlElement(doc, create, "op:domain", "");

            if(PromoCode != null)
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
