using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureFileShareTest
{
	class Program
	{
		static void Main(string[] args)
		{
			StorageAccountCredentials credentials = new StorageAccountCredentials();
			credentials.AccessKey = "";
			credentials.AccountName = "";
			credentials.FileShareName = "";
			FileShareWorker worker = new FileShareWorker(credentials);
			worker.Init();
			worker.GetVolumeLabel();
			//worker.SetVolumeLabel("datadisk");
			worker.GetDirectoryContent(".", "*");
		}
	}
}
