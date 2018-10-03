using Ecard.Requests;
using Moonlit;
using System;

namespace Ecard.Services
{
    public class AccountRequest : PageRequest
    {
        public string DisplayName { get; set; }
        public string Mobile { get; set; }

        public DateTime? startTime { get; set; }

        public DateTime? endTime { get; set; }

    }
}