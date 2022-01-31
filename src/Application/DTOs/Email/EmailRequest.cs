using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Email
{
    public class EmailRequest
    {
        // Ваша почта
        public string From { get; set; }
        // Кому письмо
        public string To { get; set; }
        // Тема письма
        public string Subject { get; set; }
        // Текст письма
        public string Body { get; set; }
    }
}
