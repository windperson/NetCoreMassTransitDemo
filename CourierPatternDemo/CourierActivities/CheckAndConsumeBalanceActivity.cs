using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourierPatternDemo.AccountData;
using CourierPatternDemo.MsgContracts;
using MassTransit.Courier;
using Microsoft.Extensions.Logging;

namespace CourierPatternDemo.CourierActivities
{
    public class CheckAndConsumeBalanceActivity : Activity<ICheckThenWithdrawlBalanceArguemnt, ICheckThenWithdrawlBalanceLog>
    {
        private readonly AccountRepo _accountRepo;
        private readonly ILogger<CheckAndConsumeBalanceActivity> _logger;

        public CheckAndConsumeBalanceActivity(AccountRepo accountRepo, ILogger<CheckAndConsumeBalanceActivity> logger)
        {
            _accountRepo = accountRepo;
            _logger = logger;
        }

        public Task<ExecutionResult> Execute(ExecuteContext<ICheckThenWithdrawlBalanceArguemnt> context)
        {
            var args = context.Arguments;
            _logger.LogInformation("{@1}: doing {@2} with data= {@3}", context.ExecutionId, context.ActivityName, args);

            var account = GetAccountFromRepo(_accountRepo, args.CustomerId);
            if(account == null)
            {
                return Task.FromResult(context.Terminate(new { Reason = $"No such Account: {args.CustomerId}" }));
            }

            if(account.Balance - args.CostAmount < 0)
            {
                return Task.FromResult(context.Terminate(new { Reason = $"Not enough money for Account: {args.CustomerId} to purchase ${args.CostAmount} product in Order: {args.PurchaseTransactionId}" }));
            }

            account.Balance -= args.CostAmount;

            return Task.FromResult(context.Completed<ICheckThenWithdrawlBalanceLog>(new { PurchaseTransactionId = args.PurchaseTransactionId, IsSuccess = true, WithdrawlContext = args}));
        }

        public Task<CompensationResult> Compensate(CompensateContext<ICheckThenWithdrawlBalanceLog> context)
        {
            var withDrawlLog = context.Log;
            _logger.LogWarning("{@1}: error={@2}, data= {@3}", context.ExecutionId, withDrawlLog.FailedReason, withDrawlLog.WithdrawlContext);

            // if already withdraw the cost from account, compensate the amount back to the account.
            if (withDrawlLog.IsSuccess)
            {
                var account = GetAccountFromRepo(_accountRepo, withDrawlLog.WithdrawlContext.CustomerId);

                if (account != null)
                {
                    _logger.LogInformation("{@1}: compensate ${@2} to Account: {@3}", context.ExecutionId, withDrawlLog.WithdrawlContext.CostAmount, account.AccountId);
                    account.Balance += withDrawlLog.WithdrawlContext.CostAmount;
                }
            }
                       
            return Task.FromResult(context.Compensated());
        }

        private static Account GetAccountFromRepo(AccountRepo repo, string accountId)
        {
            return repo.Data.Where(x => x.AccountId == accountId).FirstOrDefault();
        }
    }
}
