using BLL.Interfaces;
using DAL.Context;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CloudinaryDotNet;
using BLL.Helpers;

namespace BLL.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly MvcAppDbContext _context;

        public PaymentRepository(MvcAppDbContext context) 
        {
            _context = context;
        }
        public async Task<Payment> AddPaymentAsync(Payment payment)
        {
            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        public Task<Payment> CancelPaymentAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task DeletePaymentAsync(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if(payment != null)
            {
                _context.Payments.Remove(payment);
                await _context.SaveChangesAsync();
            }

        }

        public async Task<IEnumerable<Payment>> GetAllPaymentsAsync()
        {
            return await _context.Payments
                            .Include(p => p.Booking) // Include the navigation property 'Booking'
                            .ToListAsync();
        }

        public async Task<Payment> GetPaymentByBookingId(int BookingId)
        {
            return await _context.Payments.FirstOrDefaultAsync(p => p.BookingID == BookingId);
        }

        public async Task<Payment> GetPaymentByIdAsync(int id)
        {
            return await _context.Payments.FirstOrDefaultAsync(p => p.PaymentID == id);
        }

        public Task<IEnumerable<Payment>> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByStatusAsync(string status)
        {
            return await _context.Payments.Where(p => p.PaymentStatus == status).ToListAsync();
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


    public async Task<Payment> UpdatePaymentAsync(Payment payment)
        {
            var existPayment = await _context.Payments.FindAsync(payment.PaymentID);
            if (existPayment != null)
            {
                existPayment.Amount = payment.Amount;
                existPayment.PaymentDate = payment.PaymentDate;
                existPayment.PaymentMethod = payment.PaymentMethod;

                _context.Payments.Update(existPayment);
                await _context.SaveChangesAsync();

            }
            return existPayment;
        }

        public async Task<List<decimal>> GetWeeklyRevenue()
        {
            var currentDate = DateTime.Now;
            var startOfCurrentWeek = DateHelper.StartOfWeek(currentDate);
            var revenueCounts = new List<decimal>();

            for (int i = 0; i < 4; i++)
            {
                var startOfWeek = startOfCurrentWeek.AddDays(i * 7);
                var endOfWeek = startOfWeek.AddDays(7);

                var revenue = await _context.Payments
                    .Where(p => p.Booking.StartDate >= startOfWeek && p.Booking.StartDate < endOfWeek)
                    .SumAsync(p => p.Amount); // Assuming Amount is the revenue field

                revenueCounts.Add(revenue);
            }
            return revenueCounts;
        }

        public async Task<List<decimal>> GetDailyRevenue()
        {
            var currentDate = DateTime.Now;
            var dailyRevenueCounts = new List<decimal>();

            // Loop through the next 7 days
            for (int i = 0; i < 7; i++)
            {
                var currentDay = currentDate.AddDays(i);
                var revenue = await _context.Payments
                    .Where(p => p.Booking.StartDate.Date == currentDay.Date) // Match the date exactly
                    .SumAsync(p => p.Amount); // Assuming Amount is the revenue field

                dailyRevenueCounts.Add(revenue);
            }

            return dailyRevenueCounts;
        }

    }
}
