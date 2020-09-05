#language: pt-br

Funcionalidade: CriarPedidoCliente
	Para realizar um pedido no sistema
	Devemos consumir uma API


Cenario: Criar pedido cliente
	Dado que o cliente esteja logado
	E que exista um drone
	Quando eu solicitar a criacao de um pedido
	Entao a resposta devera ser um status code 200OK