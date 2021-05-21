using ApiCatalogoJogos.Entities;
using Google.Cloud.Firestore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ApiCatalogoJogos.Repositories
{
    public class JogoSqlServerRepository : IJogoRepository
    {
        private readonly SqlConnection sqlConnection;

        private static FirestoreDb DbConnection()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"catalogo-jogos.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            return FirestoreDb.Create("catalogo-jogos-82fa1"); 
        }

        public JogoSqlServerRepository(IConfiguration configuration)
        {
            sqlConnection = new SqlConnection(configuration.GetConnectionString("Default"));
        }

        public async Task<List<Jogo>> Obter(int pagina, int quantidade)
        {
            var jogos = new List<Jogo>();

            CollectionReference usersRef = DbConnection().Collection("jogos");
            QuerySnapshot snapshot = await usersRef.GetSnapshotAsync();

            foreach(DocumentSnapshot document in snapshot.Documents)
            {
                Dictionary<string, object> documentDictionary = document.ToDictionary();
                jogos.Add(new Jogo
                {
                    Id = Guid.Parse(document.Id),
                    Nome = (string)documentDictionary["Nome"],
                    ImageUrl = (string)documentDictionary["ImageUrl"],
                    Preco = (double)Convert.ToDouble(documentDictionary["Preco"])
                });
            }

            return jogos;
        }

        public async Task<Jogo> Obter(Guid id)
        {
            Jogo jogo = null;

            DocumentReference usersRef = DbConnection().Collection("jogos").Document(id.ToString());
            DocumentSnapshot snapshot = await usersRef.GetSnapshotAsync();

            Dictionary<string, object> documentDictionary = snapshot.ToDictionary();

            jogo = new Jogo
            {
                Id = id,
                Nome = (string)documentDictionary["Nome"],
                ImageUrl = (string)documentDictionary["ImageUrl"],
                Preco = Convert.ToDouble(documentDictionary["Preco"])
            };


            return jogo;
        }

        public async Task<List<Jogo>> Obter(string nome, string ImageUrl)
        {
            var jogos = new List<Jogo>();

            Query usersRef = DbConnection().Collection("jogos").WhereEqualTo("Nome", nome).WhereEqualTo("ImageUrl", ImageUrl);
            QuerySnapshot snapshot = await usersRef.GetSnapshotAsync();

            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                Dictionary<string, object> documentDictionary = document.ToDictionary();
                jogos.Add(new Jogo
                {
                    Id = Guid.Parse(document.Id),
                    Nome = (string)documentDictionary["Nome"],
                    ImageUrl = (string)documentDictionary["ImageUrl"],
                    Preco = (double)Convert.ToDouble(documentDictionary["Preco"])
                });
            }

            return jogos;
        }

        public async Task Inserir(Jogo jogo)
        {

            DocumentReference docRef = DbConnection().Collection("jogos").Document(jogo.Id.ToString());
            Dictionary<string, object> docDictionary = new Dictionary<string, object>
            {
                { "Nome", jogo.Nome },
                { "ImageUrl", jogo.ImageUrl },
                { "Preco", jogo.Preco }
            };
            await docRef.SetAsync(docDictionary);
        }

        public async Task Atualizar(Jogo jogo)
        {
            DocumentReference docRef = DbConnection().Collection("jogos").Document(jogo.Id.ToString());
            Dictionary<string, object> updates = new Dictionary<string, object>
            {
                { "Nome", jogo.Nome },
                { "ImageUrl", jogo.ImageUrl },
                { "Preco", jogo.Preco }
            };
            await docRef.UpdateAsync(updates);

        }

        public async Task Remover(Guid id)
        {
            DocumentReference docRef = DbConnection().Collection("jogos").Document(id.ToString());
            await docRef.DeleteAsync();
        }

        public void Dispose()
        {
            sqlConnection?.Close();
            sqlConnection?.Dispose();
        }
    }
}
