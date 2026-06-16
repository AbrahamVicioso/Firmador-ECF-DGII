using Microsoft.AspNetCore.Mvc;
using System.Xml;
using Wolverine;
using Firmador_API.UsesCases.EncryptXml;

namespace Firmador_API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmpresaController : ControllerBase
    {
        private readonly IMessageBus _bus;

        public EmpresaController(IMessageBus bus)
        {
            this._bus = bus;
        }

        [HttpPost("{empresa}/[action]")]
        [Consumes("application/xml")]
        [Produces("application/xml")]
        public async Task<IActionResult> Sign(short empresa, [FromBody] XmlDocument document)
        {
            var commnad = new EncryptXmlCommand()
            {
                empresa = empresa,
                Document = document,
                schema = "CEDESTECH"
            };

            var result = await _bus.InvokeAsync<XmlDocument>(commnad);

            return Content(result.OuterXml, "application/xml");
        }
    }
}
