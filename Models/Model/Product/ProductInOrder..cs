using System;
using System.ComponentModel.DataAnnotations;

namespace eCommerceReloaded.Models 
{
    public class ProductInOrder:BaseEntity 
    {
        [Key]
        public int productInOrderId{get;set;}
        public int orderId {get;set;}
        public Order order {get;set;}
        public int productId {get;set;}
        public Product product {get;set;}
        public int quantity {get;set;}
        public DateTime created_At{get;set;}

    }
}