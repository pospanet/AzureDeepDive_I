using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Redis
{
	class Program
	{
		static void Main(string[] args)
		{
			ConnectionMultiplexer connection = ConnectionMultiplexer.Connect("");
			IDatabase cache = connection.GetDatabase();
			const string Key = "Key";
			string value = "value";
			cache.StringSet(Key, value);
			value = cache.StringGet(Key);
		}
	}

	public static class RedisExtensions
	{
		public static T Get<T>(this IDatabase cache, string key)
		{
			return Deserialize<T>(cache.StringGet(key));
		}

		public static object Get(this IDatabase cache, string key)
		{
			return Deserialize<object>(cache.StringGet(key));
		}

		public static void Set(this IDatabase cache, string key, object value)
		{
			cache.StringSet(key, Serialize(value));
		}

		static byte[] Serialize(object o)
		{
			if (o == null)
			{
				return null;
			}

			BinaryFormatter binaryFormatter = new BinaryFormatter();
			using (MemoryStream memoryStream = new MemoryStream())
			{
				binaryFormatter.Serialize(memoryStream, o);
				byte[] objectDataAsStream = memoryStream.ToArray();
				return objectDataAsStream;
			}
		}

		static T Deserialize<T>(byte[] stream)
		{
			if (stream == null)
			{
				return default(T);
			}

			BinaryFormatter binaryFormatter = new BinaryFormatter();
			using (MemoryStream memoryStream = new MemoryStream(stream))
			{
				T result = (T)binaryFormatter.Deserialize(memoryStream);
				return result;
			}
		}
	}
}
