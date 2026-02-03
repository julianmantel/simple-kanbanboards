using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleKanbanBoards.Business.Templates.ITemplate
{
    public interface IEmailTemplateBuilder
    {
        string BuildResetPasswordEmail(string url);
    }
}
