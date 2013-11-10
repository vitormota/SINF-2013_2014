using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FirstREST.Lib_Primavera.Model;
using Interop.StdPlatBS800;

namespace FirstREST.Controllers
{
    public class VendedoresController : ApiController
    {
        //
        // GET: api/Vendedores/
		public IEnumerable<Lib_Primavera.Model.Vendedores> Get()
		{
			return Lib_Primavera.Comercial.VendedoresList();
		}

    }
}
