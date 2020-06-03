using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PhanonDataReader.Models {
    public class Lesson {
		private int _ID;
		private string _Title;
		
		[JsonPropertyName("id")]
		public int ID {
			get { return _ID; }
			set { _ID = value; }
		}

		[JsonPropertyName("title")]
		public string Title {
			get { return _Title; }
			set { _Title = value; }
		}
	}
}
