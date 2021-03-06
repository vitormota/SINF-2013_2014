﻿using System;
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
		//
		// GET: /Clientes/
		public IEnumerable<Lib_Primavera.Model.Utilizador> Get()
		{
			return Lib_Primavera.Comercial.ListaUtilizadores();
		}
		// GET api/cliente/5 
		public Utilizador Get(string id)
		{
			Lib_Primavera.Model.Utilizador user = Lib_Primavera.Comercial.GetClienteUser(id);
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
		// GET api/cliente/5 
		//public Utilizador Get(string id,string id2)
		//{
		//	Lib_Primavera.Model.Utilizador cliente = Lib_Primavera.Comercial.GetClienteUser(id);
		//	if (cliente == null)
		//	{
		//		throw new HttpResponseException(
		//		Request.CreateResponse(HttpStatusCode.NotFound));

		//	}
		//	else
		//	{
		//		return cliente;
		//	}
		//}

		//public HttpResponseMessage Post(Lib_Primavera.Model.Utilizador cliente)
		//{
		//	Lib_Primavera.Model.RespostaErro erro = new Lib_Primavera.Model.RespostaErro();
		//	erro = Lib_Primavera.Comercial.InsereClienteObj(cliente);
		//	if (erro.Erro == 0)
		//	{
		//		var response = Request.CreateResponse(
		//		HttpStatusCode.Created, cliente);
		//		string uri = Url.Link("DefaultApi", new { CodCliente = cliente.Cod });
		//		response.Headers.Location = new Uri(uri);
		//		return response;
		//	}
		//	else
		//	{
		//		return Request.CreateResponse(HttpStatusCode.BadRequest);
		//	}
		//}

		public HttpResponseMessage Put(int id, Lib_Primavera.Model.Utilizador cliente)
		{
			Lib_Primavera.Model.RespostaErro erro = new Lib_Primavera.Model.RespostaErro();
			try
			{
				erro = Lib_Primavera.Comercial.UpdCliente(cliente);
				if (erro.Erro == 0)
				{
					return Request.CreateResponse(HttpStatusCode.OK, erro.Descricao);
				}
				else
				{
					return Request.CreateResponse(HttpStatusCode.NotFound, erro.Descricao);
				}
			}
			catch (Exception exc)
			{
				return Request.CreateResponse(HttpStatusCode.BadRequest, erro.Descricao);
			}
		}
		public HttpResponseMessage Delete(string id)
		{
			Lib_Primavera.Model.RespostaErro erro = new Lib_Primavera.Model.RespostaErro();
			try
			{
				erro = Lib_Primavera.Comercial.DelCliente(id);
				if (erro.Erro == 0)
				{
					return Request.CreateResponse(HttpStatusCode.OK, erro.Descricao);
				}
				else
				{
					return Request.CreateResponse(HttpStatusCode.NotFound, erro.Descricao);
				}
			}
			catch (Exception exc)
			{
				return Request.CreateResponse(HttpStatusCode.BadRequest, erro.Descricao);
			}
		}

		public IEnumerable<Lib_Primavera.Model.Utilizador> Get(string type, string id)
		{
			if(String.Compare(type,"seller")==0){
				return Lib_Primavera.Comercial.getClientsFromSeller(id);
			}
			else if (String.Compare(type, "client") == 0)
			{
				return Lib_Primavera.Comercial.getSellersFromClient(id);
			}
			else
			{
				return null;
			}	
		}
	}
}
