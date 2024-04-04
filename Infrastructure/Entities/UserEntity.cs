using System.Text.Json.Serialization;

namespace Infrastructure.Entities;

public class UserEntity
{
    public int Id { get; set; }
    public string Email { get; set; } = null!;

    [JsonIgnore]
    public virtual IEnumerable<SavedCourseEntity> SavedCourses { get; set; } = new List<SavedCourseEntity>();

    [JsonIgnore]
    public virtual IEnumerable<MyCourseEntity> MyCourses { get; set; } = new List<MyCourseEntity>();
}
