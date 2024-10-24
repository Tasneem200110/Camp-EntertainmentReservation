using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IPaymentRepository
    {

        Task<Payment> AddPaymentAsync(Payment payment);
        Task<Payment> GetPaymentByIdAsync(int id);
        Task<IEnumerable<Payment>> GetAllPaymentsAsync();
        Task<IEnumerable<Payment>> GetPaymentsByUserAsync(int userId);
        Task<Payment> UpdatePaymentAsync(Payment payment);
        Task DeletePaymentAsync(int id);
        Task<IEnumerable<Payment>> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Payment>> GetPaymentsByStatusAsync(string status);
        Task<string> GetPaymentStatusAsync(int paymentId);
        Task<Payment> ProcessRefundAsync(int paymentId);
        Task<decimal> GetTotalPaymentAmountForUserAsync(int userId);
        Task<Payment> CancelPaymentAsync(int id);
        Task<Payment> GetPaymentByBookingId(int BookingId);

    }
}
