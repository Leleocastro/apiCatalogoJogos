using ApiCatalogoJogos.Entities;
using Google.Cloud.Firestore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ApiCatalogoJogos.Repositories
{
    public class OrderSqlServerRepository : IOrderRepository
    {
        private readonly SqlConnection sqlConnection;

        private static FirestoreDb DbConnection()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"catalogo-jogos.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            return FirestoreDb.Create("catalogo-jogos-82fa1"); 
        }

        public OrderSqlServerRepository(IConfiguration configuration)
        {
            sqlConnection = new SqlConnection(configuration.GetConnectionString("Default"));
        }

        public async Task<List<Order>> Obter(int pagina, int quantidade)
        {
            var orders = new List<Order>();

            CollectionReference usersRef = DbConnection().Collection("orders");
            QuerySnapshot snapshot = await usersRef.GetSnapshotAsync();

            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                Dictionary<string, object> documentDictionary = document.ToDictionary();

                var cartItemList = new List<CartItem>();

                foreach (Dictionary<string, object> cart in (documentDictionary["Jogos"] as List<dynamic>))
                {
                    cartItemList.Add(new CartItem 
                    {
                        Id = cart["Id"].ToString(),
                        ImageUrl = cart["ImageUrl"].ToString(),
                        Preco = Convert.ToDouble(cart["Preco"]),
                        Frete = Convert.ToDouble(cart["Frete"]),
                        JogoId = cart["JogoId"].ToString(),
                        Nome = cart["Nome"].ToString(),
                        Quantidade = Convert.ToInt32(cart["Quantidade"])
                    });
                }
                try
                {
                    orders.Add(new Order
                    {
                        Id = Guid.Parse(document.Id),
                        Date = (string)documentDictionary["Date"],
                        Total = Convert.ToDouble(documentDictionary["Total"]),
                        Jogos = cartItemList
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

            }

            return orders;
        }

        public async Task<Order> Obter(Guid id)
        {
            var cartItemList = new List<CartItem>();
            Order order = null;

            DocumentReference usersRef = DbConnection().Collection("orders").Document(id.ToString());
            DocumentSnapshot snapshot = await usersRef.GetSnapshotAsync();

            Dictionary<string, object> documentDictionary = snapshot.ToDictionary();
            foreach (Dictionary<string, object> cart in (documentDictionary["Jogos"] as List<dynamic>))
            {
                cartItemList.Add(new CartItem
                {
                    Id = cart["Id"].ToString(),
                    ImageUrl = cart["ImageUrl"].ToString(),
                    Frete = Convert.ToDouble(cart["Frete"]),
                    Preco = Convert.ToDouble(cart["Preco"]),
                    JogoId = cart["JogoId"].ToString(),
                    Nome = cart["Nome"].ToString(),
                    Quantidade = Convert.ToInt32(cart["Quantidade"]),
                });
            }

            order = new Order
            {
                Id = id,
                Total = (double)documentDictionary["Total"],
                Date = (string)documentDictionary["Date"],
                Jogos = cartItemList,
            };


            return order;
        }

        public async Task Inserir(Order order)
        {

            DocumentReference docRef = DbConnection().Collection("orders").Document(order.Id.ToString());

            List<Dictionary<string, object>> dicList = new List<Dictionary<string, object>>();
            foreach (CartItem cart in order.Jogos)
            {
                Dictionary<string, object> dicTemp = new Dictionary<string, object>
                {
                    { "Id", cart.Id },
                    { "ImageUrl", cart.ImageUrl },
                    { "JogoId", cart.JogoId },
                    { "Nome", cart.Nome },
                    { "Preco", cart.Preco },
                    { "Frete", cart.Frete },
                    { "Quantidade", cart.Quantidade }
                };
                dicList.Add(dicTemp);
            }
            Dictionary<string, object> docDictionary = new Dictionary<string, object>
            {
                { "Total", order.Total },
                { "Jogos", dicList },
                { "Date", order.Date }
            };
            await docRef.SetAsync(docDictionary);
        }

        public async Task Remover(Guid id)
        {
            DocumentReference docRef = DbConnection().Collection("orders").Document(id.ToString());
            await docRef.DeleteAsync();
        }

        public void Dispose()
        {
            sqlConnection?.Close();
            sqlConnection?.Dispose();
        }
    }
}
