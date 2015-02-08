using System;
using System.Text.RegularExpressions;
using Microsoft.WindowsAzure.Storage.File;

namespace AzureFileShareTest
{
	internal class FileShareHelper
	{
		public static Regex CreateRegexFromMask(string mask)
		{
			return new Regex(mask.Replace(".", @"\.").Replace("?", @"\S").Replace("*", @"\S*"), RegexOptions.IgnoreCase);
		}

		public static FileSystemEntry CreateDictionaryEntry(IListFileItem listFileItem)
		{
			if (listFileItem is CloudFile)
			{
				return CreateDictionaryEntry((CloudFile) listFileItem);
			}
			if (listFileItem is CloudFileDirectory)
			{
				return CreateDictionaryEntry((CloudFileDirectory) listFileItem);
			}
			throw new ArgumentException("Argument type mismatch.", "listFileItem");
		}

		public static FileSystemEntry CreateDictionaryEntry(CloudFile file)
		{
			file.FetchAttributes();
			FileEntry retFile = new FileEntry();
			retFile.ETag = file.Properties.ETag;
			retFile.Name = file.Name;
			retFile.Length = file.Properties.Length;
			retFile.LastAccess = file.Properties.LastModified.HasValue
				? file.Properties.LastModified.Value.UtcDateTime
				: (DateTime?) null;
			return retFile;
		}

		public static FileSystemEntry CreateDictionaryEntry(CloudFileDirectory directory)
		{
			directory.FetchAttributes();
			DictionaryEntry retDictionary = new DictionaryEntry();
			retDictionary.ETag = directory.Properties.ETag;
			retDictionary.Name = directory.Name;
			retDictionary.LastAccess = directory.Properties.LastModified.HasValue
				? directory.Properties.LastModified.Value.UtcDateTime
				: (DateTime?) null;
			return retDictionary;
		}
	}
}