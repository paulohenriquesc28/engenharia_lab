using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace historicoNamespace.Tests
{
    [TestClass]
    public class HistoricoTests
    {
        [TestMethod]
        public void TestStatusCalculation()
        {
            // Arrange
            var medicoes = new List<TempMotoWeb.Models.Medicao>
            {
                new TempMotoWeb.Models.Medicao { Id = 1, Ph = 7, Temperatura = 25, Data_Medicao = DateTime.Now },
                new TempMotoWeb.Models.Medicao { Id = 2, Ph = 6, Temperatura = 30, Data_Medicao = DateTime.Now },
                // Adicione mais medições simuladas conforme necessário
            };

            // Act
            var statusResults = new Dictionary<int, string>();
            foreach (var medicao in medicoes)
            {
                string status;
                if (((medicao.Ph >= 1 && medicao.Ph <= 14 && medicao.Ph != 7) || (medicao.Temperatura > 35 || medicao.Temperatura < 5)))
                {
                    status = "Limpeza Recomendada";
                }
                else
                {
                    status = "Água Boa para Consumo";
                }
                statusResults.Add(medicao.Id, status);
            }

            // Assert
            Assert.AreEqual("Água Boa para Consumo", statusResults[1]);
            Assert.AreEqual("Limpeza Recomendada", statusResults[2]);
            // Adicione mais verificações para outras medições, se necessário
        }
    }
}
