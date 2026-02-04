using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleKanbanBoards.Business.Exceptions
{
    public class NotFoundException : AppException
    {
        public override int StatusCode => 404;
        public NotFoundException(string message) : base(message) { }
    }
}
