using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi_Test.Models
{
    public static class ArrayExtensions
    {
        public static bool IsNotOrEmpty(this IEnumerable<Product> products)
        {
            return products == null || !products.Any();
        }
    }
}
