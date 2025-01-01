using Udemy.Common.Base;

namespace Udemy.Course.Domain.Entities;

public class CourseDetails : BaseEntity
{
    public string Description { get; set; } = "";
    public string? ImageUrl { get; set; }
    public decimal Price { get; set; }
    public decimal? DiscountedPrice { get; set; }
    public DateTimeOffset? DiscountStartDate { get; set; }
    public DateTimeOffset? DiscountEndDate { get; set; }
    public string? PreviewVideoUrl { get; set; }
    public int RateCount { get; set; }
    public decimal RateValue { get; set; }
    public decimal AverageRate => RateCount == 0 ? 0 : Math.Round(RateValue / RateCount, 2);
    public int ViewCount { get; set; }
    public TimeSpan TotalDuration { get; set; }
    public Guid CourseId { get; set; }

    public virtual Course? Course { get; set; }

    public void AddRating(int ratingValue)
    {
        RateCount++;
        RateValue += ratingValue;
    }

    public void RemoveRating(int ratingValue)
    {
        RateCount--;
        RateValue -= ratingValue;
    }
}