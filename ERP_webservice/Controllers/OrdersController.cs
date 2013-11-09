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
    public class OrdersController : ApiController
    {
        //
        // GET: api/orders/
		//
		public IEnumerable<Lib_Primavera.Model.Order> Get()
		{
			return Lib_Primavera.Comercial.OrdersList();
		}

		//
		// GET: api/orders?clienteId=username
		//
		public IEnumerable<Lib_Primavera.Model.Order> Get(string clienteId)
		{
			return Lib_Primavera.Comercial.OrdersList(clienteId);
		}

		//
		// GET: api/orders?clienteId=username&from=yyyy-mm-dd&to=yyyy-mm-dd
		// 
		// OR
		//
		// api/orders?clienteId=username&from=yyyy-mm-dd&to=nd
		//
		// nd - not defined
		//
		public IEnumerable<Lib_Primavera.Model.Order> Get(string clienteId, string from, string to)
		{
			return Lib_Primavera.Comercial.OrdersList(clienteId,from,to);
		}

    }
}
