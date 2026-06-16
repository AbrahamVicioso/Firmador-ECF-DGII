using Firmador_API.Repositories;
using System.Threading.Tasks;
using System.Xml;

namespace Firmador_API.UsesCases.EncryptXml
{
    public class EncryptXmlHandler
    {
        public static async Task<XmlDocument> Handle(EncryptXmlCommand command, IEmpresaRepository empresaRepository)
        {
            var empresa = await empresaRepository.GetEmpresaAsync(command.schema,command.empresa);

            var result = Services.EncryptService.EncryptXmlDocument(new Services.EncryptationBody()
            {
                CertificateName = empresa.Rnc,
                CertificatePassword = empresa.ClaveFirma,
                Document = command.Document
            });

            return result;
        }
    }
}
