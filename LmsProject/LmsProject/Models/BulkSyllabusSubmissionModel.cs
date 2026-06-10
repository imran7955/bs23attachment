using System.Collections.Generic;

namespace LmsProject.Models
{
    public class BulkSyllabusSubmissionModel
    {
        public int CourseId { get; set; }
        public List<SyllabusItemStagingDto> Materials { get; set; } = new List<SyllabusItemStagingDto>();
    }

    public class SyllabusItemStagingDto
    {
        public string Title { get; set; } = string.Empty;
        public string YouTubeLink { get; set; } = string.Empty;
        public int Position { get; set; }
    }
}