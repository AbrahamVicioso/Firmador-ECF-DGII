using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml;

namespace Firmador_API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EncryptController : ControllerBase
    {
        [HttpPost]
        [Consumes("application/xml")]
        [Produces("application/xml")]
        public IActionResult Encrypt([FromBody] XmlDocument document)
        {
            var result = Services.EncryptService.EncryptXmlDocument(document);
            return Content(result.OuterXml, "application/xml");
        }
    }
}
