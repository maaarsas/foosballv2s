
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
        private readonly IWebServiceDbContext _context;

        public AuthRepository(IWebServiceDbContext context)
        {
            _context = context;
        }
        
    }
}