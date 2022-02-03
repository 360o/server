using _360o.Server.API.V1.Stores.Model;
using NpgsqlTypes;

namespace _360o.Server.API.V1.Organizations.Model
{
    public class Organization : BaseEntity
    {
        private Organization()
        {
        }

        public Organization(string userId, string name, string? englishShortDescription, string? englishLongDescription, string? frenchShortDescription, string? frenchLongDescription)
        {
            UserId = userId;
            Name = name;
            EnglishShortDescription = englishShortDescription == null ? string.Empty : englishShortDescription.Trim();
            EnglishLongDescription = englishLongDescription == null ? string.Empty : englishLongDescription.Trim();
            FrenchShortDescription = frenchShortDescription == null ? string.Empty : frenchShortDescription.Trim();
            FrenchLongDescription = frenchLongDescription == null ? string.Empty : frenchLongDescription.Trim();
        }

        public string UserId { get; private set; }

        public string Name { get; private set; }

        public string EnglishShortDescription { get; private set; }
        public string EnglishLongDescription { get; private set; }
        public List<string> EnglishCategories { get; private set; } = new List<string>();
        public string EnglishCategoriesJoined { get; private set; } = string.Empty;
        public NpgsqlTsVector EnglishSearchVector { get; private set; }

        public string FrenchShortDescription { get; private set; }
        public string FrenchLongDescription { get; private set; }
        public List<string> FrenchCategories { get; private set; } = new List<string>();
        public string FrenchCategoriesJoined { get; private set; } = string.Empty;
        public NpgsqlTsVector FrenchSearchVector { get; private set; }

        public List<Store> Stores { get; private set; }

        public void SetName(string name)
        {
            Name = name;
        }

        public void SetEnglishShortDescription(string englishShortDescription)
        {
            EnglishShortDescription = englishShortDescription;
        }

        public void SetEnglishLongDescription(string englishLongDescription)
        {
            EnglishLongDescription = englishLongDescription;
        }

        public void SetEnglishCategories(ISet<string> categories)
        {
            EnglishCategories = categories.ToList();
            EnglishCategoriesJoined = JoinCategories(EnglishCategories);
        }

        public void SetFrenchShortDescription(string frenchShortDescription)
        {
            FrenchShortDescription = frenchShortDescription;
        }

        public void SetFrenchLongDescription(string frenchLongDescription)
        {
            FrenchLongDescription = frenchLongDescription;
        }

        public void SetFrenchCategories(ISet<string> categories)
        {
            FrenchCategories = categories.ToList();
            FrenchCategoriesJoined = JoinCategories(FrenchCategories);
        }

        private string JoinCategories(IReadOnlyList<string> categories) => string.Join(" ", categories);
    }
}
