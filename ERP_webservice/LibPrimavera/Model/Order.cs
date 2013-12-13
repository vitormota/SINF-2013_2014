using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstREST.Lib_Primavera.Model
{
	public class Order
	{

		public string modExpedicao { get; set; }

		public string responsable { get; set; }

		public string id { get; set; }

		public string lastUpdated { get; set; }

		public int docNum { get; set; }

		public string CodClient
		{
			get;
			set;
		}

		public double totalMerc
		{
			get;
			set;
		}

		public double totalIva
		{
			get;
			set;
		}

		public DateTime date
		{
			get;
			set;
		}

		//public double total;

		//public double getTotal()
		//{
		//	return totalIva + totalMerc;
		//}

		public string modPag
		{
			get;
			set;
		}

		public string condPag { get; set; }

		public string numContrib
		{
			get;
			set;
		}

		public string moeda
		{
			get;
			set;
		}

		public string estadoFact { get; set; }
		public string expedido { get; set; }
	}
}