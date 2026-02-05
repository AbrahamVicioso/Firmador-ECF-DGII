using System.Xml;

namespace Firmador_API.UsesCases.EncryptXml
{
    public class EncryptXmlCommand
    {
        public XmlDocument Document { get; set; }
        public short empresa { get; set; }
    }
}
