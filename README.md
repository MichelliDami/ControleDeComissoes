**Sistema de Controle de Comissões**
API para gerenciamento de Invoices e cálculo automático de comissões de vendedores.

**Pré-requisitos**
Antes de rodar o projeto, é necessário ter instalado:
• Docker

**Como rodar o projeto**

1) Clonar o repositório
git clone https://github.com/MichelliDami/ControleDeComissoes.git

3) Entrar na pasta do projeto
A pasta contém a solution (.sln) e o arquivo docker-compose.yml.

5) Subir os containers
docker compose up --build

Esse comando irá buildar a aplicação, subir os containers necessários e iniciar a API.

7) Acessar a documentação da API
http://localhost:8080/swagger/index.html

No Swagger é possível visualizar todos os endpoints, testar requisições e verificar os modelos de dados.

**Observações importantes**

Antes de criar uma Invoice, é necessário cadastrar um Vendedor, pois o endpoint de Invoice não aceita
vendedor nulo. A comissão é calculada automaticamente com base no percentual de comissão do
vendedor.

Fluxo correto de uso da API:
• Criar um Vendedor
• Criar uma Invoice
• A comissão será gerada automaticamente
