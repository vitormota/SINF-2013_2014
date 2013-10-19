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
	public class LoginController : ApiController
	{
		//
		// GET: /Login/
		public int Get(string username, string password)
		{
			if (FirstREST.Lib_Primavera.PriEngine.InitializeCompany("BELAFLOR", username, password))
			{
				return 1;
			}
			else
			{
				return 0;
			}
		}

	}
}
