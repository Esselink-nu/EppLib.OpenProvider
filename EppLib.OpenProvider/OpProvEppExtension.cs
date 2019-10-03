using EppLib.Entities;

namespace EppLib.OpenProvider
{
    public abstract class OpProvEppExtension : EppExtension
    {
        protected override string Namespace { get; set; } = "http://www.openprovider.nl/epp/xml/opprov-1.0";
    }
}
