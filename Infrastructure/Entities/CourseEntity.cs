﻿namespace Infrastructure.Entities;

public class CourseEntity
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public decimal? Price { get; set; }
    public decimal? DiscountPrice { get; set; }
    public int? Hours { get; set; }
    public bool IsBestseller { get; set; }
    public decimal? LikesInNumbers { get; set; }
    public decimal? LikesInPercent { get; set; }
    public string? Author { get; set; }
    public string? CourseImageUrl { get; set; }
    public virtual IEnumerable<SavedCourseEntity> SavedCourses { get; set; } = new List<SavedCourseEntity>();
    public virtual IEnumerable<MyCourseEntity> MyCourses { get; set; } = new List<MyCourseEntity>();
}