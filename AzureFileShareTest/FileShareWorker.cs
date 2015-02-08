using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.File;

namespace AzureFileShareTest
{
	internal class FileShareWorker
	{
		private const string VolumeLabelKey = "ASDF_VolumeLabel";
		private const uint VolumeId = 0x21122012;

		private readonly CloudFileShare _share;
		private CloudFileDirectory _root;

		public FileShareWorker(StorageAccountCredentials credentials)
		{
			StorageCredentials storageCredentials = new StorageCredentials(credentials.AccountName, credentials.AccessKey);
			StorageUri fileStorageUri =
				new StorageUri(
					new Uri(string.Concat(@"https://", credentials.AccountName, @".file.core.windows.net/")));
			CloudStorageAccount storageAccount = new CloudStorageAccount(storageCredentials, null, null, null, fileStorageUri);
			CloudFileClient fileClient = storageAccount.CreateCloudFileClient();
			_share = fileClient.GetShareReference(credentials.FileShareName);
		}

		public void Init()
		{
			_root = _share.GetRootDirectoryReference();
			if (!_root.Exists())
			{
				_root.Create();
			}
		}

		public string GetVolumeLabel()
		{
			string volumeLabel;
			if (!_share.Metadata.TryGetValue(VolumeLabelKey, out volumeLabel))
			{
				volumeLabel = string.Empty;
			}
			return volumeLabel;
		}

		public void SetVolumeLabel(string volumeLabel)
		{
			if (_share.Metadata.ContainsKey(VolumeLabelKey))
			{
				_share.Metadata[VolumeLabelKey] = volumeLabel;
			}
			else
			{
				_share.Metadata.Add(VolumeLabelKey, volumeLabel);
			}
			_share.SetMetadata();
		}

		public uint GetVolumeId()
		{
			return VolumeId;
		}

		public IEnumerable<FileSystemEntry> GetDirectoryContent(string directoryName, string mask)
		{
			Regex fileMaskRegex = FileShareHelper.CreateRegexFromMask(mask);
			string fullPath = Path.GetFullPath(directoryName);
			string[] folders = Path.GetFullPath(directoryName).Remove(0, 3).Split('\\');
			CloudFileDirectory directory = _root;
			//foreach (string folder in folders)
			//{
			//	directory = directory.GetDirectoryReference(folder);
			//}
			OperationContext context = new OperationContext();
			//directory.FetchAttributes();
			IEnumerable<IListFileItem> listFilesAndDirectories = directory.ListFilesAndDirectories().ToArray();
			FileSystemEntry[] directoryContent = listFilesAndDirectories.Where(f => fileMaskRegex.IsMatch(f.Share.Name)).Select(FileShareHelper.CreateDictionaryEntry).ToArray();
			return directoryContent;
		}
	}
}
