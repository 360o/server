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

        public void AddEnglishCategory(string category)
        {
            if (!EnglishCategories.Contains(category))
            {
                EnglishCategories.Add(category);
                EnglishCategoriesJoined = JoinCategories(EnglishCategories);
            }
        }

        public void AddFrenchCategory(string category)
        {
            if (!FrenchCategories.Contains(category))
            {
                FrenchCategories.Add(category);
                FrenchCategoriesJoined = JoinCategories(FrenchCategories);
            }
        }

        private string JoinCategories(IReadOnlyList<string> categories) => string.Join(" ", categories);
    }
}
