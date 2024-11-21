using System.Runtime.InteropServices.JavaScript;

namespace CleanArchitectureDDD.Domain.Rentals;

public sealed record DateRange
{
    private DateRange()
    {
        
    }
    
    public DateOnly StartDate { get; init; }
    public DateOnly EndDate { get; init; }
    public int TotalDays => (EndDate.DayNumber - StartDate.DayNumber);
    
    public static DateRange Create(DateOnly startDate, DateOnly endDate)
    {
        if (startDate > endDate)
        {
            throw new ApplicationException("Start date cannot be greater than end date");
        }
        return new DateRange
        {
            StartDate = startDate,
            EndDate = endDate
        };
    }
}