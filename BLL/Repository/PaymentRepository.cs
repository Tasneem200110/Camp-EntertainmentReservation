using BLL.Interfaces;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Repository
{
    internal class PaymentRepository : IPaymentRepository
    {
        public Task<Payment> AddPaymentAsync(Payment payment)
        {
            throw new NotImplementedException();
        }

        public Task<Payment> CancelPaymentAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task DeletePaymentAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Payment>> GetAllPaymentsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Payment> GetPaymentByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Payment>> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Payment>> GetPaymentsByStatusAsync(string status)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Payment>> GetPaymentsByUserAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetPaymentStatusAsync(int paymentId)
        {
            throw new NotImplementedException();
        }

        public Task<decimal> GetTotalPaymentAmountForUserAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<Payment> ProcessRefundAsync(int paymentId)
        {
            throw new NotImplementedException();
        }

        public Task<Payment> UpdatePaymentAsync(Payment payment)
        {
            throw new NotImplementedException();
        }
    }
}
