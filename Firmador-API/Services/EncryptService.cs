using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace Firmador_API.Services
{
    public class EncryptationBody {
        public XmlDocument Document { get; set; }
        public string CertificateName { get; set; }
        public string CertificatePassword { get; set; }
    }
    public class EncryptService
    {
        public static XmlDocument EncryptXmlDocument(EncryptationBody body)
        {
            try
            {
                string p12Path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "certificados", $"{body.CertificateName}.p12");

                var certificate = X509CertificateLoader.LoadPkcs12FromFile(p12Path,$"{body.CertificatePassword}");

                if (!certificate.HasPrivateKey)
                    throw new CryptographicException("El certificado no contiene una clave privada accesible.");

                using var privateKey = certificate.GetRSAPrivateKey();
                if (privateKey == null)
                    throw new CryptographicException("No se pudo obtener la clave privada del certificado.");

                var signedXml = new SignedXml(body.Document)
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
                body.Document.DocumentElement?.AppendChild(body.Document.ImportNode(xmlDigitalSignature, true));

                return body.Document;
            }
            catch
            {
                throw;
            }
        }
    }
}
