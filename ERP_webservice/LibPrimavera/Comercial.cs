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
				String query = "SELECT * FROM PRIBELAFLOR.dbo.CabecDoc WHERE TipoDoc='ECL'";
				objList = PriEngine.Engine.Consulta(query);
				while (!objList.NoFim())
				{
					ord = new Model.Order();
					ord.id = objList.Valor("Id");
					ord.docNum = objList.Valor("NumDoc");
					ord.CodClient = objList.Valor("Entidade");
					ord.modPag = objList.Valor("ModoPag");
					ord.numContrib = objList.Valor("NumContribuinte");
					ord.totalMerc = objList.Valor("TotalMerc");
					ord.totalIva = objList.Valor("TotalIva");
					ord.moeda = objList.Valor("Moeda");
					ord.date = objList.Valor("Data");
					ord.condPag = getCondPagamentoById(objList.Valor("CondPag"));
					ord.modExpedicao = getModExpedicaoById(objList.Valor("ModoExp"));
					ord.estadoFact = invoiceState(ord.id,ord.docNum);
					ord.expedido = shippingState(ord.docNum);
					listOrders.Add(ord);
					objList.Seguinte();
				}
				return listOrders;
			}
			else
				return null;
		}

		private static string shippingState(int docNum)
		{
			string state = null;
			StdBELista objList;
			if (PriEngine.Platform.Inicializada)
			{
				string ids = getCabecDocIds(docNum);
				string[] id_arr = ids.Split(',');
				string where = "(";
				for (int i = 0; i < id_arr.Length; i++)
				{
					if (i > 0) where += " or ";
					where += "Id='" + id_arr[i] + "'";
				}

				where += ")";
				if (where.Length == 7)
				{
					state = "Pendente";
				}
				else
				{
					string query = "SELECT * FROM PRIBELAFLOR.dbo.CabecDoc where " + where + " and TipoDoc='GT'";
					objList = PriEngine.Engine.Consulta(query);
					if (objList.NumLinhas() >= 1)
					{
						state = "Expedido";
					}
					else
					{
						state = "Pendente";
					}
				}
				
			}
			return state;
		}

		/**
		 * return invoice state: [fully paid; partially paid : x/total]
		 * 
		 * */
		private static string invoiceState(string id,int numdoc)
		{
			string state = null;
			double sum = 0.0,total;
			StdBELista objList;
			if (PriEngine.Platform.Inicializada)
			{
				String query = "SELECT Estado FROM PRIBELAFLOR.dbo.CabecDocStatus WHERE IdCabecDoc='" + id.Replace("{", "").Replace("}", "") + "'";
				objList = PriEngine.Engine.Consulta(query);
				if (false)//String.Compare(objList.Valor("Estado"), "T")==0)
				{
					//invoice fully paid for ecl: id 
					state="Pago";
				}
				else
				{
					string ids = getCabecDocIds(numdoc);
					string[] id_arr = ids.Split(',');
					string where = "(";
					for (int i = 0; i < id_arr.Length; i++)
					{
						if (i > 0) where += " or ";
						where += "Id='" + id_arr[i] + "'";
					}

					where += ")";
					if (where.Length == 7) state = "Pendente";
					else
					{
						query = "SELECT * FROM PRIBELAFLOR.dbo.CabecDoc where " + where +" and TipoDoc='FA'";
						objList = PriEngine.Engine.Consulta(query);
						while (!objList.NoFim()) {
							sum += objList.Valor("TotalMerc");
							objList.Seguinte();
						}
						total = getEclTotalMerc(numdoc);
						if (sum == getEclTotalMerc(numdoc))
						{
							//means fully paid, if this happens it will invalidate first if clause
							state = "Pago (totalmente).";
						}
						else
						{
							state = "Parcial (" + sum + "/" + total + ")";
						}
					}
					
				}
			}
			return state;
		}

		public static double getEclTotalMerc(int numDoc) {
			String query = "SELECT TotalMerc FROM PRIBELAFLOR.dbo.CabecDoc WHERE TipoDoc='ECL' and NumDoc=" + numDoc;
			StdBELista objlist = PriEngine.Engine.Consulta(query);
			return objlist.Valor("TotalMerc");
		}

		public static string getCabecDocIds(int eclId)
		{
			string res = "";
			int i = 0;
			string query = "SELECT IdCabecDoc FROM PRIBELAFLOR.dbo.LinhasDoc where Descricao like 'ECL Nº" + eclId + "%'";
			StdBELista objList = PriEngine.Engine.Consulta(query);
			while (!objList.NoFim())
			{
				if (i > 0) res += ",";
				i++;
				res += objList.Valor("IdCabecDoc");
				objList.Seguinte();
			}
			return res.Replace("{","").Replace("}","");
		}

		private static string getCondPagamentoById(string id)
		{
			//workaround
			if (id.Length == 1) id = "0" + id;
			//----------
			string query = "select descricao from PRIBELAFLOR.dbo.condPag where condpag=" + id;
			StdBELista objList = PriEngine.Engine.Consulta(query);
			return objList.Valor("Descricao");
		}

		private static string getModExpedicaoById(string id)
		{
			if (id == "") return "Nao definido.";
			string query = "select descricao from PRIBELAFLOR.dbo.ModosExp where ModoExp='" + id + "'";
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
				String query = "SELECT * FROM PRIBELAFLOR.dbo.CabecDoc where entidade='" + cliente + "'";
				objList = PriEngine.Engine.Consulta(query);
				while (!objList.NoFim())
				{
					ord = new Model.Order();
					ord.id = objList.Valor("Id");
					ord.docNum = objList.Valor("NumDoc");
					ord.CodClient = objList.Valor("Entidade");
					ord.modPag = objList.Valor("ModoPag");
					ord.numContrib = objList.Valor("NumContribuinte");
					ord.totalMerc = objList.Valor("TotalMerc");
					ord.totalIva = objList.Valor("TotalIva");
					ord.moeda = objList.Valor("Moeda");
					ord.date = objList.Valor("Data");
					ord.condPag = getCondPagamentoById(objList.Valor("CondPag"));
					ord.modExpedicao = getModExpedicaoById(objList.Valor("ModoExp"));
					ord.estadoFact = invoiceState(ord.id, ord.docNum);
					ord.expedido = shippingState(ord.docNum);
					listOrders.Add(ord);
					objList.Seguinte();
				}
				return listOrders;
			}
			else
				return null;
		}

		public static List<Model.Vendedores> VendedoresList()
		{
			ErpBS objMotor = new ErpBS();
			//MotorPrimavera mp = new MotorPrimavera();
			StdBELista objList;
			
			Model.Vendedores seller = new Model.Vendedores();
			List<Model.Vendedores> listVendedores = new List<Model.Vendedores>();
			if (PriEngine.Platform.Inicializada)
			{

				//if (PriEngine.InitializeCompany("BELAFLOR", "admin", "admin") == true){
				String query = "SELECT * FROM PRIEMPRE.dbo.Utilizadores WHERE PerfilSugerido = 'Comercial I' AND activo = '1' ";
				objList = PriEngine.Engine.Consulta(query);
				while (!objList.NoFim())
				{
					seller = new Model.Vendedores();
					seller.Nome = objList.Valor("Nome");
					seller.Email = objList.Valor("Email");
					seller.Tel = objList.Valor("Telemovel");
					listVendedores.Add(seller);
					objList.Seguinte();
				}
				return listVendedores;
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
			if (from != "nd") query += " and Data>='" + from + "'";
			if (to != "nd") query += " and Data<='" + to + "'";

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
					ord.id = objList.Valor("Id");
					ord.docNum = objList.Valor("NumDoc");
					ord.CodClient = objList.Valor("Entidade");
					ord.modPag = objList.Valor("ModoPag");
					ord.numContrib = objList.Valor("NumContribuinte");
					ord.totalMerc = objList.Valor("TotalMerc");
					ord.totalIva = objList.Valor("TotalIva");
					ord.moeda = objList.Valor("Moeda");
					ord.date = objList.Valor("Data");
					ord.condPag = getCondPagamentoById(objList.Valor("CondPag"));
					ord.modExpedicao = getModExpedicaoById(objList.Valor("ModoExp"));
					ord.estadoFact = invoiceState(ord.id, ord.docNum);
					ord.expedido = shippingState(ord.docNum);
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
			if (PriEngine.Platform.Inicializada)
			{

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