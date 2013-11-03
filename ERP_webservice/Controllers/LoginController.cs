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
	public class LoginController : ApiController
	{
		//
		// GET: /Login/
		/**
		 * Upon login returns json string:
		 * 
		 * {type:0} - non valid login parameters
		 * {type:1} - admin logged in
		 * {type:2} - comercial I logged in
		 * {type:3} - cliente logged in
		 */
		public String Get(string username, string password)
		{
			if (FirstREST.Lib_Primavera.PriEngine.InitializeCompany("BELAFLOR", username, password))
			{
				StdBSAdministrador list = FirstREST.Lib_Primavera.PriEngine.Platform.Administrador;
				StdBSUtilizador user = FirstREST.Lib_Primavera.PriEngine.Platform.Contexto.Utilizador;
				String s; // = Microsoft.VisualBasic.Information.TypeName(user.get_objUtilizador());
				dynamic d = user.get_objUtilizador();
				s = d.PerfilSugerido();
				switch (s) { 
					case "Comercial I":
						//sales manager detected
						return "{type:2}";
					case "":
						//admin detected
						return "{type:1}";
					case "cliente":
						//client detected
						return "{type:3}";
					default:
						//no identifiable profile
						return "{type:0}";
				}
			}
			else
			{
				return "{type:0}";
			}
		}

	}
}
