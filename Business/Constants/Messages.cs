using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Constants
{
    public static class Messages
    {
        public static string ProductAdded = "Ürün eklendi.";
        public static string ProductNameInvalid = "Ürün ismi geçersiz.";
        public static string MaintenanceTime = "Sistemimiz şu an bakımdadır. Ancak saat 23.00 dan sonra işlem yapılabilir.";
        public static string ProductsListed = "Ürünler başarıyla listelendi.";
        public static string ProductCountOfCategoryError = "Bir kategoride en fazla 10 ürün olabilir.";
        public static string ProductNameAlreadyExists = "Eklemek istediğiniz ürünle aynı isimde başka bir ürün bulunmaktadır.";
        public static string CategoryLimitExceded = "Çok fazla kategori sayısı olduğundan ürün eklenemedi.";
        public static string CategoriesListed = "Kategoriler başarıyla listelendi.";
    }
}
