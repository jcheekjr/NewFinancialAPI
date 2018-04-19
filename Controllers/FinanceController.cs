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

        /// <summary>
        ///  Get Account by Household Id
        /// </summary>
        /// <param name="householdId">The Household Primary Key</param>
        /// <returns></returns>

        [Route("Accounts")]
        public async Task<List<PersonalAccount>> GetAccounts(int householdId)
        {
            return await db.GetAccounts(householdId);
        }


        /// <summary>
        /// Get Account by Household Id and Return Json
        /// </summary>
        /// <param name="householdId">The Household Primary Key</param>
        /// <returns></returns>
        [Route("Accounts/json")]
        public async Task<IHttpActionResult> GetAccountsJson(int householdId)
        {
            var json = JsonConvert.SerializeObject(await db.GetAccounts(householdId));
            return Ok(json);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="hhId"></param>
        /// <returns></returns>
        [Route("Transactions")]
        public async Task<List<Transaction>> GetTransactions( int accountId, int hhId)
        {
            return await db.GetTransactions(accountId, hhId);
        }


        [Route("AddTransaction")]
        [AcceptVerbs("GET", "POST")]
        public IHttpActionResult AddTransaction(int accountId, string description, decimal amount, bool trxType, bool isVoid, int categoryId, string userId, bool reconciled, decimal recBalance, bool isDeleted)
        {
            return Ok(db.AddTransaction(accountId, description, amount, trxType, isVoid, categoryId, userId, reconciled, recBalance, isDeleted));

        }

    }
}
