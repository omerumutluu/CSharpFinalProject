using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        IProductDal _productDal;

        public ProductManager(IProductDal productDal)
        {
            _productDal = productDal;
        }

        [ValidationAspect(typeof(ProductValidator))]
        public IResult Add(Product product)
        {
            // business codes (iş kodu)
            // validation codes (doğrulama kodu)
            // gönderilen nesnenin iş kodumuza uygun olup olmadığını kontrol etmeye doğrulama kodu denir.

            //ValidationTool.Validate(new ProductValidator(), product);
            //loglama
            //cacheremove
            //performance
            //transaction
            //yetkilendirme(authorization)


            // Attribute kullanarak aşağıdaki kod bloğundan kurtulmuş oluyoruz.

            // var context = new ValidationContext<Product>(product);
            // ProductValidator productValidator = new ProductValidator();
            // var result = productValidator.Validate(context);
            // if(!result.IsValid){
            //      throw new validationException(result.Errors);
            // }
            // else{
            //      _productDal.Add(product);
            // }

            //ValidationTool.Validate(new ProductValidator(), product);

            _productDal.Add(product);
            return new SuccessResult(Messages.ProductAdded);
        }

        public IDataResult<List<Product>> GetAll()
        {
            if (DateTime.Now.Hour == 1)
            {
                return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
            }
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(), Messages.ProductsListed);
        }

        public IDataResult<List<Product>> GetAllByCategoryId(int categoryId)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.CategoryId == categoryId));
        }

        public IDataResult<List<Product>> GetAllByUnitPrice(decimal min, decimal max)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.UnitPrice >= min && p.UnitPrice <= max));
        }

        public IDataResult<Product> GetById(int productId)
        {
            return new SuccessDataResult<Product>(_productDal.Get(p => p.ProductId == productId));
        }

        public IDataResult<List<ProductDetailDto>> GetProductDetails()
        {
            return new SuccessDataResult<List<ProductDetailDto>>(_productDal.GetProductDetails(), Messages.ProductsListed);
        }
    }
}
