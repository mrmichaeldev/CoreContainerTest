using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreContainer.Models
{
    public class Result<T> : Result
    {
        public T Data { get; protected set; }

        public Result<T> WithData(T data)
        {
            Data = data;

            return this;
        }
    }

    public class Result
    {
        public bool IsSuccess { get; protected set; } = true;

        public Result WithSuccess()
        {
            IsSuccess = true;

            return this;
        }

        public string Error { get; protected set; }

        public Result WithError( string error)
        {
            Error = error;
            IsSuccess = false;

            return this;
        }
    }
}
