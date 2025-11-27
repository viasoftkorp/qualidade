using System;
using Viasoft.Core.ServiceBus.Abstractions;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalContracts.LogisticsProducts.Produtos.Models;

namespace Viasoft.Qualidade.RNC.Core.Domain.ExternalContracts.LogisticsProducts.Produtos
{
    [Endpoint("Viasoft.Logistics.Products.ProductUpdated")]

    public class ProductUpdated : IMessage
    {
        public Guid Id { get; set; }
        public Guid EnvironmentId { get; set; }
        public DateTime AsOfDate { get; set; }
        public ProdutoModel Product { get; set; }
        public ProductUpdated(DateTime dateTime, Guid environmentId)
        {
            Id = Guid.NewGuid();
            AsOfDate = dateTime;
            EnvironmentId = environmentId;
        }

        public ProductUpdated()
        {
            
        }
        
    }
}