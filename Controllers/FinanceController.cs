using NewFinancialAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace NewFinancialAPI.Controllers
{
    [RoutePrefix("api/Finance")]
    public class FinanceController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //======================================================================================//
        //-- INSERTS --//

        /// <summary>
        /// Insert a Transaction
        /// </summary>
        /// <param name="accountId">The PersonalAccounts FK</param>
        /// <param name="description">Description of Transaction</param>
        /// <param name="amount">Amount of Transaction</param>
        /// <param name="trxType">Type of Transaction</param>
        /// <param name="isVoid">?</param>
        /// <param name="categoryId">Category of Transaction</param>
        /// <param name="userId">Transaction Creator UserId</param>
        /// <param name="reconciled">Reconciled Y or N</param>
        /// <param name="recBalance">The Reconciled Balance Amount</param>
        /// <param name="isDeleted">Archived Y or N</param>
        /// <returns></returns>
        [Route("AddTransaction")]
        [AcceptVerbs("GET", "POST")]
        public IHttpActionResult AddTransaction(int accountId, string description, decimal amount, bool trxType, bool isVoid, int categoryId, string userId, bool reconciled, decimal recBalance, bool isDeleted)
        {
            return Ok(db.AddTransaction(accountId, description, amount, trxType, isVoid, categoryId, userId, reconciled, recBalance, isDeleted));

        }

        /// <summary>
        /// Insert a PersonalAccount
        /// </summary>
        /// <param name="hhId">Household FK</param>
        /// <param name="name">Account Name</param>
        /// <param name="balance">Account Balance</param>
        /// <param name="recbalance">Account Reconciled Balance Amount</param>
        /// <param name="userId">Account Creator UserId</param>
        /// <param name="isDeleted">Deleted Y or N</param>
        /// <returns></returns>
        [Route("AddAccount")]
        [AcceptVerbs("GET", "POST")]
        public IHttpActionResult AddAccount(int hhId, string name, decimal balance, decimal recbalance, string userId, bool isDeleted)
        {
            return Ok(db.AddAccount(hhId, name, balance, recbalance, userId, isDeleted));

        }

        /// <summary>
        /// Insert a Budget
        /// </summary>
        /// <param name="hhId">Household FK</param>
        /// <param name="name">Budget Name</param>
        /// <returns></returns>
        [Route("AddBudget")]
        [AcceptVerbs("GET", "POST")]
        public IHttpActionResult AddBudget(int hhId, string name)
        {
            return Ok(db.AddBudget(hhId, name));

        }

        /// <summary>
        /// Insert a Household
        /// </summary>
        /// <param name="headofhouseholdId">Head of Household UserId</param>
        /// <param name="name">Household Name</param>
        /// <returns></returns>
        [Route("AddHousehold")]
        [AcceptVerbs("GET", "POST")]
        public IHttpActionResult AddHousehold(int headofhouseholdId, string name)
        {
            return Ok(db.AddHousehold(headofhouseholdId, name));

        }


        //-- SELECTS --//

        /// <summary>
        ///  Select Accounts by Household Id, Return XML
        /// </summary>
        /// <param name="householdId">The Household Foreign Key</param>
        /// <returns></returns>

        [Route("Accounts")]
        public async Task<List<PersonalAccount>> GetAccounts(int householdId)
        {
            return await db.GetAccounts(householdId);
        }


        /// <summary>
        /// Select Accounts by Household Id, Return Json
        /// </summary>
        /// <param name="householdId">The Household Foreign Key</param>
        /// <returns></returns>
        [Route("Accounts/json")]
        public async Task<IHttpActionResult> GetAccountsJson(int householdId)
        {
            var json = JsonConvert.SerializeObject(await db.GetAccounts(householdId));
            return Ok(json);
        }


        /// <summary>
        /// Select an Account Details
        /// </summary>
        /// <param name="accountId">PersonalAccount PK</param>
        /// <param name="hhId">Household FPK</param>
        /// <returns></returns>
        [Route("AccountDetails")]
        public async Task<PersonalAccount> GetAccountDetails(int accountId, int hhId)
        {
            return await db.GetAccountDetails(accountId, hhId);
        }

        /// <summary>
        /// Select an Account Details and Return Json
        /// </summary>
        /// <param name="accountId">PersonalAccount PK</param>
        /// <param name="hhId">HH FK</param>
        /// <returns></returns>
        [Route("AccountDetails/json")]
        public async Task<IHttpActionResult> GetAccountDetailsJson(int accountId, int hhId)
        {
            var json = JsonConvert.SerializeObject(await db.GetAccountDetails(accountId, hhId));
            return Ok(json);
        }


        /// <summary>
        /// Select a Household
        /// </summary>
        /// <param name="hhId">HH PK</param>
        /// <returns></returns>
        [Route("Household")]
        public async Task<Household> GetHousehold(int hhId)
        {
            return await db.GetHousehold(hhId);
        }

        /// <summary>
        /// Select a Household, Return Json
        /// </summary>
        /// <param name="hhId">HH PK</param>
        /// <returns></returns>
        [Route("Household/json")]
        public async Task<IHttpActionResult> GetHouseholdJson(int hhId)
        {
            var json = JsonConvert.SerializeObject(await db.GetHousehold(hhId));
            return Ok(json);
        }



        /// <summary>
        /// Select All Households
        /// </summary>
        /// <returns>int</returns>
        [Route("Households")]
        public async Task<List<Household>> GetAllHouseholds()
        {
            return await db.GetAllHouseholds();
        }

        /// <summary>
        /// Select All Households, Return Json
        /// </summary>
        /// <returns>int</returns>
        [Route("Households/json")]
        public async Task<IHttpActionResult> GetAllHouseholdsJson()
        {
            var json = JsonConvert.SerializeObject(await db.GetAllHouseholds());
            return Ok(json);
        }



        /// <summary>
        /// Select Transactions by Account and Household
        /// </summary>
        /// <param name="accountId">PersonalAccount FK</param>
        /// <param name="hhId">Household FK</param>
        /// <returns></returns>
        [Route("Transactions")]
        public async Task<List<Transaction>> GetTransactions(int accountId, int hhId)
        {
            return await db.GetTransactions(accountId, hhId);
        }

        /// <summary>
        /// Select Transactions by Account and Household, Return Json
        /// </summary>
        /// <param name="accountId">Personal Account FK</param>
        /// <param name="hhId">HH FK</param>
        /// <returns></returns>
        [Route("Transactions/json")]
        public async Task<IHttpActionResult> GetTransactionsJson(int accountId, int hhId)
        {
            var json = JsonConvert.SerializeObject(await db.GetTransactions(accountId, hhId));
            return Ok(json);
        }


        /// <summary>
        /// Select Balance from Personal Account
        /// </summary>
        /// <param name="hhId">HH FK</param>
        /// <param name="id">Personal Accounts PK</param>
        /// <returns></returns>
        [Route("AccountBalance")]
        public async Task<PersonalAccount> GetAccountBalance(int hhId, int id)
        {
            return await db.GetAccountBalance(hhId, id);
        }

        /// <summary>
        /// Select Account Balance by PK and FK
        /// </summary>
        /// <param name="hhId">HH FK</param>
        /// <param name="id">PersonalAccount PK</param>
        /// <returns></returns>
        [Route("AccountBalance/json")]
        public async Task<IHttpActionResult> GetAccountBalanceJson(int hhId, int id)
        {
            var json = JsonConvert.SerializeObject(await db.GetAccountBalance(hhId, id));
            return Ok(json);
        }

        /// <summary>
        /// Select Balance on Budget
        /// </summary>
        /// <param name="hhId">HH FK</param>
        /// <returns></returns>
        [Route("BudgetBalance")]
        public async Task<Budget> GetBudgetBalance(int hhId)
        {
            return await db.GetBudgetBalance(hhId);
        }






    }
}
