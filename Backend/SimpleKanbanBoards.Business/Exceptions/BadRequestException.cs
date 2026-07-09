using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleKanbanBoards.Business.Exceptions
{
    public class BadRequestException : AppException
    {
        public override int StatusCode => 400;
        public BadRequestException(string message) : base(message) { }
    }
}
