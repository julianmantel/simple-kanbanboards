using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleKanbanBoards.Business.Exceptions
{
    public abstract class AppException : Exception
    {
        public abstract int StatusCode { get; }
        protected AppException(string message) : base(message) { }
    }
}
