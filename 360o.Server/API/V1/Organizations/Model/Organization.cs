using _360o.Server.Api.V1.Stores.Model;
using Ardalis.GuardClauses;
using NpgsqlTypes;

namespace _360o.Server.Api.V1.Organizations.Model
{
    public class Organization : BaseEntity
    {
        private string _name;
        private string? _englishShortDescription;
        private string? _englishLongDescription;
        private List<string>? _englishCategories;
        private List<string>? _frenchCategories;
        private string? _frenchShortDescription;
        private string? _frenchLongDescription;

        public Organization(string userId, string name)
        {
            UserId = Guard.Against.NullOrWhiteSpace(userId, nameof(userId));
            Name = name;
        }

        private Organization()
        {
        }

        public string UserId { get; private set; }

        public string Name
        {
            get => _name;
            set
            {
                _name = Guard.Against.NullOrWhiteSpace(value, nameof(Name));
            }
        }

        public string? EnglishShortDescription
        {
            get => _englishShortDescription;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _englishShortDescription = null;
                }
                else
                {
                    _englishShortDescription = value.Trim();
                }
            }
        }

        public string? EnglishLongDescription
        {
            get => _englishLongDescription;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _englishLongDescription = null;
                }
                else
                {
                    _englishLongDescription = value.Trim();
                }
            }
        }

        public List<string>? EnglishCategories
        {
            get => _englishCategories?.ToList();
            set
            {
                if (value == null || value.Count == 0)
                {
                    _englishCategories = null;
                }
                else
                {
                    _englishCategories = value.ToHashSet()
                        .Where(c => !string.IsNullOrWhiteSpace(c))
                        .Select(c => c.Trim())
                        .ToList();
                }

                EnglishCategoriesJoined = JoinCategories(_englishCategories);
            }
        }

        public string EnglishCategoriesJoined { get; private set; } = string.Empty;

        public NpgsqlTsVector EnglishSearchVector { get; private set; }

        public string? FrenchShortDescription
        {
            get => _frenchShortDescription;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _frenchShortDescription = null;
                }
                else
                {
                    _frenchShortDescription = value.Trim();
                }
            }
        }

        public string? FrenchLongDescription
        {
            get => _frenchLongDescription;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _frenchLongDescription = null;
                }
                else
                {
                    _frenchLongDescription = value.Trim();
                }
            }
        }

        public List<string>? FrenchCategories
        {
            get => _frenchCategories?.ToList();
            set
            {
                if (value == null || value.Count == 0)
                {
                    _frenchCategories = null;
                }
                else
                {
                    _frenchCategories = value.ToHashSet()
                        .Where(c => !string.IsNullOrWhiteSpace(c))
                        .Select(c => c.Trim())
                        .ToList();
                }

                FrenchCategoriesJoined = JoinCategories(_frenchCategories);
            }
        }

        public string FrenchCategoriesJoined { get; private set; } = string.Empty;

        public NpgsqlTsVector FrenchSearchVector { get; private set; }

        public List<Store> Stores { get; private set; }

        private string JoinCategories(IEnumerable<string>? categories) => categories == null ? string.Empty : string.Join(" ", categories);
    }
}