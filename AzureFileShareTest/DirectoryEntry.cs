using System;

namespace AzureFileShareTest
{
	internal abstract class FileSystemEntry
	{
		public string Name { get; set; }
		public DateTime? LastAccess { get; set; }
		public string ETag { get; set; }
	}

	internal class FileEntry : FileSystemEntry
	{
		public long Length { get; set; }
	}
	internal class DictionaryEntry : FileSystemEntry
	{
	}
}