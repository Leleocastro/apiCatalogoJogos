# apiCatalogoJogos
Repositório para bootcamp Take Blip .NET

## API Catálogo de Jogos

Foi desenvolvida com o intuito de praticar API em .NET.

####GET Jogos:

http://catalogo-jogos.azurewebsites.net/api/V1/Jogos

####GET Pedidos:

http://catalogo-jogos.azurewebsites.net/api/V1/Orders

####POST Jogos:

http://catalogo-jogos.azurewebsites.net/api/V1/Jogos
Id é gerado automaticamente e já adicionado ao firebase
Request Body: application/json
Exemplo aceito:
{
  "nome": "string",
  "imageUrl": "string",
  "preco": 0
}
