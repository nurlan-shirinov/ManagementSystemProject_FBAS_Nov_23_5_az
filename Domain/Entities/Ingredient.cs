using Domain.BaseEntities;

namespace Domain.Entities;

public class Ingredient : ProductBaseEntity
{
    public int MinimumCount { get; set; }
    public int MaksimumCount { get; set; }
    public int FreeIngredientCount { get; set; }
    public List<int> DepartmentsId { get; set; }
}
