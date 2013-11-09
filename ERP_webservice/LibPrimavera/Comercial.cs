using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Interop.ErpBS800;
using Interop.StdPlatBS800;
using Interop.StdBE800;
using Interop.GcpBE800;
using ADODB;
using Interop.IGcpBS800;
namespace FirstREST.Lib_Primavera
{
	public class Comercial
	{

		public static List<Model.Order> OrdersList()
		{
			ErpBS objMotor = new ErpBS();
			//MotorPrimavera mp = new MotorPrimavera();
			StdBELista objList;
			Model.Order ord = new Model.Order();
			List<Model.Order> listOrders = new List<Model.Order>();
			if (PriEngine.Platform.Inicializada)
			{

				//if (PriEngine.InitializeCompany("BELAFLOR", "admin", "admin") == true){
				String query = "SELECT * FROM PRIBELAFLOR.dbo.CabecDoc";
				objList = PriEngine.Engine.Consulta(query);
				while (!objList.NoFim())
				{
					ord = new Model.Order();
					ord.CodClient = objList.Valor("Entidade");
					ord.modPag = objList.Valor("ModoPag");
					ord.numContrib = objList.Valor("NumContribuinte");
					ord.totalMerc = objList.Valor("TotalMerc");
					ord.totalIva = objList.Valor("TotalIva");
					ord.moeda = objList.Valor("Moeda");
					ord.date = objList.Valor("Data");
					ord.condPag = getCondPagamentoById(objList.Valor("CondPag"));
					ord.modExpedicao = getModExpedicaoById(objList.Valor("ModoExp"));
					listOrders.Add(ord);
					objList.Seguinte();
				}
				return listOrders;
			}
			else
				return null;
		}

		private static string getCondPagamentoById(string id) {
			//workaround
			if (id.Length == 1) id = "0" + id;
			//----------
			string query = "select descricao from PRIBELAFLOR.dbo.condPag where condpag=" + id;
			StdBELista objList = PriEngine.Engine.Consulta(query);
			return objList.Valor("Descricao");
		}

		private static string getModExpedicaoById(string id) {
			if (id == "") return "Nao definido.";
			string query = "select descricao from PRIBELAFLOR.dbo.ModosExp where ModoExp='"+id+"'";
			StdBELista objList = PriEngine.Engine.Consulta(query);
			return objList.Valor("Descricao");
		}

		public static List<Model.Order> OrdersList(string cliente)
		{
			ErpBS objMotor = new ErpBS();
			//MotorPrimavera mp = new MotorPrimavera();
			StdBELista objList;
			Model.Order ord = new Model.Order();
			List<Model.Order> listOrders = new List<Model.Order>();
			if (PriEngine.Platform.Inicializada)
			{

				//if (PriEngine.InitializeCompany("BELAFLOR", "admin", "admin") == true){
				String query = "SELECT * FROM PRIBELAFLOR.dbo.CabecDoc where entidade='"+cliente+"'";
				objList = PriEngine.Engine.Consulta(query);
				while (!objList.NoFim())
				{
					ord = new Model.Order();
					ord.CodClient = objList.Valor("Entidade");
					ord.modPag = objList.Valor("ModoPag");
					ord.numContrib = objList.Valor("NumContribuinte");
					ord.totalMerc = objList.Valor("TotalMerc");
					ord.totalIva = objList.Valor("TotalIva");
					ord.moeda = objList.Valor("Moeda");
					ord.date = objList.Valor("Data");
					ord.condPag = getCondPagamentoById(objList.Valor("CondPag"));
					ord.modExpedicao = getModExpedicaoById(objList.Valor("ModoExp"));
					listOrders.Add(ord);
					objList.Seguinte();
				}
				return listOrders;
			}
			else
				return null;
		}

