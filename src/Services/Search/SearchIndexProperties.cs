using System;

namespace Arashi.Services.Search
{
	public class SearchIndexProperties
	{
		public int NumberOfDocuments { get; set; }
		public DateTime LastModified { get; set; }
		public string IndexDirectory { get; set; }
	}
}