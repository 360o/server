using NpgsqlTypes;

namespace _360o.Server.API.V1.Stores.Model
{
    public class Store : BaseEntity
    {
        private readonly List<string> _englishCategories;

        private readonly List<string> _frenchCategories;

        private Store()
        {
        }

        public Store(string userId, string displayName, string? englishShortDescription, string? englishLongDescription, ISet<string>? englishCategories, string? frenchShortDescription, string? frenchLongDescription, ISet<string>? frenchCategories, Place place)
        {
            UserId = userId;
            DisplayName = displayName;
            EnglishShortDescription = englishShortDescription ?? string.Empty;
            EnglishLongDescription = englishLongDescription ?? string.Empty;
            _englishCategories = englishCategories != null ? englishCategories.ToList() : new List<string>();
            EnglishCategoriesJoined = string.Join(" ", EnglishCategories);
            FrenchShortDescription = frenchShortDescription ?? string.Empty;
            FrenchLongDescription = frenchLongDescription ?? string.Empty;
            _frenchCategories = frenchCategories != null ? frenchCategories.ToList() : new List<string>();
            FrenchCategoriesJoined = string.Join(" ", FrenchCategories);
            Place = place;
        }

        public string UserId { get; private set; }

        public string DisplayName { get; private set; }

        public string EnglishShortDescription { get; private set; }

        public string EnglishLongDescription { get; private set; }

        public List<string> EnglishCategories => _englishCategories;

        public string EnglishCategoriesJoined { get; private set; }

        public NpgsqlTsVector EnglishSearchVector { get; private set; }

        public string FrenchShortDescription { get; private set; }

        public string FrenchLongDescription { get; private set; }

        public List<string> FrenchCategories => _frenchCategories;

        public string FrenchCategoriesJoined { get; private set; }

        public NpgsqlTsVector FrenchSearchVector { get; private set; }

        public Place Place { get; private set; }
    }
}

