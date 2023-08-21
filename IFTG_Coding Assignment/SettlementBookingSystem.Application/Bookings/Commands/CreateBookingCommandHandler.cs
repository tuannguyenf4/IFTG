using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SettlementBookingSystem.Application.Bookings.Dtos;

namespace SettlementBookingSystem.Application.Bookings.Commands
{
    public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, BookingDto>
    {
        public List<CreateBookingCommand> bookings ;
        public CreateBookingCommandHandler()
        {
            bookings = new List<CreateBookingCommand>();
        }

        public Task<BookingDto> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            // Check whether booking time are conflicted
            var filePath = @"..\\SettlementBookingSystem\\ProblemDetails\\Data.txt";
            String[] lines = File.ReadAllLines(filePath);
            var isConflict = lines.Any(x => x == request.BookingTime);
            if (isConflict)
            {
                throw new NotImplementedException("Booking time are conflicted");
            }
            TimeSpan startTime = new TimeSpan(0, 0, 0);
            TimeSpan endTime = new TimeSpan(23, 59, 0);
            var time = request.BookingTime.Split(":");
            TimeSpan bookingTime = new TimeSpan(Convert.ToInt32(time[0]), Convert.ToInt32(time[1]), 0);
            if (bookingTime.Days !> 0)
            {
                throw new NotImplementedException("Booking time should be a 24-hour time");
            }

            bookings.Add(new CreateBookingCommand
            {
                Name = request.Name,
                BookingTime = request.BookingTime
            });

            // Write booking time to file
            using (StreamWriter w = File.AppendText(filePath))
            {
                w.WriteLine(request.BookingTime);

            }
            return Task.FromResult(new BookingDto()
            {
                BookingTime = request.BookingTime
            });
        }
    }
}
