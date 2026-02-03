using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleKanbanBoards.Business.Templates.ITemplate;

namespace SimpleKanbanBoards.Business.Templates
{
    public class EmailTemplateBuilder : IEmailTemplateBuilder
    {
        public string BuildResetPasswordEmail(string url)
        {
            return $@"<!DOCTYPE html>
        <html>
        <body>
            <h2>Change Password</h2>
            <p>For change your password:</p>
            <a href=""{url}"">Click here</a>
        </body>
        </html>";
        }
    }
}
