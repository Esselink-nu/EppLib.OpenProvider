namespace EppLib.OpenProvider
{
    public static class OpObjURIs
    {
        public static string Domain { get; set; } = "urn:ietf:params:xml:ns:domain-1.0";
        public static string Contact { get; set; } = "urn:ietf:params:xml:ns:contact-1.0";
    }

    public static class OpExtensions
    {
        public static string RGP { get; set; } = "urn:ietf:params:xml:ns:rgp-1.0";
        public static string OpProv { get; set; } = "http://www.openprovider.nl/epp/xml/opprov-1.0";
    }

    public static class OpOptions
    {
        public static string MVersion { get; set; } = "1.0";
        public static string MLang { get; set; } = "en";
    }
}
