using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace NewFinancialAPI.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            userIdentity.AddClaim(new Claim("HouseholdId", HouseholdId.ToString()));
            return userIdentity;
        }

       
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string DisplayName { get; set; }
        public int? HouseholdId { get; set; }
        public string InviteEmail { get; set; }
        

    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Household> Households { get; set; }
        public DbSet<PersonalAccount> PersonalAccounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<BudgetItem> BudgetItems { get; set; }
        public DbSet<Invite> Invites { get; set; }


        public int AddTransaction(int accountId, string description, decimal amount, bool trxType, bool isVoid, int categoryId, string userId, bool reconciled, decimal recBalance, bool isDeleted)
        {
            return Database.ExecuteSqlCommand("AddTransaction @accountId, @description, @amount, @type, @void, @categoryId, @enteredById, @reconciled, @reconciledAmount, @isDeleted",
                new SqlParameter("accountId", accountId),
                new SqlParameter("description", description),
                new SqlParameter("amount", amount),
                new SqlParameter("type", trxType),
                new SqlParameter("void", isVoid),
                new SqlParameter("categoryId", categoryId),
                new SqlParameter("enteredById", userId),
                new SqlParameter("reconciled", reconciled),
                new SqlParameter("reconciledAmount", recBalance),
                new SqlParameter("isDeleted", isDeleted),
                new SqlParameter("date", DateTimeOffset.Now));
        }

        public int AddAccount(int hhId, string name, decimal balance, decimal recbalance, string userId, bool isDeleted)
        {
            return Database.ExecuteSqlCommand("AddAccount @hhId, @name, @balance, @recbalance, @createdbyId, @isDeleted",
                new SqlParameter("hhId", hhId),
                new SqlParameter("name", name),
                new SqlParameter("balance", balance),
                new SqlParameter("recbalance", recbalance),
                new SqlParameter("createdbyId", userId),
                new SqlParameter("isDeleted", isDeleted));
        }

        public int AddBudget(int hhId, string name)
        {
            return Database.ExecuteSqlCommand("AddBudget @hhId, @name",
                new SqlParameter("hhId", hhId),
                new SqlParameter("name", name));
        }

        public int AddHousehold(int headofhouseholdId, string name)
        {
            return Database.ExecuteSqlCommand("AddHousehold @headofhouseholdId, @name",
                new SqlParameter("headofhouseholdId", headofhouseholdId),
                new SqlParameter("name", name));
        }



        public async Task<List<PersonalAccount>> GetAccounts(int hhId)
        {
            return await Database.SqlQuery<PersonalAccount>("GetAccountByHousehold @hhId",
                new SqlParameter("hhId", hhId)).ToListAsync();
        }

        public async Task<PersonalAccount> GetAccountDetails(int accountId, int hhId)
        {
            return await Database.SqlQuery<PersonalAccount>("GetAccountDetails @acctId, @hhId",
                new SqlParameter("acctId", accountId),
                new SqlParameter("hhId", hhId)).FirstOrDefaultAsync();
        }

        public async Task<Household> GetHousehold(int hhId)
        {
            return await Database.SqlQuery<Household>("GetHousehold @hhId", 
                new SqlParameter("hhId", hhId)).FirstOrDefaultAsync();
        }

        public async Task<List<Household>> GetAllHouseholds()
        {
            return await Database.SqlQuery<Household>("GetAllHouseholds").ToListAsync();
        }

        public async Task<List<Transaction>> GetTransactions(int accountId, int hhId)
        {
            return await Database.SqlQuery<Transaction>("GetTransactions @acctId, @hhId",
                new SqlParameter("acctId", accountId),
                new SqlParameter("hhId", hhId)).ToListAsync();
        }

    }
}