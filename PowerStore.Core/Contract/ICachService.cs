using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerStore.Core.Contract
{
    public interface ICachService
    {
        Task CacheOtpAsync(string phone, string otp);
        Task<string> GetCachedOtpAsync(string phone);
    }
}
