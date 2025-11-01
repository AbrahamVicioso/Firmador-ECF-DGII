using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace Firmador_API.Services
{
    public class EncryptService
    {
        public static XmlDocument EncryptXmlDocument(XmlDocument document)
        {
            try
            {
                string p12Path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "certificado.p12");

                var certificate = X509CertificateLoader.LoadPkcs12FromFile(p12Path,"123456789");

                if (!certificate.HasPrivateKey)
                    throw new CryptographicException("El certificado no contiene una clave privada accesible.");

                using var privateKey = certificate.GetRSAPrivateKey();
                if (privateKey == null)
                    throw new CryptographicException("No se pudo obtener la clave privada del certificado.");

                var signedXml = new SignedXml(document)
                {
                    SigningKey = privateKey
                };

                var reference = new Reference { Uri = "" };
                reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
                reference.AddTransform(new XmlDsigC14NTransform());
                signedXml.AddReference(reference);

                var keyInfo = new KeyInfo();
                keyInfo.AddClause(new KeyInfoX509Data(certificate));
                signedXml.KeyInfo = keyInfo;

                signedXml.ComputeSignature();

                var xmlDigitalSignature = signedXml.GetXml();
                document.DocumentElement?.AppendChild(document.ImportNode(xmlDigitalSignature, true));

                return document;
            }
            catch
            {
                throw;
            }
        }
    }
}
