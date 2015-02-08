using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureFileShareTest
{
	internal class StorageAccountCredentials
	{
		public string AccountName { get; set; }
		public string AccessKey { get; set; }
		public string FileShareName { get; set; }
	}
}
