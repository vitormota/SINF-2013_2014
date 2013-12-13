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
			String state_date;
			ErpBS objMotor = new ErpBS();
			//MotorPrimavera mp = new MotorPrimavera();
			StdBELista objList;
			StdBELista objdate;
			Model.Order ord = new Model.Order();
			List<Model.Order> listOrders = new List<Model.Order>();
			if (PriEngine.Platform.Inicializada)
			{
				//if (PriEngine.InitializeCompany("BELAFLOR", "admin", "admin") == true){
				String query = "SELECT * FROM PRIBELAFLOR.dbo.CabecDoc WHERE TipoDoc='ECL'";
				String date_query = "SELECT CURRENT_TIMESTAMP as date";
				objdate = PriEngine.Engine.Consulta(date_query);
				objList = PriEngine.Engine.Consulta(query);
				while (!objList.NoFim())
				{
					ord = new Model.Order();
					ord.id = objList.Valor("Id");
					ord.responsable = objList.Valor("Utilizador");
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
					state_date = invoiceStateAndDate(ord.id, ord.docNum);
					ord.estadoFact = state_date.Split('|')[0];
					ord.expedido = shippingState(ord.docNum);
					if (String.Compare(ord.expedido, "Expedido") == 0 && String.Compare(ord.estadoFact,"Pago (totalmente)")==0)
					{
						//means this order is finalized
						ord.lastUpdated = "Completa";
					}
					else
					{
						//calculate date diff
						if (String.Compare(ord.estadoFact, "Pendente") == 0)
						{
							ord.lastUpdated = dateDiff(ord.date.ToString(),ord.date.ToString()).ToString();
						}
						else
						{
							ord.lastUpdated = dateDiff(state_date.Split('|')[1],ord.date.ToString()).ToString();
						}
					}
					listOrders.Add(ord);
					objList.Seguinte();
				}
				return listOrders;
			}
			else
				return null;
		}

		private static int dateDiff(String d1,String eclDate)
		{
			if (d1.Length == 0)
			{
				d1 = eclDate;
			}
			StdBELista obj = PriEngine.Engine.Consulta("DECLARE @startdate datetime2 = CURRENT_TIMESTAMP;"+
				"DECLARE @endDate datetime2 = convert(datetime, '"+d1+"', 103);"+
				"SELECT DATEDIFF(day,@endDate , @startdate) AS DiffDate");
			return obj.Valor("DiffDate");
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
		private static string invoiceStateAndDate(string id,int numdoc)
		{
			string state = null;
			string date = null;
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
						query = "SELECT Data,TotalMerc FROM PRIBELAFLOR.dbo.CabecDoc where " + where +" and TipoDoc='FA'";
						objList = PriEngine.Engine.Consulta(query);
						while (!objList.NoFim()) {
							sum += objList.Valor("TotalMerc");
							date = ((DateTime) objList.Valor("Data")).ToString();
							objList.Seguinte();
						}
						total = getEclTotalMerc(numdoc);
						if (sum == getEclTotalMerc(numdoc))
						{
							//means fully paid, if this happens it will invalidate first if clause
							state = "Pago (totalmente)|"+date;
						}
						else
						{
							state = "Parcial (" + sum + "/" + total + ")|"+date;
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
			string query = "SELECT IdCabecDoc FROM PRIBELAFLOR.dbo.LinhasDoc where Descricao like 'ECL Nº" + eclId + "/%'";
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
			String state_date;
			StdBELista objdate;
			ErpBS objMotor = new ErpBS();
			//MotorPrimavera mp = new MotorPrimavera();
			StdBELista objList;
			Model.Order ord = new Model.Order();
			List<Model.Order> listOrders = new List<Model.Order>();
			if (PriEngine.Platform.Inicializada)
			{

				//if (PriEngine.InitializeCompany("BELAFLOR", "admin", "admin") == true){
				String query = "SELECT * FROM PRIBELAFLOR.dbo.CabecDoc where entidade='" + cliente + "' and TipoDoc='ECL'";
				String date_query = "SELECT CURRENT_TIMESTAMP as date";
				objdate = PriEngine.Engine.Consulta(date_query);
				objList = PriEngine.Engine.Consulta(query);
				while (!objList.NoFim())
				{
					ord = new Model.Order();
					ord.id = objList.Valor("Id");
					ord.responsable = objList.Valor("Utilizador");
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
					state_date = invoiceStateAndDate(ord.id, ord.docNum);
					ord.estadoFact = state_date.Split('|')[0];
					ord.expedido = shippingState(ord.docNum);
					if (String.Compare(ord.expedido, "Expedido") == 0 && String.Compare(ord.estadoFact, "Pago (totalmente)") == 0)
					{
						//means this order is finalized
						ord.lastUpdated = "Completa";
					}
					else
					{
						//calculate date diff
						if (String.Compare(ord.estadoFact, "Pendente") == 0)
						{
							ord.lastUpdated = dateDiff(ord.date.ToString(), ord.date.ToString()).ToString();
						}
						else
						{
							ord.lastUpdated = dateDiff(state_date.Split('|')[1], ord.date.ToString()).ToString();
						}
					}
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
			String state_date;
			StdBELista objdate;
			string query = "SELECT * FROM PRIBELAFLOR.dbo.CabecDoc where entidade='" + cliente + "' and TipoDoc='ECL'";
			if (from != "nd") query += " and Data>='" + from + "'";
			if (to != "nd") query += " and Data<='" + to + "'";

			ErpBS objMotor = new ErpBS();
			StdBELista objList;
			Model.Order ord = new Model.Order();
			List<Model.Order> listOrders = new List<Model.Order>();
			if (PriEngine.Platform.Inicializada)
			{
				String date_query = "SELECT CURRENT_TIMESTAMP as date";
				objdate = PriEngine.Engine.Consulta(date_query);
				objList = PriEngine.Engine.Consulta(query);
				while (!objList.NoFim())
				{
					ord = new Model.Order();
					ord.id = objList.Valor("Id");
					ord.responsable = objList.Valor("Utilizador");
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
					state_date = invoiceStateAndDate(ord.id, ord.docNum);
					ord.estadoFact = state_date.Split('|')[0];
					ord.expedido = shippingState(ord.docNum);
					if (String.Compare(ord.expedido, "Expedido") == 0 && String.Compare(ord.estadoFact, "Pago (totalmente)") == 0)
					{
						//means this order is finalized
						ord.lastUpdated = "Completa";
					}
					else
					{
						//calculate date diff
						if (String.Compare(ord.estadoFact, "Pendente") == 0)
						{
							ord.lastUpdated = dateDiff(ord.date.ToString(), ord.date.ToString()).ToString();
						}
						else
						{
							ord.lastUpdated = dateDiff(state_date.Split('|')[1], ord.date.ToString()).ToString();
						}
					}
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


		// Clientes
		public static List<Model.Utilizador> ListaUtilizadores()
		{
			ErpBS objMotor = new ErpBS();
			//MotorPrimavera mp = new MotorPrimavera();
			StdBELista objList;
			Model.Utilizador cli = new Model.Utilizador();
			List<Model.Utilizador> listClientes = new List<Model.Utilizador>();
			if (PriEngine.Platform.Inicializada)
			{
				//if (PriEngine.InitializeCompany("BELAFLOR", "admin", "admin") == true){
				//objList = PriEngine.Engine.Comercial.Clientes.LstClientes();
				String query = "SELECT * FROM PRIBELAFLOR.dbo.Clientes";
				objList = PriEngine.Engine.Consulta(query);
				while (!objList.NoFim())
				{
					cli = new Model.Utilizador();
					cli.Cod = objList.Valor("Cliente");
					cli.Tipo = 3;
					cli.Nome = objList.Valor("Nome");
					cli.Morada = objList.Valor("Fac_Mor");
					cli.Localidade = objList.Valor("Fac_Local");
					cli.CP = objList.Valor("Fac_Cp");
					cli.CPLocal = objList.Valor("Fac_Cploc");
					cli.Telefone = objList.Valor("Fac_Tel");
					cli.Fax = objList.Valor("Fac_Fax");
					cli.Pais = objList.Valor("Pais");
					cli.Idioma = objList.Valor("Idioma");
					cli.Moeda = objList.Valor("Moeda");
					cli.NumContribuinte = objList.Valor("NumContrib");
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


		public static Lib_Primavera.Model.Utilizador GetClienteUser(string codCliente)
        {
            ErpBS objMotor = new ErpBS();
            StdBELista objCli;
            Model.Utilizador mycli = new Model.Utilizador();
            if (PriEngine.InitializeCompany("BELAFLOR", "admin", "admin") == true)
            {
                //if (PriEngine.InitializeCompany("BELAFLOR", "admin", "admin") == true){
                String query = "SELECT * FROM PRIBELAFLOR.dbo.Clientes where Cliente = '" + codCliente + "'";
                objCli = PriEngine.Engine.Consulta(query);
                if(objCli.NumLinhas() > 0)
                {
                    mycli.Cod = objCli.Valor("Cliente");
                    mycli.Tipo = 3;
                    mycli.Nome = objCli.Valor("Nome");
                    mycli.Morada = objCli.Valor("Fac_Mor");
                    mycli.Localidade = objCli.Valor("Fac_Local");
                    mycli.CP = objCli.Valor("Fac_Cp");
                    mycli.CPLocal = objCli.Valor("Fac_Cploc");
                    mycli.Telefone = objCli.Valor("Fac_Tel");
                    mycli.Fax = objCli.Valor("Fac_Fax");
                    mycli.Pais = objCli.Valor("Pais");
                    mycli.Idioma = objCli.Valor("Idioma");
                    mycli.Moeda = objCli.Valor("Moeda");
                    mycli.NumContribuinte = objCli.Valor("NumContrib");
                    return mycli;
                }
                else
                {
                    return null;
                }
            }
            else
                return null;
        }


		//public static Lib_Primavera.Model.Cliente GetCliente(string codCliente)
		//{
		//	ErpBS objMotor = new ErpBS();
		//	GcpBECliente objCli = new GcpBECliente();
		//	Model.Cliente myCli = new Model.Cliente();
		//	if (PriEngine.InitializeCompany("BELAFLOR", "", "") == true)
		//	{
		//		if (PriEngine.Engine.Comercial.Clientes.Existe(codCliente) == true)
		//		{
		//			objCli = PriEngine.Engine.Comercial.Clientes.Edita(codCliente);
		//			myCli.CodCliente = objCli.get_Cliente();
		//			myCli.NomeCliente = objCli.get_Nome();
		//			myCli.Moeda = objCli.get_Moeda();
		//			myCli.NumContribuinte = objCli.get_NumContribuinte();
		//			return myCli;
		//		}
		//		else
		//		{
		//			return null;
		//		}
		//	}
		//	else
		//		return null;
		//}
		public static Lib_Primavera.Model.RespostaErro UpdCliente(Lib_Primavera.Model.Utilizador cliente)
		{
			Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();
			ErpBS objMotor = new ErpBS();
			GcpBECliente objCli = new GcpBECliente();
			try
			{
				if (PriEngine.InitializeCompany("BELAFLOR", "", "") == true)
				{
					if (PriEngine.Engine.Comercial.Clientes.Existe(cliente.Cod) == false)
					{
						erro.Erro = 1;
						erro.Descricao = "O cliente não existe";
						return erro;
					}
					else
					{
						objCli = PriEngine.Engine.Comercial.Clientes.Edita(cliente.Cod);
						objCli.set_EmModoEdicao(true);
						objCli.set_Nome(cliente.Nome);
						objCli.set_Morada(cliente.Morada);
						objCli.set_Localidade(cliente.Localidade);
						objCli.set_CodigoPostal(cliente.CP);
						objCli.set_LocalidadeCodigoPostal(cliente.CPLocal);
						objCli.set_Telefone(cliente.Telefone);
						objCli.set_Fax(cliente.Fax);
						objCli.set_Pais(cliente.Pais);
						objCli.set_Idioma(cliente.Idioma);
						objCli.set_Moeda(cliente.Moeda);
						objCli.set_NumContribuinte(cliente.NumContribuinte);
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

// Administradores
        public static Lib_Primavera.Model.Utilizador GetAdmin(string codAdmin)
        {
            ErpBS objMotor = new ErpBS();
            StdBELista objAdm;
            Model.Utilizador myAdm = new Model.Utilizador();
            String query = "SELECT * FROM PRIEMPRE.dbo.Utilizadores WHERE Administrador = '1' AND activo = '1' AND Codigo = '" + codAdmin + "'";
            objAdm = PriEngine.Engine.Consulta(query);
            if (PriEngine.Platform.Inicializada)
            {
                myAdm.Cod = objAdm.Valor("Codigo");
                myAdm.Tipo = 1;
                myAdm.Nome = objAdm.Valor("Nome");
                myAdm.Email = objAdm.Valor("Email");
                myAdm.Telefone = objAdm.Valor("Telemovel");
                myAdm.Idioma = objAdm.Valor("Idioma");
                return myAdm;

            }
            else
                return null;
        }

// Artigos
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
		//// ? preciso rever isto
		//public static Lib_Primavera.Model.Admin GetAdmin(string codAdmin)
		//{
		//	/*ErpBS objMotor = new ErpBS();
		//	GcpBEAdmin objAdm = new GcpBEAdmin(); // erro aqui
		//	Model.Admin myAdm = new Model.Admin();
		//	if (PriEngine.InitializeCompany("BELAFLOR", "", "") == true)
		//	{
		//		if (PriEngine.Engine.Comercial.Admins.Existe(codAdmin) == true) // erro aqui
		//		{
		//			objAdm = PriEngine.Engine.Comercial.Admins.Edita(codAdmin); // erro aqui
		//			myAdm.CodAdmin = objAdm.get_Cliente();
		//			myAdm.NomeAdmin = objAdm.get_Nome();
		//			myAdm.Moeda = objAdm.get_Moeda();
		//			myAdm.NumContribuinte = objAdm.get_NumContribuinte();
		//			return myAdm;
		//		}
		//		else
		//		{
		//			return null;
		//		}
		//	}
		//	else*/
		//		return null;
		//}

		//public static Lib_Primavera.Model.Artigo GetArtigo(string codArtigo)
		//{
		//	// ErpBS objMotor = new ErpBS();
		//	GcpBEArtigo objArtigo = new GcpBEArtigo();
		//	Model.Artigo myArt = new Model.Artigo();
		//	if (PriEngine.InitializeCompany("BELAFLOR", "", "") == true)
		//	{
		//		if (PriEngine.Engine.Comercial.Artigos.Existe(codArtigo) == false)
		//		{
		//			return null;
		//		}
		//		else
		//		{
		//			objArtigo = PriEngine.Engine.Comercial.Artigos.Edita(codArtigo);
		//			myArt.CodArtigo = objArtigo.get_Artigo();
		//			myArt.DescArtigo = objArtigo.get_Descricao();
		//			return myArt;
		//		}
		//	}
		//	else
		//	{
		//		return null;
		//	}
		//}

        public static List<Model.Admin> AdminsList()
        {
            ErpBS objMotor = new ErpBS();
            //MotorPrimavera mp = new MotorPrimavera();
            StdBELista objList;

            Model.Admin adm = new Model.Admin();
            List<Model.Admin> listAdmins = new List<Model.Admin>();
            if (PriEngine.Platform.Inicializada)
            {

                //if (PriEngine.InitializeCompany("BELAFLOR", "admin", "admin") == true){
                String query = "SELECT * FROM PRIEMPRE.dbo.Utilizadores WHERE PerfilSugerido = 'Administrador' AND activo = '1' "; //? verificar
                objList = PriEngine.Engine.Consulta(query);
                while (!objList.NoFim())
                {
                    adm = new Model.Admin();
                    adm.NomeAdmin = objList.Valor("Nome");
                    listAdmins.Add(adm);
                    objList.Seguinte();
                }
                return listAdmins;
            }
            else
                return null;
        }
    }
}