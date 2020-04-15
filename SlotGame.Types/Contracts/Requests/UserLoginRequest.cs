using System;
using System.Collections.Generic;
using System.Text;

namespace SlotGame.Types.Contracts.Requests
{
    public class UserLoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
