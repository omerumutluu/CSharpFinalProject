﻿using Business.Concrete;
using DataAccess.Concrete.EntityFramework;
using DataAccess.Concrete.InMemory;
using System;

namespace ConsoleUI
{
    // SOLID
    // O = Open Closed Principle = yazılıma yeni bir özellik eklendiğinde mevcuttaki koduna dokunmadan yazılımın çalışması.
    class Program
    {
        static void Main(string[] args)
        {
            // DTO = Data Transformation Object
            // IoC Container

            // ProductManagerTest();
            // CategoryManagerTest();
        }

        private static void CategoryManagerTest()
        {
            CategoryManager categoryManager = new CategoryManager(new EfCategoryDal());

            foreach (var category in categoryManager.GetAll().Data)
            {
                Console.WriteLine(category.CategoryName);
            }
        }

        private static void ProductManagerTest()
        {
            ProductManager productManager = new ProductManager(new EfProductDal(), new CategoryManager(new EfCategoryDal()));

            var result = productManager.GetProductDetails();

            if (result.Success==true)
            {
                foreach (var product in result.Data)
                {
                    Console.WriteLine(product.ProductName + " - " + product.CategoryName);
                }
            }
            else
            {
                Console.WriteLine(result.Message);
            }
            
        }
    }
}
