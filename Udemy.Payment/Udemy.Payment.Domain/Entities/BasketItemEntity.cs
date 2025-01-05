using Iyzipay.Model;
using Udemy.Common.Base;

namespace Udemy.Payment.Domain.Entities;

public class BasketItemEntity : BaseEntity
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required string Category { get; set; }
    public required string ItemType { get; set; }
    public required decimal Price { get; set; }
    public required string BasketId { get; set; }

    public static BasketItemEntity MapToBasketItemEntity(BasketItem basketItem, string basketId)
    {
        return new BasketItemEntity
        {
            Id = basketItem.Id,
            Name = basketItem.Name,
            Category = $"{basketItem.Category1} > {basketItem.Category2}",
            ItemType = basketItem.ItemType,
            Price = decimal.Parse(basketItem.Price),
            BasketId = basketId
        };
    }

    public static BasketItem MapToBasketItem(BasketItemEntity basketItemEntity)
    {
        return new BasketItem
        {
            Id = basketItemEntity.Id,
            Name = basketItemEntity.Name,
            Price = basketItemEntity.Price.ToString("F2"),
            Category1 = basketItemEntity.Category.Split(" > ")[0],
            Category2 = basketItemEntity.Category.Split(" > ")[1],
            ItemType = basketItemEntity.ItemType
        };
    }
}