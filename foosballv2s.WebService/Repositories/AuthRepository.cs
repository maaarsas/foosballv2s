
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace foosballv2s.WebService.Models
{
    /// <summary>
    /// A class for authentication
    /// </summary>
    public class AuthRepository : IAuthRepository
    {
        private readonly IIdentityContext _context;

        public AuthRepository(IIdentityContext context)
        {
            _context = context;
        }
        
    }
}