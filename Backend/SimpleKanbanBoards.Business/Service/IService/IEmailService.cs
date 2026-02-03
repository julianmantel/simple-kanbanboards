using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleKanbanBoards.Business.Models.Email;

namespace SimpleKanbanBoards.Business.Service.IService
{
    public interface IEmailService
    {
        Task<string> SendEmailAsync(EmailRequestModel request);
    }
}
