using System.Security.Cryptography;
using System.Text;

namespace WebApi.Common
{
    public static class Helpers
    {
		public static string GetPasswordHash(string s)
		{
			if (s == null)
				return null;
			using var hashAlgorithm = SHA512.Create();
			var hash = hashAlgorithm.ComputeHash(Encoding.Unicode.GetBytes(s));
			return string.Concat(hash.Select(item => item.ToString("x2")));
		}
	}
}
