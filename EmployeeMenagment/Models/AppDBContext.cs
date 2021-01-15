using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeMenagment.Models
{
    public class AppDBContext :DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public AppDBContext(DbContextOptions<AppDBContext> options)
            :base(options)
        {
        }
    }
}
