﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Signal_R.ViewModels
{
    public class LoginVM
    {
       public string UserName { get; set; }

        [DataType(DataType.Password)]

       public string Password { get; set; }
    }
}
