using Business.Abstract;
using Business.CCS;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        IProductDal _productDal;
        ICategoryService _categoryService;

        public ProductManager(IProductDal productDal, ICategoryService categoryService)
        {
            _productDal = productDal;
            _categoryService = categoryService;
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

            //Log Metodlarını AOP kullanarak tek attribute ile yapabiliriz. Bu bize tek merkezden yönetimi ve sürdürebilir yazılımı sağlar.
            //_logger.Log();
            //try
            //{
            //    _productDal.Add(product);
            //    return new SuccessResult(Messages.ProductAdded);
            //}
            //catch (Exception exception)
            //{
            //    _logger.Log();
            //}
            //return new ErrorResult();

            //if (CheckIfProductCountOfCategoryCorrect(product.CategoryId).Success)
            //{
            //    if (CheckIfProductNameExists(product.ProductName).Success)
            //    {
            //        _productDal.Add(product);
            //        return new SuccessResult(Messages.ProductAdded);
            //    }
            //    return new ErrorResult(Messages.ProductNameAlreadyExists);
            //}
            //return new ErrorResult(Messages.ProductCountOfCategoryError);

            // İş Motoru kullanarak Clean Code yazmış olduk.
            IResult result = BusinessRules.Run(CheckIfProductCountOfCategoryCorrect(product.CategoryId), CheckIfProductNameExists(product.ProductName),
                CheckIfCategoryLimitExceded());
            if (result != null)
            {
                return result;
            }
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

        [ValidationAspect(typeof(ProductValidator))]
        public IResult Update(Product product)
        {
            throw new NotImplementedException();
        }

        private IResult CheckIfProductCountOfCategoryCorrect(int categoryId)
        {
            if (_productDal.GetAll(c => c.CategoryId == categoryId).Count >= 10)
            {
                return new ErrorResult(Messages.ProductCountOfCategoryError);
            }
            return new SuccessResult();
        }

        private IResult CheckIfProductNameExists(string productName)
        {
            if (_productDal.GetAll(c => c.ProductName == productName).Any())
            {
                return new ErrorResult(Messages.ProductNameAlreadyExists);
            }
            return new SuccessResult();
        }

        //private IResult CheckIfCategoryCountIsMuch()
        //{
        //    ICategoryDal _categoryDal= new DataAccess.Concrete.EntityFramework.EfCategoryDal();
        //    var result = _categoryDal.GetAll().Count;
        //    if (result > 15)
        //    {
        //        return new ErrorResult(Messages.CategoryCountIsMuch);
        //    }
        //    return new SuccessResult();
        //}

        private IResult CheckIfCategoryLimitExceded()
        {
            int result = _categoryService.GetAll().Data.Count;
            if (result >15)
            {
                return new ErrorResult(Messages.CategoryLimitExceded);
            }
            return new SuccessResult();
        }
    }
}
