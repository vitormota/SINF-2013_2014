using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FirstREST.Lib_Primavera.Model;

namespace FirstREST.Controllers
{
	public class AdminsController : ApiController
	{
        public IEnumerable<Lib_Primavera.Model.Admin> Get()
        {
            return Lib_Primavera.Comercial.AdminsList();
        }
		// GET return admin for profile 
		public Utilizador Get(string id)
		{
			Lib_Primavera.Model.Utilizador adm = Lib_Primavera.Comercial.GetAdmin(id);
			if (adm == null)
			{
				throw new HttpResponseException(
				Request.CreateResponse(HttpStatusCode.NotFound));

			}
			else
			{
				return adm;
			}
		}
	}
}
