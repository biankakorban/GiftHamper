using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace GrandeGift.Services
{
	//instances of this class cannot be created
	public static class SessionHelper
	{
		//set the OrderItems as Json data inside the session variable
		//key is representing the session variable name
		//value is basically all the OrderLine
		public static void SetObjectAsJson(this ISession session, string key, object value)
		{
			session.SetString(key, JsonConvert.SerializeObject(value));
		}

		//retrieve all session data which is Json obejcts and convert them to real object
		public static T GetObjectFromJson<T>(this ISession session, string key)
		{
			var value = session.GetString(key);
			//condition ? consequent : alternative
			//the conditional operator ?:
			// is this condition true ? yes :  no
			return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
		}
	}
}