		/**
		 * Get orders from client <cliente> between dates from & to (yyyy-mm-dd)
		 * to make date unspecified pass value "nd" for any of the dates
		 * 
		 * */
		public static List<Model.Order> OrdersList(string cliente, string from, string to)
		{
			string query = "SELECT * FROM PRIBELAFLOR.dbo.CabecDoc where entidade='" + cliente + "'";
			if(from != "nd") query += " and Data>='"+from+"'";
			if(to != "nd") query += " and Data<='"+to+"'";

			ErpBS objMotor = new ErpBS();
			StdBELista objList;
			Model.Order ord = new Model.Order();
			List<Model.Order> listOrders = new List<Model.Order>();
			if (PriEngine.Platform.Inicializada)
			{
				objList = PriEngine.Engine.Consulta(query);
				while (!objList.NoFim())
				{
					ord = new Model.Order();
					ord.CodClient = objList.Valor("Entidade");
					ord.modPag = objList.Valor("ModoPag");
					ord.numContrib = objList.Valor("NumContribuinte");
					ord.totalMerc = objList.Valor("TotalMerc");
					ord.totalIva = objList.Valor("TotalIva");
					ord.moeda = objList.Valor("Moeda");
					ord.date = objList.Valor("Data");
					ord.condPag = getCondPagamentoById(objList.Valor("CondPag"));
					ord.modExpedicao = getModExpedicaoById(objList.Valor("ModoExp"));
					listOrders.Add(ord);
					objList.Seguinte();
				}
				return listOrders;
			}
			else
				return null;
		}

