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
	public class UtilizadoresController : ApiController
	{
        public IEnumerable<Lib_Primavera.Model.Utilizador> Get(string id)
        {
            if (id == "clientes")
                return listaClientes();
            if (id == "vendedores")
                return listaVendedores();
            if (id == "admins")
                return listaAdmins();

            return null;
        }
        public Utilizador Get(string id, string userId)
        {
            if (id == "clientes")
                return oneCliente(userId);
            if (id == "vendedores")
                return oneVendedor(userId);
            if (id == "admins")
                return oneAdmin(userId);   
      
            return null;
        }

		// CLIENTES
		public IEnumerable<Lib_Primavera.Model.Utilizador> listaClientes()
		{
			return Lib_Primavera.Comercial.ListaClientes();
		}
		
		public Utilizador oneCliente(string id)
		{
			Lib_Primavera.Model.Utilizador user = Lib_Primavera.Comercial.GetCliente(id);
			if (user == null)
			{
				throw new HttpResponseException(
				Request.CreateResponse(HttpStatusCode.NotFound));

			}
			else
			{
				return user;
			}
		}
		

		// VENDEDORES
		public IEnumerable<Lib_Primavera.Model.Utilizador> listaVendedores()
		{
			return Lib_Primavera.Comercial.ListaVendedores();
		}
		
		public Utilizador oneVendedor(string id)
		{
			Lib_Primavera.Model.Utilizador seller = Lib_Primavera.Comercial.GetVendedor(id);
			if (seller == null)
			{
				throw new HttpResponseException(
				Request.CreateResponse(HttpStatusCode.NotFound));

			}
			else
			{
				return seller;
			}
		}
		
		// ADMINS
		public IEnumerable<Lib_Primavera.Model.Utilizador> listaAdmins()
		{
			return Lib_Primavera.Comercial.ListaAdmins();
		}
		
		public Utilizador oneAdmin(string id)
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
