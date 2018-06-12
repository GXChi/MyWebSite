using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebSite.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "用户名错误")]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public string RememberMe { get; set; }
    }
}
