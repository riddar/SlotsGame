using System;
using System.Collections.Generic;
using System.Text;

namespace SlotGame.Types.Contracts.Responses
{
    public class AuthSuccessResponse
    {
        public string Token { get; set; }    
    }

    public class AuthFailedResponse
    {
        public IEnumerable<string> Errors { get; set; }
    }
}
