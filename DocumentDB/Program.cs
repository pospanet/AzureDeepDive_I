using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace DocumentDB
{
	class Program
	{
		private const string EndpointUrl = "";
		private const string AuthKey = "";
		private const string DatabasebId = "";
		private const string CollectionId = "";

		private const string FileName1 = "";
		private const string FileName2 = "";
		private static void Main(string[] args)
		{
			ConnectionPolicy policy = new ConnectionPolicy()
			{
				ConnectionMode = ConnectionMode.Direct,
				ConnectionProtocol = Protocol.Tcp
			};
			Uri endPoint=new Uri(EndpointUrl);
			using (DocumentClient client = new DocumentClient(endPoint, AuthKey, policy))
			{
				Database database = client.CreateDatabaseQuery().Where(db => db.Id == DatabasebId).AsEnumerable().First();
				DocumentCollection collection =
					client.CreateDocumentCollectionQuery(database.SelfLink).Where(c => c.Id == CollectionId).AsEnumerable().First();
				DataClass data1 = new DataClass()
				{
					Name = "Name 1",
					UniqueId = 1,
					Date = DateTime.UtcNow
				};
				DataClass data2 = new DataClass()
				{
					Name = "Name 2",
					UniqueId = 2,
					Date = DateTime.UtcNow
				};
				Task<ResourceResponse<Document>> td1 = client.CreateDocumentAsync(collection.DocumentsLink, data1);
				Task<ResourceResponse<Document>> td2 = client.CreateDocumentAsync(collection.DocumentsLink, data2);
				Task.WaitAll(td1, td2);
				Document doc1 = td1.Result.Resource;
				Document doc2 = td1.Result.Resource;
				Stream attStream1 = File.Open(FileName1, FileMode.Open, FileAccess.Read, FileShare.Read);
				Stream attStream2 = File.Open(FileName2, FileMode.Open, FileAccess.Read, FileShare.Read);
				client.CreateAttachmentAsync(doc1.AttachmentsLink, attStream1);
				client.CreateAttachmentAsync(doc2.AttachmentsLink, attStream2);
				DataClass[] data = client.CreateDocumentQuery<DataClass>(collection.DocumentsLink).Select(d => d).ToArray();
				var attachments = client.CreateAttachmentQuery(doc1.AttachmentsLink).ToArray();
			}
		}
	}

	public class DataClass
	{
		public string Name { get; set; }
		public int UniqueId { get; set; }
		public DateTime Date { get; set; }
	}
}
