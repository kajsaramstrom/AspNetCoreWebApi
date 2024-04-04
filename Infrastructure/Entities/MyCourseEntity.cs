namespace Infrastructure.Entities
{
    public class MyCourseEntity
    {
        public int UserId { get; set; }
        public virtual UserEntity? User { get; set; }

        public int CourseId { get; set; }
        public virtual CourseEntity? Course { get; set; }
    }
}
