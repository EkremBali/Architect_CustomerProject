using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;

namespace CustomerProject.WebUI.Extensions
{
	//TempData'da model taşıyabilmek için gerekli JSON işlemlerinin yapıldığı extension.
	public static class TempDataExtensions
    {
        //Kendisine gelen model ve key bilgilerini alır. Key'i kullanarak tempData oluşturur ve modeli json'a çevirerek bu tempdata'ya atar.
        public static void Put<T>(this ITempDataDictionary tempData, string key, T value) where T : class
        {
            tempData[key] = JsonConvert.SerializeObject(value);
        }

		//Kendisine gelen key'i tempData'da okur ve elde ettiği json yapısını generic olarak gelen modele çevirir.
		public static T Get<T>(this ITempDataDictionary tempData, string key) where T : class
        {
            return JsonConvert.DeserializeObject<T>(tempData[key] as string);
        }
    }
}
