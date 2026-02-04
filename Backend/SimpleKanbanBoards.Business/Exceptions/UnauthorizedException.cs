using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleKanbanBoards.Business.Exceptions
{
    public class UnauthorizedException : AppException
    {
        public override int StatusCode => 401;
        public UnauthorizedException(string message) : base(message) { }
    }
}
