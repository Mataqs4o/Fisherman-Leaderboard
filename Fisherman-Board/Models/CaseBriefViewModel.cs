using System.Collections.Generic;

namespace Fisherman_Board.Models
{
    public class CaseBriefViewModel
    {
        public string PageTitle { get; set; } = string.Empty;

        public string Eyebrow { get; set; } = string.Empty;

        public string Heading { get; set; } = string.Empty;

        public string Subtitle { get; set; } = string.Empty;

        public List<CaseSectionViewModel> Sections { get; set; } = new();
    }

    public class CaseSectionViewModel
    {
        public string Title { get; set; } = string.Empty;

        public List<string> Paragraphs { get; set; } = new();

        public List<string> BulletPoints { get; set; } = new();
    }
}