		# region Cliente
		public static List<Model.Cliente> ListaClientes()
		{
			ErpBS objMotor = new ErpBS();
			//MotorPrimavera mp = new MotorPrimavera();
			StdBELista objList;
			Model.Cliente cli = new Model.Cliente();
			List<Model.Cliente> listClientes = new List<Model.Cliente>();
			if(PriEngine.Platform.Inicializada){

			//if (PriEngine.InitializeCompany("BELAFLOR", "admin", "admin") == true){
				//objList = PriEngine.Engine.Comercial.Clientes.LstClientes();
				String query = "SELECT * FROM PRIBELAFLOR.dbo.Clientes";
				objList = PriEngine.Engine.Consulta(query);
				while (!objList.NoFim())
				{
					cli = new Model.Cliente();
					cli.CodCliente = objList.Valor("Cliente");
					cli.NomeCliente = objList.Valor("Nome");
					cli.Moeda = objList.Valor("Moeda");
					//cli.NumContribuinte = objList.Valor("NumContribuinte");
					listClientes.Add(cli);
					objList.Seguinte();
				}
				return listClientes;
			}
			else
				return null;
		}
		public static Lib_Primavera.Model.Cliente GetCliente(string codCliente)
		{
			ErpBS objMotor = new ErpBS();
			GcpBECliente objCli = new GcpBECliente();
			Model.Cliente myCli = new Model.Cliente();
			if (PriEngine.InitializeCompany("BELAFLOR", "", "") == true)
			{
				if (PriEngine.Engine.Comercial.Clientes.Existe(codCliente) == true)
				{
					objCli = PriEngine.Engine.Comercial.Clientes.Edita(codCliente);
					myCli.CodCliente = objCli.get_Cliente();
					myCli.NomeCliente = objCli.get_Nome();
					myCli.Moeda = objCli.get_Moeda();
					myCli.NumContribuinte = objCli.get_NumContribuinte();
					return myCli;
				}
				else
				{
					return null;
				}
			}
			else
				return null;
		}
		public static Lib_Primavera.Model.RespostaErro UpdCliente(Lib_Primavera.Model.Cliente cliente)
		{
			Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();
			ErpBS objMotor = new ErpBS();
			GcpBECliente objCli = new GcpBECliente();
			try
			{
				if (PriEngine.InitializeCompany("BELAFLOR", "", "") == true)
				{
					if (PriEngine.Engine.Comercial.Clientes.Existe(cliente.CodCliente) == false)
					{
						erro.Erro = 1;
						erro.Descricao = "O cliente não existe";
						return erro;
					}
					else
					{
						objCli = PriEngine.Engine.Comercial.Clientes.Edita(cliente.CodCliente);
						objCli.set_EmModoEdicao(true);
						objCli.set_Nome(cliente.NomeCliente);
						objCli.set_NumContribuinte(cliente.NumContribuinte);
						objCli.set_Moeda(cliente.Moeda);
						PriEngine.Engine.Comercial.Clientes.Actualiza(objCli);
						erro.Erro = 0;
						erro.Descricao = "Sucesso";
						return erro;
					}
				}
				else
				{
					erro.Erro = 1;
					erro.Descricao = "Erro ao abrir a empresa";
					return erro;
				}
			}
			catch (Exception ex)
			{
				erro.Erro = 1;
				erro.Descricao = ex.Message;
				return erro;
			}
		}
		public static Lib_Primavera.Model.RespostaErro DelCliente(string codCliente)
		{
			Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();
			GcpBECliente objCli = new GcpBECliente();
			try
			{
				if (PriEngine.InitializeCompany("BELAFLOR", "", "") == true)
				{
					if (PriEngine.Engine.Comercial.Clientes.Existe(codCliente) == false)
					{
						erro.Erro = 1;
						erro.Descricao = "O cliente não existe";
						return erro;
					}
					else
					{
						PriEngine.Engine.Comercial.Clientes.Remove(codCliente);
						erro.Erro = 0;
						erro.Descricao = "Sucesso";
						return erro;
					}
				}
				else
				{
					erro.Erro = 1;
					erro.Descricao = "Erro ao abrir a empresa";
					return erro;
				}
			}
			catch (Exception ex)
			{
				erro.Erro = 1;
				erro.Descricao = ex.Message;
				return erro;
			}
		}
		public static Lib_Primavera.Model.RespostaErro InsereClienteObj(Model.Cliente cli)
		{
			Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();
			//ErpBS objMotor = new ErpBS();
			//MotorPrimavera mp = new MotorPrimavera();
			GcpBECliente myCli = new GcpBECliente();
			try
			{
				if (PriEngine.InitializeCompany("BELAFLOR", "", "") == true)
				{
					myCli.set_Cliente(cli.CodCliente);
					myCli.set_Nome(cli.NomeCliente);
					myCli.set_NumContribuinte(cli.NumContribuinte);
					myCli.set_Moeda(cli.Moeda);
					PriEngine.Engine.Comercial.Clientes.Actualiza(myCli);
					erro.Erro = 0;
					erro.Descricao = "Sucesso";
					return erro;
				}
				else
				{
					erro.Erro = 1;
					erro.Descricao = "Erro ao abrir empresa";
					return erro;
				}
			}
			catch (Exception ex)
			{
				erro.Erro = 1;
				erro.Descricao = ex.Message;
				return erro;
			}
		}
		/*
		public static void InsereCliente(string codCliente, string nomeCliente, string numContribuinte, string 
		moeda)
		{
		ErpBS objMotor = new ErpBS();
		MotorPrimavera mp = new MotorPrimavera();
		GcpBECliente myCli = new GcpBECliente();
		objMotor = mp.AbreEmpresa("DEMO", "", "", "Default");
		myCli.set_Cliente(codCliente);
		myCli.set_Nome(nomeCliente);
		myCli.set_NumContribuinte(numContribuinte);
		myCli.set_Moeda(moeda);
		objMotor.Comercial.Clientes.Actualiza(myCli);
		}
		*/
		#endregion Cliente;   // ­­­­­­­­­­­­­­­­­­­­­­­­­­­­­  END   CLIENTE    ­­­­­­­­­­­­­­­­­­­­­­­
		public static Lib_Primavera.Model.Artigo GetArtigo(string codArtigo)
		{
			// ErpBS objMotor = new ErpBS();
			GcpBEArtigo objArtigo = new GcpBEArtigo();
			Model.Artigo myArt = new Model.Artigo();
			if (PriEngine.InitializeCompany("BELAFLOR", "", "") == true)
			{
				if (PriEngine.Engine.Comercial.Artigos.Existe(codArtigo) == false)
				{
					return null;
				}
				else
				{
					objArtigo = PriEngine.Engine.Comercial.Artigos.Edita(codArtigo);
					myArt.CodArtigo = objArtigo.get_Artigo();
					myArt.DescArtigo = objArtigo.get_Descricao();
					return myArt;
				}
			}
			else
			{
				return null;
			}
		}
		public static List<Model.Artigo> ListaArtigos()
		{
			ErpBS objMotor = new ErpBS();
			//MotorPrimavera mp = new MotorPrimavera();
			StdBELista objList;
			Model.Artigo art = new Model.Artigo();
			List<Model.Artigo> listArts = new List<Model.Artigo>();
			if (PriEngine.InitializeCompany("BELAFLOR", "admin", "admin") == true)
			{
				objList = PriEngine.Engine.Comercial.Artigos.LstArtigos();
				while (!objList.NoFim())
				{
					art = new Model.Artigo();
					art.CodArtigo = objList.Valor("artigo");
					art.DescArtigo = objList.Valor("descricao");
					listArts.Add(art);
					objList.Seguinte();
				}
				return listArts;
			}
			else
			{
				return null;
			}
		}
	}
}