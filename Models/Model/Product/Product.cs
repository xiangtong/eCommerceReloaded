using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCommerceReloaded.Models 
{
    public class Product:BaseEntity 
    {
        public Product()
        {
            listOfProductInCart = new List<ProductInCart>();
            listOfProductInWishlist = new List<ProductInWishlist>();
            listOfProductInOrder = new List<ProductInOrder>();
            listOfProductEvent = new List<ProductEvent>();
        }
        [Key]
        public int productId{get;set;}
        public string name{get;set;}
        public string description {get;set;}
        public byte[] image{get;set;}
        public string imageUrl {get;set;}
        public int inventory{get;set;}
        public int cost{get;set;}
        public int price{get;set;}
         public int featured {get;set;}
        public DateTime created_At{get;set;}
       public List<ProductInCart> listOfProductInCart{get;set;}
       public List<ProductInWishlist> listOfProductInWishlist{get;set;}
       public List<ProductInOrder> listOfProductInOrder{get;set;}
       public List<ProductEvent> listOfProductEvent{get;set;}
       public int categoryId{get;set;}
       public Category category{get;set;}

    }
}