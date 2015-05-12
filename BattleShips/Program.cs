using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace ShopService2
	{
		class Program
		{
			private List<ServiceHost> hosts = new List<ServiceHost>();

			public Program()
			{
				//hosts.Add(new ServiceHost(typeof(ProductService)));
				//hosts.Add(new ServiceHost(typeof(UserService)));
				hosts.Add(new ServiceHost(typeof(Service.ShopService)));
			}
			public void Open()
			{
				foreach (ServiceHost sh in hosts)
				{
					sh.Open();
				}
			}

			public void Close()
			{
				foreach (ServiceHost sh in hosts)
				{
					sh.Close();
				}
			}

			static void Main(string[] args)
			{

				Program p = new Program();
				Console.WriteLine("Starting service");
				p.Open();
				Console.WriteLine("Service Started");

				Console.ReadLine();

				Console.WriteLine("Stopping Service");
				p.Close();
				Console.WriteLine("Services Stopped");
			}
		}
	}