using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstREST.Lib_Primavera.Model
{
	public class Order
	{
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

		public string modPag
		{
			get;
			set;
		}

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
	}
}