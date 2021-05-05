using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Results
{
    public class Result : IResult
    {
        public Result(bool success,string message):this(success)
        {
            Message = message;
        }

        public Result(bool success)
        {
            Success = success;
        }

        public bool Success { get; }

        public string Message { get; }
    }
}

// SADECE GETTER OLAN PROPERTYLER READ-ONLY'DİR. BUNLAR SADECE CONSTRUCTOR İÇERİSİNDE SET EDİLEBİLİR.