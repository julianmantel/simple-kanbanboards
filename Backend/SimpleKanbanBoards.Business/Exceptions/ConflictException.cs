using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleKanbanBoards.Business.Exceptions
{
    public class ConflictException : AppException
    {
        public override int StatusCode => 409;
        public ConflictException(string message) : base(message) { }
    }
}
